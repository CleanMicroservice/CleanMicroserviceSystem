using CleanMicroserviceSystem.Gateway.Configurations;
using CleanMicroserviceSystem.Gateway.Extensions;
using CleanMicroserviceSystem.Oceanus.Application.Abstraction.Repository;
using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IO;

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
}
