using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Entities;
using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Persistence;
using CleanMicroserviceSystem.Themis.Domain.Identity;
using CleanMicroserviceSystem.Themis.Infrastructure.DataSeeds;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Themis.Infrastructure.Persistence;

public class ThemisDBContext : IdentityDbContext<OceanusUser, OceanusRole, int>, IOceanusDBContext
{
    private readonly ILogger<ThemisDBContext> logger;

    public ThemisDBContext(
        ILogger<ThemisDBContext> logger)
        : base()
    {
        this.logger = logger;
    }

    public ThemisDBContext(
        ILogger<ThemisDBContext> logger,
        DbContextOptions<ThemisDBContext> options)
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
        (this as IOceanusDBContext).OnCommonModelCreating(builder);
        base.OnModelCreating(builder);

        builder.InitializeIdentityData();
    }
}
