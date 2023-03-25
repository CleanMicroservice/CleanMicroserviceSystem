using CleanMicroserviceSystem.Authentication.Domain;
using CleanMicroserviceSystem.Themis.Application.Repository;
using CleanMicroserviceSystem.Themis.Application.Services;
using CleanMicroserviceSystem.Themis.Domain.Entities.Identity;
using CleanMicroserviceSystem.Themis.Infrastructure.Persistence;
using CleanMicroserviceSystem.Themis.Infrastructure.Repository;
using CleanMicroserviceSystem.Themis.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanMicroserviceSystem.Themis.Infrastructure;

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
            .AddAuthorization(options =>
            {
                options.AddPolicy(IdentityContract.ThemisAPIReadPolicyName, IdentityContract.ThemisAPIReadPolicy);
                options.AddPolicy(IdentityContract.ThemisAPIWritePolicyName, IdentityContract.ThemisAPIWritePolicy);
            })
            .AddScoped<IClientManager, ClientManager>()
            .AddScoped<IApiResourceManager, ApiResourceManager>()
            .AddScoped<IOceanusUserRepository, OceanusUserRepository>()
            .AddScoped<IOceanusRoleRepository, OceanusRoleRepository>()
            .AddScoped<IApiResourceRepository, ApiResourceRepository>()
            .AddScoped<IClientClaimRepository, ClientClaimRepository>()
            .AddScoped<IClientRepository, ClientRepository>()
            .AddDbContext<DbContext, ThemisDbContext>(options => options
                .UseSqlite(connectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .UseLazyLoadingProxies())
            .AddDbContext<IdentityDbContext>(options => options
                .UseSqlite(connectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .UseLazyLoadingProxies())
            .AddDbContext<ConfigurationDbContext>(options => options
                .UseSqlite(connectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .UseLazyLoadingProxies())
            .AddIdentity<OceanusUser, OceanusRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 4;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Lockout.AllowedForNewUsers = true;
                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultTokenProviders();
        return services;
    }
}
