using CleanMicroserviceSystem.Oceanus.Application.Abstraction.Configurations;
using CleanMicroserviceSystem.Uranus.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Provider.Consul;

namespace CleanMicroserviceSystem.Uranus.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        OceanusDbConfiguration dbConfiguration)
    {
        services
            .AddCors(options => options
                .AddDefaultPolicy(builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()))
            .AddDbContext<DbContext, UranusDBContext>(
                options => options
                    .UseSqlite(dbConfiguration.ConnectionString)
                    .UseLazyLoadingProxies())
            .AddOcelot()
            .AddConsul();

        return services;
    }
}
