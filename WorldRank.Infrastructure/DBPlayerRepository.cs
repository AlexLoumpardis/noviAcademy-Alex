using Microsoft.EntityFrameworkCore;
using WorldRank.Application;
using WorldRank.Domain;
using WorldRank.Infrastructure;

public class DBPlayerRepository : IPlayerRepository
{
    private readonly WorldRankDbContext _context;

    public DBPlayerRepository(WorldRankDbContext context) => _context = context;

    public async Task AddPlayer(Player player, CancellationToken ct)
    {
        await _context.Players.AddAsync(player, ct);
        await _context.SaveChangesAsync(ct);
    }

    public Task<Player?> GetById(int id, CancellationToken ct) =>
        _context.Players.FirstOrDefaultAsync(p => p.Id == id, ct);   // ΧΩΡΙΣ AsNoTracking — θέλουμε tracking εδώ

    public async Task<IEnumerable<Player>> GetAll(CancellationToken ct) =>
        await _context.Players.AsNoTracking().ToListAsync(ct);

    public Task SaveChanges(CancellationToken ct) => _context.SaveChangesAsync(ct);
}