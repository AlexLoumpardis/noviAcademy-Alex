using Microsoft.EntityFrameworkCore;
using WorldRank.Application;
using WorldRank.Domain;

namespace WorldRank.Infrastructure
{
    public class DBWalletRepository : IWalletRepository
    {
        private readonly WorldRankDbContext _db;

	public DBWalletRepository(WorldRankDbContext db) => _db = db;

	public async Task Add(Wallet wallet, CancellationToken cancellationToken = default)
	{
            await _db.Wallets.AddAsync(wallet, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
        }


        public Task<Wallet?> GetById(int Id, CancellationToken cancellationToken = default) =>
        _db.Wallets.FirstOrDefaultAsync(w => w.Id == Id, cancellationToken);

        public async Task<IEnumerable<Wallet>> GetByPlayerId(int playerId, CancellationToken cancellationToken = default) =>
            await _db.Wallets.Where(w => w.PlayerId == playerId).ToListAsync(cancellationToken);

        // Read-only leaderboard query — no tracking needed.
        public async Task<IReadOnlyList<Wallet>> GetAll(CancellationToken cancellationToken = default) =>
            await _db.Wallets.AsNoTracking().ToListAsync(cancellationToken);

        public Task SaveChanges(CancellationToken cancellationToken = default) =>
            _db.SaveChangesAsync(cancellationToken);

    }
}