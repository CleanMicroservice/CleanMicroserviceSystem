using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Entities;
using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Identity;
using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Persistence.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Persistence;

public class OceanusDBContext : IdentityDbContext<OceanusUser, OceanusRole, int>
{
    public OceanusDBContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<WebAPILog> WebAPILogs { get; set; }

    public DbSet<GenericOption> GenericOptions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ConfigureGenericOption();
        builder.ConfigureWebAPILog();

        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}
