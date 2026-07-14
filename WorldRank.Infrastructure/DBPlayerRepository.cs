using Microsoft.EntityFrameworkCore;
using WorldRank.Application;
using WorldRank.Domain;

namespace WorldRank.Infrastructure
{
    public class DBPlayerRepository : IPlayerRepository
    {
        private WorldRankDbContext _context;

        public DBPlayerRepository(WorldRankDbContext context)
        {
            _context = context;
        }

        public void AddPlayer(Player player)
        {
            _context.Players.Add(player) ;
            _context.SaveChanges();
        }

        public void DeletePlayer(int playerId)
        {
            var player = _context.Players.Where(item => item.Id == playerId).FirstOrDefault();

            if (player is null)
                return;

            _context.Players.Remove(player);
            _context.SaveChanges();
        }

        public Player? FindPlayer(int playerId)
        {
            return _context.Players.AsNoTracking().FirstOrDefault(p => p.Id == playerId);
        }

        public IEnumerable<Player> GetAllPlayers()
        {
            return _context.Players.AsNoTracking().ToList();
        }

        public IEnumerable<IGrouping<int, Player>> GroupPlayersByScore()
        {
            return _context.Players
            .AsNoTracking()
            .AsEnumerable()
            .GroupBy(p => p.Score)
            .OrderByDescending(g => g.Key);
        }
    }
}
