using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CleanMicroserviceSystem.Tethys.Infrastructure.Persistence;

public class TethysDBContext : OceanusDBContext
{
    public TethysDBContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
