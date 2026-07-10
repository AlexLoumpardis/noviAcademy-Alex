using Microsoft.EntityFrameworkCore;
using WorldRank.Domain;

namespace WorldRank.Infrastructure;

public class WorldRankDbContext : DbContext
{
	public DbSet<Player> Players { get; set; } = null!;

	public WorldRankDbContext(DbContextOptions<WorldRankDbContext> options) : base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Player>(x =>
		{
			x.ToTable("Players");
			x.HasKey(y => y.Id);
			x.Property(y => y.Id).ValueGeneratedNever();          // Id is assigned by the app, not the DB
			x.Property(y => y.Name).HasMaxLength(100).IsRequired();
			x.Property(y => y.Score).IsRequired();
		});

		base.OnModelCreating(modelBuilder);
	}
}
