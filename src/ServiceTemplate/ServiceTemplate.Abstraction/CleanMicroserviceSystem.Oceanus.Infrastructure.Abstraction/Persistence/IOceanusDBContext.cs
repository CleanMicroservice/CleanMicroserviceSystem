using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Entities;
using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Persistence;

public interface IOceanusDBContext
{
    public DbSet<WebAPILog> WebAPILogs { get; set; }

    public DbSet<GenericOption> GenericOptions { get; set; }

    public void OnCommonModelCreating(ModelBuilder builder)
    {
        builder.ConfigureGenericOption();
        builder.ConfigureWebAPILog();
    }
}
