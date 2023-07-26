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
                CreatedBy = IdentityContract.AdminUserId,
                Name = IdentityContract.ThemisAPIResource,
                Description = IdentityContract.ThemisAPIResource
            }});
        builder.Entity<Client>().HasData(new[] {
            new Client()
            {
                Id = 1,
                Enabled = true,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = IdentityContract.AdminUserId,
                Name = IdentityContract.TethysClient,
                Description = IdentityContract.TethysClient,
                Secret = "TethysSecret".Sha256(),
            },
            new Client {
                Id = 2,
                Enabled = true,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = IdentityContract.AdminUserId,
                Name = IdentityContract.ThemisNTLMClient,
                Description = IdentityContract.ThemisNTLMClient,
                Secret = "ThemisNTLMSecret".Sha256()
            },
        });
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
            },
            new ClientClaim
            {
                Id = 3,
                ClientId = 2,
                ClaimType = IdentityContract.ThemisAPIResource,
                ClaimValue = IdentityContract.Read
            },
            new ClientClaim
            {
                Id = 4,
                ClientId = 2,
                ClaimType = IdentityContract.ThemisAPIResource,
                ClaimValue = IdentityContract.Write
            }
        });
        return builder;
    }
}
