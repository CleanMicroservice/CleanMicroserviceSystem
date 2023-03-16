using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Entities;
using CleanMicroserviceSystem.Astra.Infrastructure.Persistence.Configurations;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Astra.Infrastructure.Persistence;

public class BaGetDBContext : DbContext, IContext
{
    private const int SqliteUniqueConstraintViolationErrorCode = 19;
    private readonly ILogger<BaGetDBContext> logger;

    public BaGetDBContext(
        ILogger<BaGetDBContext> logger)
        : base()
    {
        this.logger = logger;
    }

    public BaGetDBContext(
        ILogger<BaGetDBContext> logger,
        DbContextOptions<BaGetDBContext> options)
        : base(options)
    {
        this.logger = logger;
    }

    public Task<int> SaveChangesAsync() => SaveChangesAsync(default);

    public virtual async Task RunMigrationsAsync(CancellationToken cancellationToken)
        => await Database.MigrateAsync(cancellationToken);

    public virtual bool SupportsLimitInSubqueries => true;

    public bool IsUniqueConstraintViolationException(DbUpdateException exception)
    {
        return exception.InnerException is SqliteException sqliteException &&
            sqliteException.SqliteErrorCode == SqliteUniqueConstraintViolationErrorCode;
    }

    public DbSet<Package> Packages { get; set; }

    public DbSet<PackageDependency> PackageDependencies { get; set; }

    public DbSet<PackageType> PackageTypes { get; set; }

    public DbSet<TargetFramework> TargetFrameworks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(log => this.logger.LogDebug(log));
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ConfigureBaGet();
    }
}
