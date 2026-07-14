using WorldRank.Domain;

namespace WorldRank.Application
{
	public interface IWalletRepository
	{
		Task Add(Wallet wallet, CancellationToken ct);

		Task<Wallet?> GetById(int id, CancellationToken ct);

		Task<IEnumerable<Wallet>> GetByPlayerId(int playerId, CancellationToken ct);

		Task SaveChanges(CancellationToken ct);
	}
}

