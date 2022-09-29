using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Entities;
using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Persistence;

public class OceanusDBContext : IdentityDbContext<OceanusUser, OceanusRole, int>
{
    public OceanusDBContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<WebAPILog> WebAPILogs { get; set; }

    public DbSet<GenericOption> GenericOptions { get; set; }
}
