using CleanMicroserviceSystem.Oceanus.Application.Abstraction.Configurations;
using CleanMicroserviceSystem.Themis.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CleanMicroserviceSystem.Themis.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        OceanusDBConfiguration dbConfiguration)
    {
        return services
            .AddDbContext<DbContext, ThemisDBContext>(
                options => options
                    .UseSqlite(dbConfiguration.ConnectionString)
                    .UseLazyLoadingProxies());
    }
}
