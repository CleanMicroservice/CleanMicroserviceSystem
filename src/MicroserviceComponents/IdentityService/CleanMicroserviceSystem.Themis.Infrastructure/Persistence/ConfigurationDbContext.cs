using CleanMicroserviceSystem.Themis.Domain.Entities.Configuration;
using CleanMicroserviceSystem.Themis.Infrastructure.DataSeeds;
using CleanMicroserviceSystem.Themis.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Themis.Infrastructure.Persistence
{
    public class ConfigurationDbContext : DbContext
    {
        private readonly ILogger<ConfigurationDbContext> logger;

        public ConfigurationDbContext(
            ILogger<ConfigurationDbContext> logger)
            : base()
        {
            this.logger = logger;
        }

        public ConfigurationDbContext(
            ILogger<ConfigurationDbContext> logger,
            DbContextOptions<ConfigurationDbContext> options)
            : base(options)
        {
            this.logger = logger;
        }

        public DbSet<ApiResource> ApiResources { get; set; }

        public DbSet<ApiScope> ApiScopes { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<ClientApiScopeMap> ClientApiScopeMaps { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(log => this.logger.LogDebug(log));
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder
                .ConfigureApiResource()
                .ConfigureApiScope()
                .ConfigureClient()
                .ConfigureClientApiScopeMap()
                .InitializeConfigurationDataAsync();
        }
    }
}
