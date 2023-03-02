using System.Text;
using CleanMicroserviceSystem.Authentication.Application;
using CleanMicroserviceSystem.Authentication.Configurations;
using CleanMicroserviceSystem.Authentication.Domain;
using CleanMicroserviceSystem.Authentication.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace CleanMicroserviceSystem.Authentication.Extensions;
public static class CleanMicroserviceSystemAuthenticationExtension
{
    public static IServiceCollection AddJwtBearerAuthentication(
        this IServiceCollection services,
        JwtBearerConfiguration configuration)
    {
        services
            .Configure(new Action<JwtBearerConfiguration>(options =>
            {
                options.JwtAudience = configuration.JwtAudience;
                options.JwtExpiryForUser = configuration.JwtExpiryForUser;
                options.JwtExpiryForClient = configuration.JwtExpiryForClient;
                options.JwtIssuer = configuration.JwtIssuer;
                options.JwtSecurityKey = configuration.JwtSecurityKey;
            }))
            .AddHybridAuthenticationSchemeProvider(new AuthenticationSchemeConfiguration[]
            {
                new AuthenticationSchemeConfiguration(
                    IdentityContract.ClientJwtBearerScheme,
                    context =>
                        context.Request.Headers.TryGetValue(IdentityContract.AuthenticationSchemeHeaderName, out var headerValue) &&
                        IdentityContract.ClientAuthenticationSchemeHeaderValue.Equals(headerValue, StringComparison.OrdinalIgnoreCase)),
            })
            .AddScoped<IJwtBearerTokenGenerator, JwtBearerTokenGenerator>()
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityContract.UserJwtBearerScheme;
                options.DefaultChallengeScheme = IdentityContract.ClientJwtBearerScheme;
            })
            .AddJwtBearer(IdentityContract.UserJwtBearerScheme, "CleanMicroserviceSystem Bearer for User (IdentityServer)", options =>
            {
                options.Events = new JwtBearerEvents()
                {
                    OnForbidden = context => Task.CompletedTask,
                    OnAuthenticationFailed = context => Task.CompletedTask,
                };
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    RequireAudience = true,
                    RequireSignedTokens = true,
                    RequireExpirationTime = true,
                    ValidIssuer = configuration.JwtIssuer,
                    ValidAudience = configuration.JwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.JwtSecurityKey))
                };
                options.Validate();
            })
            .AddJwtBearer(IdentityContract.ClientJwtBearerScheme, "CleanMicroserviceSystem Bearer for Client (IdentityServer)", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    RequireAudience = true,
                    RequireSignedTokens = true,
                    RequireExpirationTime = true,
                    ValidIssuer = configuration.JwtIssuer,
                    ValidAudience = configuration.JwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.JwtSecurityKey)),
                };
            });

        return services;
    }

    public static IServiceCollection AddHybridAuthenticationSchemeProvider(
        this IServiceCollection services,
        IEnumerable<AuthenticationSchemeConfiguration> configurations)
    {
        services.AddSingleton<IAuthenticationSchemeProvider, HybridAuthenticationSchemeProvider>();
        services.Configure<AuthenticationSchemeConfigurations>(options =>
        {
            options.SchemeConfigurations = configurations;
        });
        return services;
    }
}
