using BaGet;
using BaGet.Core;
using CleanMicroserviceSystem.Astra.Application.Configurations;
using CleanMicroserviceSystem.Astra.Infrastructure.Persistence;
using CleanMicroserviceSystem.Astra.Infrastructure.Services;
using CleanMicroserviceSystem.Authentication.Domain;
using CleanMicroserviceSystem.Oceanus.Application.Abstraction.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CleanMicroserviceSystem.Astra.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        OceanusDbConfiguration dbConfiguration,
        NuGetServerConfiguration nuGetConfiguration)
    {
        services
            .AddCors(options => options
                .AddDefaultPolicy(builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()))
            .AddAuthorization(options =>
            {
                options.AddPolicy(IdentityContract.AstraAPIReadPolicyName, IdentityContract.AstraAPIReadPolicy);
                options.AddPolicy(IdentityContract.AstraAPIWritePolicyName, IdentityContract.AstraAPIWritePolicy);
            })
            .AddDbContext<DbContext, AstraDbContext>(options => options
                .UseSqlite(dbConfiguration.ConnectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .UseLazyLoadingProxies());
        services
            .AddTransient<IUrlGenerator, BaGetUrlGenerator>()
            .AddDbContext<BaGetDBContext>(options => options
                .UseSqlite(dbConfiguration.ConnectionString)
            )
            .AddBaGetApplication(bagetApplication =>
            {
                bagetApplication.AddSqliteDatabase(options =>
                {
                    options.ConnectionString = dbConfiguration.ConnectionString;
                });
                bagetApplication.AddFileStorage(options =>
                {
                    options.Path = nuGetConfiguration.PackagePath;
                });
            });
        return services;
    }
}
