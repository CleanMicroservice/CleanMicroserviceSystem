using CleanMicroserviceSystem.Themis.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Themis.Infrastructure.Persistence;

public class IdentityDbContext : IdentityDbContext<OceanusUser, OceanusRole, int>
{
    private readonly ILogger<IdentityDbContext> logger;

    public IdentityDbContext(
        ILogger<IdentityDbContext> logger)
        : base()
    {
        this.logger = logger;
    }

    public IdentityDbContext(
        ILogger<IdentityDbContext> logger,
        DbContextOptions options)
        : base(options)
    {
        this.logger = logger;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(log => this.logger.LogDebug(log));
        base.OnConfiguring(optionsBuilder);
    }
}
