using Microsoft.EntityFrameworkCore;
using WorldRank.Application;
using WorldRank.Domain;
using NLog;

namespace WorldRank.Infrastructure
{
    public class DBPlayerRepository : IPlayerRepository
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
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
            {
                _logger.Warn("Delete skipped: player {PlayerId} not found", playerId);
                return;
            }

            _context.Players.Remove(player);
            _context.SaveChanges();
        }

        public Player? FindPlayer(int playerId)
        {
            return _context.Players.FirstOrDefault(p => p.Id == playerId);
        }

        public IEnumerable<Player> GetAllPlayers()
        {

           return _context.Players.ToList();
        }

        public IEnumerable<IGrouping<int, Player>> GroupPlayersByScore()
        {
            return _context.Players
            .AsEnumerable()                     
            .GroupBy(p => p.Score)
            .OrderByDescending(g => g.Key);
        }
    }
}
