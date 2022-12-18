using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Entities;
using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Persistence;

public class OceanusDbContext : DbContext
{
    private readonly ILogger<OceanusDbContext> logger;

    public OceanusDbContext(
        ILogger<OceanusDbContext> logger)
        : base()
    {
        this.logger = logger;
    }

    public OceanusDbContext(
        ILogger<OceanusDbContext> logger,
        DbContextOptions options)
        : base(options)
    {
        this.logger = logger;
    }

    public DbSet<WebAPILog> WebAPILogs { get; set; }

    public DbSet<GenericOption> GenericOptions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(log => this.logger.LogDebug(log));
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ConfigureGenericOption();
        builder.ConfigureWebAPILog();
    }
}
