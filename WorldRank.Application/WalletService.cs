using Microsoft.Extensions.Logging;
using WorldRank.Domain;

namespace WorldRank.Application;

// The service layer owns the cache (cache-aside reads, write-through on writes).
// Cross-entity validation (does the player exist? is there already a wallet in
// this currency?) lives here, not in the repository.
public class WalletService : IWalletService
{
	private static readonly TimeSpan Ttl = TimeSpan.FromSeconds(60);

	private readonly IWalletRepository _wallets;
	private readonly IPlayerRepository _players;
	private readonly ICache _cache;
	private readonly ILogger<WalletService> _logger;

	public WalletService(IWalletRepository wallets, IPlayerRepository players, ICache cache, ILogger<WalletService> logger)
	{
		_wallets = wallets;
		_players = players;
		_cache = cache;
		_logger = logger;
	}

	private static string WalletKey(int id) => $"wallet:{id}";
	private static string PlayerWalletsKey(int playerId) => $"wallets:player:{playerId}";

	public async Task<Wallet> Create(int playerId, Currency currency, decimal initialBalance, CancellationToken ct)
	{
		if (await _players.GetById(playerId, ct) is null)
			throw new PlayerNotFoundException(playerId);

		var existing = await _wallets.GetByPlayerId(playerId, ct);
		if (existing.Any(w => w.Currency == currency))
			throw new DuplicateWalletException(playerId, currency);

		var wallet = new Wallet(Random.Shared.Next(1, int.MaxValue), playerId, currency, initialBalance);
		await _wallets.Add(wallet, ct);
		_logger.LogInformation("Wallet created {WalletId} for player {PlayerId} in {Currency}", wallet.Id, playerId, currency);

		Refresh(wallet);
		return wallet;
	}

	public async Task<Wallet?> GetById(int id, CancellationToken ct)
	{
		if (_cache.TryGet(WalletKey(id), out Wallet? cached) && cached is not null)
		{
			_logger.LogInformation("Cache HIT wallet {WalletId}", id);
			return cached;
		}

		_logger.LogInformation("Cache MISS wallet {WalletId} — loading from database", id);
		var wallet = await _wallets.GetById(id, ct);
		if (wallet is not null)
			_cache.Set(WalletKey(id), wallet, Ttl);
		return wallet;
	}

	public async Task<Wallet?> Deposit(int walletId, decimal amount, CancellationToken ct)
	{
		var wallet = await _wallets.GetById(walletId, ct);
		if (wallet is null)
			return null;

		wallet.Deposit(amount);
		await _wallets.SaveChanges(ct);
		_logger.LogInformation("Deposited {Amount} to wallet {WalletId}; new balance {Balance}", amount, walletId, wallet.Balance);

		Refresh(wallet);
		return wallet;
	}

	private void Refresh(Wallet wallet)
	{
		_cache.Set(WalletKey(wallet.Id), wallet, Ttl);
		_cache.Remove(PlayerWalletsKey(wallet.PlayerId));
		_logger.LogInformation("Cache write-through wallet {WalletId}; list cache invalidated", wallet.Id);
	}
}
