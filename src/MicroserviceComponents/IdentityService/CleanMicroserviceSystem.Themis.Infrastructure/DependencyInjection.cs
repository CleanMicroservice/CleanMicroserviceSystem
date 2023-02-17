﻿using CleanMicroserviceSystem.Authentication.Domain;
using CleanMicroserviceSystem.Oceanus.Application.Abstraction.Configurations;
using CleanMicroserviceSystem.Themis.Application.Repository;
using CleanMicroserviceSystem.Themis.Domain.Entities.Identity;
using CleanMicroserviceSystem.Themis.Infrastructure.Persistence;
using CleanMicroserviceSystem.Themis.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CleanMicroserviceSystem.Themis.Infrastructure;

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
            .AddAuthorization(options =>
            {
                options.AddPolicy(IdentityContract.AccessUsersPolicy, IdentityContract.IsAdministratorRolePolicyBuilder.Build());
                options.AddPolicy(IdentityContract.AccessRolesPolicy, IdentityContract.IsAdministratorRolePolicyBuilder.Build());
            })
            .AddScoped<IOceanusUserRepository, OceanusUserRepository>()
            .AddScoped<IOceanusRoleRepository, OceanusRoleRepository>()
            .AddDbContext<DbContext, ThemisDbContext>(options => options
                .UseSqlite(dbConfiguration.ConnectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution)
                .UseLazyLoadingProxies())
            .AddDbContext<IdentityDbContext>(options => options
                .UseSqlite(dbConfiguration.ConnectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution)
                .UseLazyLoadingProxies())
            .AddDbContext<ConfigurationDbContext>(options => options
                .UseSqlite(dbConfiguration.ConnectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution)
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
            .AddEntityFrameworkStores<IdentityDbContext>();
        return services;
    }
}
