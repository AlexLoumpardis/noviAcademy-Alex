using Microsoft.EntityFrameworkCore;
using WorldRank.Domain;

namespace WorldRank.Infrastructure;

public class WorldRankDbContext : DbContext
{
	public DbSet<Player> Players { get; set; } = null!;
    public DbSet<Wallet> Wallets { get; set; } = null!;

    public WorldRankDbContext(DbContextOptions<WorldRankDbContext> options) : base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Player>(x =>
		{
			x.ToTable("Players");
			x.HasKey(y => y.Id);
			x.Property(y => y.Id).ValueGeneratedNever();          
			x.Property(y => y.Name).HasMaxLength(100).IsRequired();
			x.Property(y => y.Score).IsRequired();
		});

        modelBuilder.Entity<Wallet>(x =>
        {
            x.ToTable("Wallets");
            x.HasKey(y => y.Id);
            x.Property(y => y.Id).ValueGeneratedNever();
            x.Property(y => y.Currency).IsRequired();
            x.Property(y => y.Balance).HasPrecision(18 , 2);
            x.Property(y => y.IsBlocked).IsRequired();

            x.HasOne<Player>()
			.WithMany()
			.HasForeignKey(y => y.PlayerId);
        });

        base.OnModelCreating(modelBuilder);
	}
}
