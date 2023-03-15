using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Astra.Infrastructure.Persistence;

public class AstraDbContext : OceanusDbContext
{
    public AstraDbContext(
        ILogger<AstraDbContext> logger)
        : base(logger)
    {
    }

    public AstraDbContext(
        ILogger<AstraDbContext> logger,
        DbContextOptions<AstraDbContext> options)
        : base(logger, options)
    {
    }
}
