using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Persistence;
using CleanMicroserviceSystem.Themis.Infrastructure.DataSeeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Themis.Infrastructure.Persistence;

public class ThemisDbContext : OceanusDbContext
{
    private readonly ILogger<ThemisDbContext> logger;

    public ThemisDbContext(
        ILogger<ThemisDbContext> logger)
        : base(logger)
    {
        this.logger = logger;
    }

    public ThemisDbContext(
        ILogger<ThemisDbContext> logger,
        DbContextOptions<ThemisDbContext> options)
        : base(logger, options)
    {
        this.logger = logger;
    }
}
