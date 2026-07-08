using NLog;


namespace WorldRank.Console
{


    public class InMemoryWalletRepository : IWalletRepository
    {

        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private List<IPlayer> _players;

        public InMemoryWalletRepository(List<IPlayer> players)
        {
            _players = players;
        }
        public void AddWallet(Wallet wallet, int playerId)
        {
            var player = _players.Where(item => item.Id == playerId).SingleOrDefault();
            _logger.Info($"Wallet {wallet.Currency} added to player {playerId}");

            if (player != null)
            {
                player.Wallets.Add(wallet.Currency, wallet);
                _logger.Warn($"AddWallet: player {playerId} not found");
            }
        }

        public List<Wallet> GetByPlayer(int playerId)
        {
            var wallets = _players.Where(item => item.Id == playerId).SelectMany(item => item.Wallets.Values);
            return wallets.ToList();
        }
    }

}