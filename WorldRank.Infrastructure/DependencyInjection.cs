using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WorldRank.Application;

namespace WorldRank.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<WorldRankDbContext>(options =>
            options.UseSqlServer(connectionString));

        // In-memory repositories hold state, so they must live for the whole app (Singleton).
        services.AddSingleton<IPlayerRepository, InMemoryPlayerRepository>();
        services.AddSingleton<IWalletRepository, InMemoryWalletRepository>();

        return services;
    }
}