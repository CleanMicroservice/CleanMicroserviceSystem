using CleanMicroserviceSystem.Authentication.Domain;
using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Extensions;
using CleanMicroserviceSystem.Themis.Domain.Entities.Configuration;
using Microsoft.EntityFrameworkCore;

namespace CleanMicroserviceSystem.Themis.Infrastructure.DataSeeds;

public static class ConfigurationDatabaseInitializer
{
    public static ModelBuilder InitializeConfigurationDataAsync(this ModelBuilder builder)
    {
        builder.Entity<ApiResource>().HasData(new[] {
            new ApiResource()
            {
                Id = 1,
                Enabled = true,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = IdentityContract.SuperUserId,
                Name = IdentityContract.ThemisAPIResource,
                Description = IdentityContract.ThemisAPIResource
            }});
        builder.Entity<Client>().HasData(new[] {
            new Client()
            {
                Id = 1,
                Enabled = true,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = IdentityContract.SuperUserId,
                Name = IdentityContract.TethysClient,
                Description = IdentityContract.TethysClient,
                Secret = "TethysSecret".Sha256(),
            }});
        builder.Entity<ClientClaim>().HasData(new[] {
            new ClientClaim()
            {
                Id = 1,
                ClientId = 1,
                ClaimType= IdentityContract.ThemisAPIResource,
                ClaimValue= IdentityContract.Read,
            },
            new ClientClaim()
            {
                Id = 2,
                ClientId = 1,
                ClaimType= IdentityContract.ThemisAPIResource,
                ClaimValue= IdentityContract.Write,
            }});
        return builder;
    }
}
