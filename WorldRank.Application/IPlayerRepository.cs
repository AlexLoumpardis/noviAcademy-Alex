using WorldRank.Domain;

namespace WorldRank.Application
{
    public interface IPlayerRepository
    {
        Task AddPlayer(Player player, CancellationToken ct);

        Task<Player?> GetById(int id, CancellationToken ct);

        Task<IEnumerable<Player>> GetAll(CancellationToken ct);

        Task SaveChanges(CancellationToken ct);
    }
}