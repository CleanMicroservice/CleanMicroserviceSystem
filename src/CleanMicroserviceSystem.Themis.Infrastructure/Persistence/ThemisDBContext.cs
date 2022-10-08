using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CleanMicroserviceSystem.Themis.Infrastructure.Persistence;

public class ThemisDBContext : OceanusDBContext
{
    public ThemisDBContext() : base() { }

    public ThemisDBContext(DbContextOptions options) : base(options)
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
