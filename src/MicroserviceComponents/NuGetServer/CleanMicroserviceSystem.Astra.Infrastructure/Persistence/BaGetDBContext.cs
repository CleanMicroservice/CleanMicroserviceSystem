using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Astra.Infrastructure.Persistence;

public class BaGetDBContext : AbstractContext<BaGetDBContext>
{
    private const int SqliteUniqueConstraintViolationErrorCode = 19;

    public BaGetDBContext(
        ILogger<BaGetDBContext> logger)
        : base(logger)
    {
    }

    public BaGetDBContext(
        ILogger<BaGetDBContext> logger,
        DbContextOptions<BaGetDBContext> options)
        : base(logger, options)
    {
    }

    public override bool IsUniqueConstraintViolationException(DbUpdateException exception)
    {
        return exception.InnerException is SqliteException sqliteException &&
            sqliteException.SqliteErrorCode == SqliteUniqueConstraintViolationErrorCode;
    }
}
