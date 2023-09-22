using System.Text;
using CleanMicroserviceSystem.Authentication.Application;
using CleanMicroserviceSystem.Authentication.Configurations;
using CleanMicroserviceSystem.Authentication.Domain;
using CleanMicroserviceSystem.Authentication.Domain.Configurations;
using CleanMicroserviceSystem.Authentication.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace CleanMicroserviceSystem.Authentication.Extensions;

public static class CleanMicroserviceSystemAuthenticationExtension
{
    private const string JwtBearerConfigurationKey = "JwtBearerConfiguration";

    public static IServiceCollection AddJwtBearerAuthentication(
        this IServiceCollection services, IConfiguration configuration)
    {
        services
            .Configure<JwtBearerConfiguration>(configuration.GetRequiredSection(JwtBearerConfigurationKey))
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
                var jwtBearerConfiguration = configuration.GetRequiredSection(JwtBearerConfigurationKey)!.Get<JwtBearerConfiguration>()!;
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
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    RequireAudience = true,
                    RequireSignedTokens = true,
                    RequireExpirationTime = true,
                    ValidIssuer = jwtBearerConfiguration.JwtIssuer,
                    ValidAudience = jwtBearerConfiguration.JwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtBearerConfiguration.JwtSecurityKey))
                };
                options.Validate();
            })
            .AddJwtBearer(IdentityContract.ClientJwtBearerScheme, "CleanMicroserviceSystem Bearer for Client (IdentityServer)", options =>
            {
                var jwtBearerConfiguration = configuration.GetRequiredSection(JwtBearerConfigurationKey)!.Get<JwtBearerConfiguration>()!;
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
                    ValidIssuer = jwtBearerConfiguration.JwtIssuer,
                    ValidAudience = jwtBearerConfiguration.JwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtBearerConfiguration.JwtSecurityKey)),
                };
            });

        return services;
    }

    public static IServiceCollection AddHybridAuthenticationSchemeProvider(
        this IServiceCollection services,
        IEnumerable<AuthenticationSchemeConfiguration> configurations)
    {
        services.AddSingleton<IAuthenticationSchemeProvider, HybridAuthenticationSchemeProvider>();
        foreach (var configuration in configurations)
        {
            services.AddSingleton(configuration);
        }
        return services;
    }
}
