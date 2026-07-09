namespace WorldRank.Console
    ;

public interface IWalletRepository
{
    void AddWallet(Wallet wallet, int playerId);

    List<Wallet> GetByPlayer(int playerId);
}