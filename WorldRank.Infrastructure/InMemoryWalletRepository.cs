using WorldRank.Application;
using WorldRank.Domain;

namespace WorldRank.Infrastructure
{
	public class InMemoryWalletRepository : IWalletRepository
	{
		private readonly List<Wallet> _wallets = new();

		public Task Add(Wallet wallet, CancellationToken ct)
		{
			var exists = _wallets.Any(w => w.PlayerId == wallet.PlayerId && w.Currency == wallet.Currency);

			if (exists)
				throw new DuplicateWalletException(wallet.PlayerId, wallet.Currency);

			_wallets.Add(wallet);
			return Task.CompletedTask;
		}

		public Task<Wallet?> GetById(int id, CancellationToken ct)
		{
			var wallet = _wallets.FirstOrDefault(w => w.Id == id);
			return Task.FromResult(wallet);
		}

		public Task<IEnumerable<Wallet>> GetByPlayerId(int playerId, CancellationToken ct)
		{
			var wallets = _wallets.Where(w => w.PlayerId == playerId);
			return Task.FromResult(wallets);
		}

		public Task SaveChanges(CancellationToken ct)
		{
			// No-op: mutating a wallet fetched from _wallets already mutates it in place.
			return Task.CompletedTask;
		}
	}
}
