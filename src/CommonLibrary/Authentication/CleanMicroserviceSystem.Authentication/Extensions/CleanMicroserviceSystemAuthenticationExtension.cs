using System.Text;
using CleanMicroserviceSystem.Authentication.Configurations;
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
            .AddSingleton<IAuthenticationSchemeProvider, HybridAuthenticationSchemeProvider>()
            .AddScoped<IJwtBearerTokenGenerator, JwtBearerTokenGenerator>()
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = UserJwtBearerKey;
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
                    ValidateAudience = false,
                };
            });

        return services;
    }
}
