using Microsoft.Extensions.DependencyInjection;

namespace CleanMicroserviceSystem.Hermes.Client;

public static class K8sAgentClientExtension
{
    public static IServiceCollection AddHermesClients(this IServiceCollection services, Action<K8sAgentClientConfiguration> options)
    {
        _ = services.Configure(options);
        services
            .AddScoped<K8sAgentClient>();
        return services;
    }
}
