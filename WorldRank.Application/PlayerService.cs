using Microsoft.Extensions.Logging;
using WorldRank.Domain;

namespace WorldRank.Application;

// The service layer owns the cache (cache-aside reads, write-through on create).
public class PlayerService : IPlayerService
{
	private static readonly TimeSpan Ttl = TimeSpan.FromSeconds(60);

	private readonly IPlayerRepository _players;
	private readonly ICache _cache;
	private readonly ILogger<PlayerService> _logger;

	public PlayerService(IPlayerRepository players, ICache cache, ILogger<PlayerService> logger)
	{
		_players = players;
		_cache = cache;
		_logger = logger;
	}

	private static string PlayerKey(int id) => $"player:{id}";
	private const string AllPlayersKey = "players:all";

	public async Task<Player> Create(string name, int score, CancellationToken ct)
	{
		var player = new Player(Random.Shared.Next(1, int.MaxValue), name);
		player.AddScore(score);

		await _players.AddPlayer(player, ct);
		_logger.LogInformation("Player created {PlayerId} {Name} (score {Score})", player.Id, name, score);

		_cache.Set(PlayerKey(player.Id), player, Ttl);
		_cache.Remove(AllPlayersKey);
		_logger.LogInformation("Cache write-through player {PlayerId}; list cache invalidated", player.Id);
		return player;
	}

	public async Task<Player?> GetById(int id, CancellationToken ct)
	{
		if (_cache.TryGet(PlayerKey(id), out Player? cached) && cached is not null)
		{
			_logger.LogInformation("Cache HIT player {PlayerId}", id);
			return cached;
		}

		_logger.LogInformation("Cache MISS player {PlayerId} — loading from database", id);
		var player = await _players.GetById(id, ct);
		if (player is not null)
			_cache.Set(PlayerKey(id), player, Ttl);
		return player;
	}

	public async Task<IEnumerable<Player>> GetAll(CancellationToken ct)
	{
		if (_cache.TryGet(AllPlayersKey, out IEnumerable<Player>? cached) && cached is not null)
		{
			_logger.LogInformation("Cache HIT all players");
			return cached;
		}

		_logger.LogInformation("Cache MISS all players — loading from database");
		var players = await _players.GetAll(ct);
		_cache.Set(AllPlayersKey, players, Ttl);
		return players;
	}
}
