using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.DataSeed;
public static class DatabaseInitializer
{
    public static IHost InitializeDatabase(this IHost host)
    {
        if (host == null)
        {
            throw new ArgumentNullException(nameof(host));
        }

        InitializeDatabaseAsync(host).ConfigureAwait(false);

        return host;
    }

    private async static Task InitializeDatabaseAsync(IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<IHost>>();
        var dbContext = services.GetRequiredService<DbContext>();

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
