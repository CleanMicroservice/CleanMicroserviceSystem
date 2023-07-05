using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Hermes.Infrastructure.Persistence;

public class HermesDbContext : OceanusDbContext
{
    public HermesDbContext(
        ILogger<HermesDbContext> logger)
        : base(logger)
    {
    }

    public HermesDbContext(
        ILogger<HermesDbContext> logger,
        DbContextOptions<HermesDbContext> options)
        : base(logger, options)
    {
    }
}
