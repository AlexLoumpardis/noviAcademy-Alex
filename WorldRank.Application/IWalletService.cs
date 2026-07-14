using WorldRank.Domain;

namespace WorldRank.Application;

public interface IWalletService
{
	Task<Wallet> Create(int playerId, Currency currency, decimal initialBalance, CancellationToken ct);
	Task<Wallet?> GetById(int id, CancellationToken ct);

	// Fetches, deposits, and persists. Returns null if the wallet does not exist.
	Task<Wallet?> Deposit(int walletId, decimal amount, CancellationToken ct);
}
