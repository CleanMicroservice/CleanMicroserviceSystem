using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Themis.Infrastructure.DataSeeds
{
    public static class ConfigurationDatabaseInitializer
    {
        public async static Task InitializeConfigurationDataAsync(this IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var logger = serviceProvider.GetRequiredService<ILogger<ConfigurationDbContext>>();
            var dbContext = serviceProvider.GetRequiredService<ConfigurationDbContext>();

            logger.LogInformation("Start to initialize Configuration database...");
            try
            {
                if (!dbContext.ApiScopes.Any())
                {
                    await dbContext.ApiScopes.AddRangeAsync(
                        new ApiScope() { Name = "ThemisAPI", DisplayName = "Themis - IdentityService" }.ToEntity());
                }
                if (!dbContext.Clients.Any())
                {
                    await dbContext.Clients.AddRangeAsync(
                        new Client()
                        {
                            ClientId = "Tethys",
                            AllowedGrantTypes = GrantTypes.ClientCredentials,
                            ClientSecrets = { new Secret("TethysSecret".Sha256()) },
                            AllowedScopes = { "ThemisAPI" }
                        }.ToEntity());
                }
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Database initialize failed.");
            }
            finally
            {
                logger.LogInformation($"Database initialize finished.");
            }
        }
    }
}
