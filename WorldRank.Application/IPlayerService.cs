using WorldRank.Domain;

namespace WorldRank.Application;

public interface IPlayerService
{
	Task<Player> Create(string name, int score, CancellationToken ct);
	Task<Player?> GetById(int id, CancellationToken ct);
	Task<IEnumerable<Player>> GetAll(CancellationToken ct);
}
