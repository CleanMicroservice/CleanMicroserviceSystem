using CleanMicroserviceSystem.Gateway.Configurations;
using CleanMicroserviceSystem.Gateway.Extensions;
using CleanMicroserviceSystem.Oceanus.Application.Abstraction.Repository;
using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IO;
using Microsoft.OpenApi.Models;

namespace CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Extensions;

public static class OceanusServicesExtension
{
    public static IServiceCollection AddOceanusServices(
        this IServiceCollection services,
        AgentServiceRegistrationConfiguration agentServiceRegistrationConfiguration)
    {
        services
            .AddScoped<IGenericOptionRepository, GenericOptionRepository>()
            .AddScoped<IWebAPILogRepository, WebAPILogRepository>()
            .AddScoped<RecyclableMemoryStreamManager>()
            .AddGatewayServiceRegister(agentServiceRegistrationConfiguration);

        return services;
    }

    public static IServiceCollection AddOceanusSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition(
                JwtBearerDefaults.AuthenticationScheme,
                new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Description = "Please input token",
                    BearerFormat = "Jwt",
                    Name = "Authorization",
                    Scheme = JwtBearerDefaults.AuthenticationScheme.ToLowerInvariant(),
                    Type = SecuritySchemeType.Http
                });
            options.AddSecurityRequirement(
                new OpenApiSecurityRequirement() {
                    {
                        new OpenApiSecurityScheme() { Reference = new OpenApiReference()
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        } },
                        Array.Empty<string>()
                    }
                });
        });
        return services;
    }
}
