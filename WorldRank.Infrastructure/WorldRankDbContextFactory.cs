using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WorldRank.Infrastructure;

// Used ONLY at design time (Add-Migration / Update-Database) so EF can build the
// context without a running app. Not used when the app runs — that goes through DI.
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
