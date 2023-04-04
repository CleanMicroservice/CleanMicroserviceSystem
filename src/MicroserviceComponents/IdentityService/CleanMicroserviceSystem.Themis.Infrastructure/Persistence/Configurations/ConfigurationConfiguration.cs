using CleanMicroserviceSystem.Themis.Domain.Entities.Configuration;
using Microsoft.EntityFrameworkCore;

namespace CleanMicroserviceSystem.Themis.Infrastructure.Persistence.Configurations;

public static class ConfigurationConfiguration
{
    public static ModelBuilder ConfigureApiResource(this ModelBuilder modelBuilder)
    {
        var apiResourceBuilder = modelBuilder.Entity<ApiResource>();
        apiResourceBuilder.HasKey(nameof(ApiResource.Id));
        apiResourceBuilder.HasIndex(nameof(ApiResource.Name)).IsUnique();
        apiResourceBuilder.Property(nameof(ApiResource.Name)).UseCollation("NOCASE");
        return modelBuilder;
    }

    public static ModelBuilder ConfigureClient(this ModelBuilder modelBuilder)
    {
        var clientBuilder = modelBuilder.Entity<Client>();
        clientBuilder.HasKey(nameof(Client.Id));
        clientBuilder.HasIndex(nameof(Client.Name)).IsUnique();
        clientBuilder.Property(nameof(Client.Name)).UseCollation("NOCASE");
        clientBuilder.HasMany(client => client.Claims).WithOne(claim => claim.Client).HasForeignKey(claim => claim.ClientId).OnDelete(DeleteBehavior.Cascade);
        return modelBuilder;
    }

    public static ModelBuilder ConfigureClientClaim(this ModelBuilder modelBuilder)
    {
        var clientClaimBuilder = modelBuilder.Entity<ClientClaim>();
        clientClaimBuilder.HasKey(nameof(ClientClaim.Id));
        clientClaimBuilder.HasIndex(nameof(ClientClaim.ClientId));
        clientClaimBuilder.HasIndex(nameof(ClientClaim.ClaimType), nameof(ClientClaim.ClaimValue));
        return modelBuilder;
    }
}
