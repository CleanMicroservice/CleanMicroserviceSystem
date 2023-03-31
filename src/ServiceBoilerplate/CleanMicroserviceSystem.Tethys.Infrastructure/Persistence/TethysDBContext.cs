using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Tethys.Infrastructure.Persistence;

public class TethysDbContext : OceanusDbContext
{
    public TethysDbContext(
        ILogger<TethysDbContext> logger)
        : base(logger)
    {
    }

    public TethysDbContext(
        ILogger<TethysDbContext> logger,
        DbContextOptions<TethysDbContext> options)
        : base(logger, options)
    {
    }
}
