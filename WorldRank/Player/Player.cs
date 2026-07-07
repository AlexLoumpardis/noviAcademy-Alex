namespace WorldRank.Console;

public class Player : IPlayer
{
	public int Id { get; }
	public string Name { get; }
	public int Score { get; private set; }

    public Dictionary<Currency, Wallet> Wallets { get; set; } = new Dictionary<Currency, Wallet>();

    public Player(int id, string name)
	{
		if (string.IsNullOrEmpty(name))
			throw new ArgumentException("Name cannot be null or empty.", nameof(name));

		Id = id;
		Name = name;
	}

	public void UpdateScore(int newScore)
	{
		if (newScore < 0)
			throw new ArgumentOutOfRangeException(nameof(newScore), "Score cannot be negative.");

		Score = newScore;
	}

    public override string ToString() => $"[{Id}] {Name} - Score: {Score}";
}
