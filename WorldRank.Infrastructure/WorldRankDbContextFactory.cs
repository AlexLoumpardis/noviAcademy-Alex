using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WorldRank.Infrastructure;


public class WorldRankDbContextFactory : IDesignTimeDbContextFactory<WorldRankDbContext>
{
	public WorldRankDbContext CreateDbContext(string[] args)
	{
		var options = new DbContextOptionsBuilder<WorldRankDbContext>()
			.UseSqlServer("Server=localhost;Database=WorldRank;Integrated Security=true;TrustServerCertificate=true")
			.Options;

		return new WorldRankDbContext(options);
	}
}
