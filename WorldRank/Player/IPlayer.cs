namespace WorldRank.Console
{
    public interface IPlayer
    {
        int Id { get; }
        string Name { get; }
        int Score { get; }

        Dictionary<Currency, Wallet> Wallets { get; set; }

    }
}