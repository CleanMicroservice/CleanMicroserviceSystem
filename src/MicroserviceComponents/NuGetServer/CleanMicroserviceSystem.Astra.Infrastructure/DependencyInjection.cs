using CleanMicroserviceSystem.Astra.Application.Configurations;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Configuration;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Extensions;
using CleanMicroserviceSystem.Astra.Infrastructure.Persistence;
using CleanMicroserviceSystem.Astra.Infrastructure.Services;
using CleanMicroserviceSystem.Authentication.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanMicroserviceSystem.Astra.Infrastructure;

public static class DependencyInjection
{
    private const string NuGetServerConfigurationKey = "NuGetServerConfiguration";

    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ServiceDB")!;
        _ = services
            .AddCors(options => options
                .AddDefaultPolicy(builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()))
            .AddAuthorization(options =>
            {
                options.AddPolicy(IdentityContract.AstraAPIReadPolicyName, IdentityContract.AstraAPIReadPolicy);
                options.AddPolicy(IdentityContract.AstraAPIWritePolicyName, IdentityContract.AstraAPIWritePolicy);
                options.AddPolicy(IdentityContract.AstraAPIDeletePolicyName, IdentityContract.AstraAPIDeletePolicy);
            })
            .AddDbContext<DbContext, AstraDbContext>(options => options
                .UseSqlite(connectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .UseLazyLoadingProxies());
        _ = services
            .AddTransient<IUrlGenerator, BaGetUrlGenerator>()
            .AddBaGetDbContextProvider<BaGetDBContext>("Sqlite", (provider, options) => options
                .UseSqlite(connectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .UseLazyLoadingProxies())
            .Configure<BaGetOptions>(options =>
            {
                options.PackageDeletionBehavior = PackageDeletionBehavior.HardDelete;
                options.ApiKey = configuration.GetRequiredSection(NuGetServerConfigurationKey)!.Get<NuGetServerConfiguration>()!.ApiKey;
            })
            .AddBaGetApplication(bagetApplication =>
            {
                _ = bagetApplication.AddFileStorage(options =>
                {
                    options.Path = configuration.GetRequiredSection(NuGetServerConfigurationKey)!.Get<NuGetServerConfiguration>()!.PackagePath;
                });
            });
        return services;
    }
}
