using CleanMicroserviceSystem.Common.Contracts;
using CleanMicroserviceSystem.Oceanus.Application.Abstraction.Configurations;
using CleanMicroserviceSystem.Themis.Application.Repository;
using CleanMicroserviceSystem.Themis.Domain.Identity;
using CleanMicroserviceSystem.Themis.Infrastructure.Persistence;
using CleanMicroserviceSystem.Themis.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CleanMicroserviceSystem.Themis.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        OceanusDBConfiguration dbConfiguration)
    {
        /* TODO: https://docs.duendesoftware.com/identityserver/v6/overview/
         * API Scope to protect CLients and API
         */
        services
            .AddCors(options => options
                .AddDefaultPolicy(builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()))
            .AddAuthorizationCore(options =>
            {
                options.AddPolicy(IdentityContract.AccessUsersPolicy, policy => policy.RequireRole(IdentityContract.AdministratorRole));
                options.AddPolicy(IdentityContract.AccessRolesPolicy, policy => policy.RequireRole(IdentityContract.AdministratorRole));
            })
            .AddScoped<IOceanusUserRepository, OceanusUserRepository>()
            .AddScoped<IOceanusRoleRepository, OceanusRoleRepository>()
            .AddDbContext<DbContext, ThemisDBContext>(options => options
                .UseSqlite(dbConfiguration.ConnectionString)
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
            .AddEntityFrameworkStores<ThemisDBContext>();
        return services;
    }
}
