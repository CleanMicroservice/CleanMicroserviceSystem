using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.DataSeed;

public static class DatabaseInitializer
{
    public async static Task InitializeDatabaseAsync(this IServiceProvider services)
        => await services.InitializeDatabaseAsync<DbContext>();

    public async static Task InitializeDatabaseAsync<TDbContext>(this IServiceProvider services)
        where TDbContext : DbContext
    {
        using var scope = services.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var logger = serviceProvider.GetRequiredService<ILogger<TDbContext>>();
        var dbContext = serviceProvider.GetRequiredService<TDbContext>();

        logger.LogInformation("Start to validate database...");
        try
        {
            logger.LogDebug($"Ensure database created...");
            await dbContext.Database.EnsureCreatedAsync();
            logger.LogDebug($"Check database pending migrations...");
            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                logger.LogInformation($"Pending database migrations: \n\t{string.Join(",", pendingMigrations)}");
                await dbContext.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Database validation failed.");
        }
        finally
        {
            logger.LogInformation($"Database validation finished.");
        }
    }
}
