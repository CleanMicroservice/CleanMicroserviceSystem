using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Entities;
using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CleanMicroserviceSystem.Tethys.Infrastructure.Persistence;

public class TethysDBContext : DbContext, IOceanusDBContext
{
    public TethysDBContext() : base() { }

    public TethysDBContext(DbContextOptions options) : base(options)
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
    }
}
