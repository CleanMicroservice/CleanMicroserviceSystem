using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Entities;
using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Persistence;
using CleanMicroserviceSystem.Themis.Domain.Identity;
using CleanMicroserviceSystem.Themis.Infrastructure.DataSeeds;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CleanMicroserviceSystem.Themis.Infrastructure.Persistence;

public class ThemisDBContext : IdentityDbContext<OceanusUser, OceanusRole, int>, IOceanusDBContext
{
    public ThemisDBContext() : base() { }

    public ThemisDBContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<WebAPILog> WebAPILogs { get; set; }
    public DbSet<GenericOption> GenericOptions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        (this as IOceanusDBContext).OnCommonModelCreating(builder);
        base.OnModelCreating(builder);

        builder.InitializeIdentityData();
    }
}
