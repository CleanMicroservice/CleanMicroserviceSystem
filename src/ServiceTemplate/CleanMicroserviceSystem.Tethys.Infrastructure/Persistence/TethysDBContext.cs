using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Entities;
using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Tethys.Infrastructure.Persistence;

public class TethysDBContext : DbContext, IOceanusDBContext
{
    private readonly ILogger<TethysDBContext> logger;

    public TethysDBContext(
        ILogger<TethysDBContext> logger)
        : base()
    {
        this.logger = logger;
    }

    public TethysDBContext(
        ILogger<TethysDBContext> logger,
        DbContextOptions<TethysDBContext> options)
        : base(options)
    {
        this.logger = logger;
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
