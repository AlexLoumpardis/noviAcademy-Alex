

namespace WorldRank.Console
{
    public class InsufficientFundsException : WalletException
    {
        public InsufficientFundsException(string message) : base(message) { }
    }
}
