using CleanMicroserviceSystem.Authentication.Domain;
using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Extensions;
using CleanMicroserviceSystem.Themis.Domain.Entities.Configuration;
using Microsoft.EntityFrameworkCore;

namespace CleanMicroserviceSystem.Themis.Infrastructure.DataSeeds
{
    public static class ConfigurationDatabaseInitializer
    {
        public static ModelBuilder InitializeConfigurationDataAsync(this ModelBuilder builder)
        {
            builder.Entity<ApiResource>().HasData(new[] {
                new ApiResource()
                {
                    ID = 1,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = IdentityContract.SuperUserId,
                    Name = ConfigurationContract.ThemisAPIResource,
                    Description = ConfigurationContract.ThemisAPIResource
                }});
            builder.Entity<ApiScope>().HasData(new[] {
                new ApiScope()
                {
                    ID = 1,
                    ApiResourceID = 1,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = IdentityContract.SuperUserId,
                    Name = ConfigurationContract.ThemisAPIReadScope,
                    Description = ConfigurationContract.ThemisAPIReadScope,
                },
                new ApiScope()
                {
                    ID = 2,
                    ApiResourceID = 1,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = IdentityContract.SuperUserId,
                    Name = ConfigurationContract.ThemisAPIWriteScope,
                    Description = ConfigurationContract.ThemisAPIWriteScope,
                }});
            builder.Entity<Client>().HasData(new[] {
                new Client()
                {
                    ID = 1,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = IdentityContract.SuperUserId,
                    Name = ConfigurationContract.TethysClient,
                    Description = ConfigurationContract.TethysClient,
                    Secret = "TethysSecret".Sha256(),
                }});
            builder.Entity<ClientApiScopeMap>().HasData(new[]
            {
                new ClientApiScopeMap()
                {
                    ClientID = 1,
                    ApiScopeID = 1,
                    CreatedBy = IdentityContract.SuperUserId,
                    CreatedOn = DateTime.UtcNow,
                },
                new ClientApiScopeMap()
                {
                    ClientID = 1,
                    ApiScopeID = 2,
                    CreatedBy = IdentityContract.SuperUserId,
                    CreatedOn = DateTime.UtcNow,
                }
            });
            return builder;
        }
    }
}
