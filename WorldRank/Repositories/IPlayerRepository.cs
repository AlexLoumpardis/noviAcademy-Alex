using WorldRank.Console;

namespace WorldRank.Repositories;

public interface IPlayerRepository
{
    void AddPlayer(Player p);

    Player FindPlayer(int PlayerId);

    void DeletePlayer(int PlayerId);

    IEnumerable<IGrouping<int, Player>> GroupPlayersByScore();
}
