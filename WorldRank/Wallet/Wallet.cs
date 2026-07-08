namespace WorldRank.Console;

public enum Currency { NONE, EUR, USD, GBP}

public class Wallet
{
	public decimal Balance { get; private set; }
    public Currency Currency { get; }
    public bool IsBlocked { get; private set; }

	public Wallet(Currency currency, decimal initialBalance, bool isBlocked)
	{
        if (initialBalance < 0)

            throw new ArgumentOutOfRangeException(nameof(initialBalance), "Balance cannot be negative.");

        Currency = currency;
        Balance = initialBalance;
        IsBlocked = false;
    }

    public void SetBalance(decimal balance)
    {
        if (balance < 0)
        {
            return;
        }
        Balance = balance;
    }

    public void Withdraw(decimal amount)
    {
        if(amount > Balance)
        {
            throw new InsufficientFundsException("Not enough funds");
        }

        Balance -= amount;
    }
    public override string ToString()
    {
        return "Balance -> " + Balance + " Currency ->" + Currency + " IsBlocked -> " + IsBlocked;
    }

}
