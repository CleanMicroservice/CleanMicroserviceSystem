using CleanMicroserviceSystem.Common.Contracts;
using CleanMicroserviceSystem.Oceanus.Application.Abstraction.Configurations;
using CleanMicroserviceSystem.Themis.Application.Repository;
using CleanMicroserviceSystem.Themis.Domain.Identity;
using CleanMicroserviceSystem.Themis.Infrastructure.Persistence;
using CleanMicroserviceSystem.Themis.Infrastructure.Repository;
using Duende.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

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
            .AddAuthorizationCore(options =>
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

        /* TODO: https://docs.duendesoftware.com/identityserver/v6/overview/
         * API Scope to protect CLients and API
         */
        var migrationsAssembly = typeof(ThemisDbContext).Assembly.GetName().Name;
        services
            .AddIdentityServer()
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = builder => builder
                    .UseSqlite(dbConfiguration.ConnectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = builder => builder
                    .UseSqlite(dbConfiguration.ConnectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
            });
        services
            .AddAuthentication()
            .AddOpenIdConnect("oidc_interactive", "CleanMicroserviceSystem.Themis (IdentityServer)", options =>
            {
                options.SignInScheme = IdentityServerConstants.JwtRequestClientKey;
                options.SignOutScheme = IdentityServerConstants.SignoutScheme;
                options.SaveTokens = true;

                options.Authority = "https://demo.duendesoftware.com";
                options.ClientId = "interactive.confidential";
                options.ClientSecret = "secret";
                options.ResponseType = "code";

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role"
                };
            });

        return services;
    }
}
