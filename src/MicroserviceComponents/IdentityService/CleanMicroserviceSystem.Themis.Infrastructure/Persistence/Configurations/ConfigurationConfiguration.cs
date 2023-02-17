using CleanMicroserviceSystem.Themis.Domain.Entities.Configuration;
using Microsoft.EntityFrameworkCore;

namespace CleanMicroserviceSystem.Themis.Infrastructure.Persistence.Configurations
{
    public static class ConfigurationConfiguration
    {
        public static ModelBuilder ConfigureApiResource(this ModelBuilder modelBuilder)
        {
            var apiResourceBuilder = modelBuilder.Entity<ApiResource>();
            apiResourceBuilder.HasKey(nameof(ApiResource.ID));
            apiResourceBuilder.HasIndex(nameof(ApiResource.Name)).IsUnique();
            apiResourceBuilder.Property(nameof(ApiResource.Name)).UseCollation("NOCASE");
            apiResourceBuilder.HasMany(r => r.ApiScopes).WithOne(s => s.ApiResource).HasForeignKey(s => s.ApiResourceID).IsRequired();
            return modelBuilder;
        }

        public static ModelBuilder ConfigureApiScope(this ModelBuilder modelBuilder)
        {
            var apiScopeBuilder = modelBuilder.Entity<ApiScope>();
            apiScopeBuilder.HasKey(nameof(ApiScope.ID));
            apiScopeBuilder.HasIndex(nameof(ApiScope.ApiResourceID), nameof(ApiScope.Name)).IsUnique();
            apiScopeBuilder.Property(nameof(ApiScope.Name)).UseCollation("NOCASE");
            return modelBuilder;
        }

        public static ModelBuilder ConfigureClient(this ModelBuilder modelBuilder)
        {
            var clientBuilder = modelBuilder.Entity<Client>();
            clientBuilder.HasKey(nameof(Client.ID));
            clientBuilder.HasIndex(nameof(Client.Name)).IsUnique();
            clientBuilder.Property(nameof(Client.Name)).UseCollation("NOCASE");
            return modelBuilder;
        }

        public static ModelBuilder ConfigureClientApiScopeMap(this ModelBuilder modelBuilder)
        {
            var clientApiScopeMapBuilder = modelBuilder.Entity<ClientApiScopeMap>();
            clientApiScopeMapBuilder.HasKey(nameof(ClientApiScopeMap.ClientID), nameof(ClientApiScopeMap.ApiScopeID));
            clientApiScopeMapBuilder.HasOne(map => map.Client).WithMany(client => client.ApiScopesMaps).HasForeignKey(map => map.ClientID).OnDelete(DeleteBehavior.Cascade);
            clientApiScopeMapBuilder.HasOne(map => map.ApiScope).WithMany(client => client.ClientMaps).HasForeignKey(map => map.ApiScopeID).OnDelete(DeleteBehavior.Cascade);
            return modelBuilder;
        }
    }
}
