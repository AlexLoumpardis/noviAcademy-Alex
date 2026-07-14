using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WorldRank.Application;

namespace WorldRank.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, string connectionString, bool useDatabase)
    {
        services.AddDbContext<WorldRankDbContext>(options =>
            options.UseSqlServer(connectionString));

        if (useDatabase)
        {
            services.AddScoped<IPlayerRepository, DBPlayerRepository>();
            services.AddScoped<IWalletRepository, DBWalletRepository>();
        }
        else
        {
            services.AddSingleton<IPlayerRepository, InMemoryPlayerRepository>();
            services.AddSingleton<IWalletRepository, InMemoryWalletRepository>();
        }

        return services;
    }
}