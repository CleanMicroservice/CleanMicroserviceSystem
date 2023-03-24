using CleanMicroserviceSystem.Uranus.Infrastructure.Middlewares;
using CleanMicroserviceSystem.Uranus.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Provider.Consul;
using Ocelot.Request.Mapper;

namespace CleanMicroserviceSystem.Uranus.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ServiceDB")!;
        services
            .AddCors(options => options
                .AddDefaultPolicy(builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()))
            .AddDbContext<DbContext, UranusDBContext>(
                options => options
                    .UseSqlite(connectionString)
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                    .UseLazyLoadingProxies())
            .AddOcelot()
            .AddConsul()
            .Services.AddSingleton<IRequestMapper, UranusRequestMapper>();

        return services;
    }
}
