using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Themis.Infrastructure.Persistence;

public class ThemisDbContext : OceanusDbContext
{
    public ThemisDbContext(
        ILogger<ThemisDbContext> logger)
        : base(logger)
    {
    }

    public ThemisDbContext(
        ILogger<ThemisDbContext> logger,
        DbContextOptions<ThemisDbContext> options)
        : base(logger, options)
    {
    }
}
