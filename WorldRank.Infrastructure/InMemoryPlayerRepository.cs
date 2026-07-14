using WorldRank.Application;
using WorldRank.Domain;

namespace WorldRank.Infrastructure
{
	public class InMemoryPlayerRepository : IPlayerRepository
	{
		private readonly List<Player> _players = new();

		public Task AddPlayer(Player player, CancellationToken ct)
		{
			_players.Add(player);
			return Task.CompletedTask;
		}

		public Task<Player?> GetById(int id, CancellationToken ct)
		{
			var player = _players.FirstOrDefault(p => p.Id == id);
			return Task.FromResult(player);
		}

		public Task<IEnumerable<Player>> GetAll(CancellationToken ct)
		{
			// Return a copy so callers cannot mutate the repository's internal list.
			return Task.FromResult<IEnumerable<Player>>(_players.ToList());
		}

		public Task SaveChanges(CancellationToken ct)
		{
			// No-op: mutating a player fetched from _players already mutates it in place.
			return Task.CompletedTask;
		}
	}
}
