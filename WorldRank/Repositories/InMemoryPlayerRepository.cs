using WorldRank.Repositories;
using NLog;

namespace WorldRank.Console
{
    public class InMemoryPlayerRepository : IPlayerRepository
    {
        private List<IPlayer> _players;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public InMemoryPlayerRepository(List<IPlayer> players)
        {
            _players = players;
        }
        public void AddPlayer(IPlayer player)
        {
            _players.Add(player);
            _logger.Info($"Player {player.Id} stored");
        }

        public void DeletePlayer(int playerId)
        {
            var player = _players.Where(item => item.Id == playerId).FirstOrDefault();

            if (player != null)
            {
                _players.Remove(player);
                _logger.Info($"Player {playerId} deleted");
            }
        }

        public IPlayer? FindPlayer(int playerId)
        {
            return _players.Where(item => item.Id == playerId).FirstOrDefault();
        }

        public IEnumerable<IGrouping<int, IPlayer>> GroupPlayersByScore()
        {
           return _players.GroupBy(p => p.Score);
        }
    }
}
