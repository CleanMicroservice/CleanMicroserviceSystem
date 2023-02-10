using System.Security.Cryptography;
using System.Text;
using CleanMicroserviceSystem.Authentication.Configurations;
using CleanMicroserviceSystem.Authentication.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
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
                options.JwtExpiryInMinutes = configuration.JwtExpiryInMinutes;
                options.JwtIssuer = configuration.JwtIssuer;
                options.JwtSecurityKey = configuration.JwtSecurityKey;
            }))
            .AddScoped<IJwtBearerTokenGenerator, JwtBearerTokenGenerator>()
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, "CleanMicroserviceSystem Bearer (IdentityServer)", options =>
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
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, "CleanMicroserviceSystem OpenIdConnect (IdentityServer)", options =>
            {
                options.Authority = "https://localhost:5001";
                options.ClientId = "web";
                options.ClientSecret = "secret";
                options.ResponseType = "code";
                options.SaveTokens = true;
                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("offline_access");
                options.Scope.Add("api1");
                options.GetClaimsFromUserInfoEndpoint = true;
            });

        return services;
    }
}
