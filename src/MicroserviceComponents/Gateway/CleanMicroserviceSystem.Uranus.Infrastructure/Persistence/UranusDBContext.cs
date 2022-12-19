using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Uranus.Infrastructure.Persistence;

public class UranusDBContext : OceanusDbContext
{
    public UranusDBContext(
        ILogger<UranusDBContext> logger)
        : base(logger)
    {
    }

    public UranusDBContext(
        ILogger<UranusDBContext> logger,
        DbContextOptions<UranusDBContext> options)
        : base(logger, options)
    {
    }
}
