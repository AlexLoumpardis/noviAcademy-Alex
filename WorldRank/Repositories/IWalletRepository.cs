using NLog;

namespace WorldRank.Console;

public interface IWalletRepository
{

    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

    void AddWallet(Wallet wallet, int playerId);

    List<Wallet> GetByPlayer(int playerId);
}