using IdentityServer4.EntityFramework.DbContexts;
using Entities = IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CleanMicroserviceSystem.Authentication.Domain;

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
                        new Entities.ApiScope()
                        {
                            Name = ConfigurationContract.ThemisAPIReadScope,
                            DisplayName = ConfigurationContract.ThemisAPIReadScope
                        },
                        new Entities.ApiScope()
                        {
                            Name = ConfigurationContract.ThemisAPIWriteScope,
                            DisplayName = ConfigurationContract.ThemisAPIWriteScope
                        });
                }
                if (!dbContext.ApiResources.Any())
                {
                    await dbContext.ApiResources.AddRangeAsync(
                        new Entities.ApiResource()
                        {
                            Name = ConfigurationContract.ThemisAPIResource,
                            DisplayName = ConfigurationContract.ThemisAPIResource,
                            Scopes = new List<Entities.ApiResourceScope> {
                                new Entities.ApiResourceScope() { Scope = ConfigurationContract.ThemisAPIReadScope },
                                new Entities.ApiResourceScope() { Scope = ConfigurationContract.ThemisAPIWriteScope }
                            }
                        });
                }
                if (!dbContext.Clients.Any())
                {
                    await dbContext.Clients.AddRangeAsync(
                        new Entities.Client()
                        {
                            ClientId = ConfigurationContract.TethysClient,
                            AllowedGrantTypes = GrantTypes.ClientCredentials.Select(type => new Entities.ClientGrantType() { GrantType = type }).ToList(),
                            ClientSecrets = new List<Entities.ClientSecret>() {
                                new Entities.ClientSecret() { Value = "TethysSecret".Sha256() }
                            },
                            AllowedScopes = new List<Entities.ClientScope>() {
                                new Entities.ClientScope() { Scope = ConfigurationContract.ThemisAPIReadScope },
                                new Entities.ClientScope() { Scope = ConfigurationContract.ThemisAPIWriteScope }
                            }
                        });
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
