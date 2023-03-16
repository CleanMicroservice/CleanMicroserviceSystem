using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Entities;

public interface IContext
{
    DatabaseFacade Database { get; }

    DbSet<Package> Packages { get; set; }

    bool IsUniqueConstraintViolationException(DbUpdateException exception);

    bool SupportsLimitInSubqueries { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    Task RunMigrationsAsync(CancellationToken cancellationToken);
}