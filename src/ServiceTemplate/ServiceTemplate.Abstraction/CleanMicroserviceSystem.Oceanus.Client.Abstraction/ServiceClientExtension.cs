using Microsoft.Extensions.DependencyInjection;

namespace CleanMicroserviceSystem.Oceanus.Client.Abstraction;
public static class ServiceClientExtension
{
    public static IServiceCollection ConfigServiceClient(this IServiceCollection services, OceanusServiceClientConfiguration configuration)
    {
        _ = services.Configure(new Action<OceanusServiceClientConfiguration>(options =>
        {
            options.GatewayClientName = configuration.GatewayClientName;
        }));
        return services;
    }
}
