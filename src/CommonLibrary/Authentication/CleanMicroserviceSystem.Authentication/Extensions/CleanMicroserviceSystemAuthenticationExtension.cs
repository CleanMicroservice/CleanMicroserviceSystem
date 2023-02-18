using System.Text;
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
        const string UserJwtBearerKey = $"{JwtBearerDefaults.AuthenticationScheme}_User";
        const string ClientJwtBearerKey = $"{JwtBearerDefaults.AuthenticationScheme}_Client";
        services
            .Configure(new Action<JwtBearerConfiguration>(options =>
            {
                options.JwtAudience = configuration.JwtAudience;
                options.JwtExpiryInMinutes = configuration.JwtExpiryInMinutes;
                options.JwtIssuer = configuration.JwtIssuer;
                options.JwtSecurityKey = configuration.JwtSecurityKey;
            }))
            .AddHybridAuthenticationSchemeProvider(new AuthenticationSchemeConfiguration[]
            {
                new AuthenticationSchemeConfiguration(
                    ClientJwtBearerKey,
                    context =>
                        context.Request.Headers.TryGetValue(IdentityContract.AuthenticationSchemeHeaderName, out var headerValue) &&
                        IdentityContract.ClientAuthenticationSchemeHeaderValue.Equals(headerValue, StringComparison.OrdinalIgnoreCase)),
            })
            .AddScoped<IJwtBearerTokenGenerator, JwtBearerTokenGenerator>()
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = UserJwtBearerKey;
                options.DefaultChallengeScheme = ClientJwtBearerKey;
            })
            .AddJwtBearer(UserJwtBearerKey, "CleanMicroserviceSystem Bearer for User (IdentityServer)", options =>
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
                    // Correction of expiration time's offset
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration.JwtIssuer,
                    ValidAudience = configuration.JwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.JwtSecurityKey))
                };
                options.Validate();
            })
            .AddJwtBearer(ClientJwtBearerKey, "CleanMicroserviceSystem Bearer for Client (IdentityServer)", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    RequireSignedTokens = true,
                    RequireAudience = true,
                    RequireExpirationTime = false,
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
