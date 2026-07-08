using WorldRank.Console;
using NLog;

namespace WorldRank.Repositories;

public interface IPlayerRepository
{

    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

    void AddPlayer(IPlayer p);

    IPlayer FindPlayer(int PlayerId);

    void DeletePlayer(int PlayerId);

    IEnumerable<IGrouping<int, IPlayer>> GroupPlayersByScore();
}
