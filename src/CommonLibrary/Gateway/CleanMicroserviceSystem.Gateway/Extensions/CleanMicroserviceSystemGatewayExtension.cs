using CleanMicroserviceSystem.Gateway.Configurations;
using Consul;
using Microsoft.Extensions.DependencyInjection;

namespace CleanMicroserviceSystem.Gateway.Extensions;

public static class CleanMicroserviceSystemGatewayExtension
{
    public static IServiceCollection AddGatewayServiceRegister(
        this IServiceCollection services,
        AgentServiceRegistrationConfiguration configuration)
    {
        var registration = new AgentServiceRegistration()
        {
            ID = configuration.ServiceInstanceId,
            Name = configuration.ServiceName,
            Address = configuration.SelfHost,
            Port = configuration.SelfPort,
            Check = new AgentServiceCheck()
            {
                Interval = TimeSpan.FromSeconds(60),
                Timeout = TimeSpan.FromSeconds(30),
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(30),
                HTTP = configuration.HealthCheckUrl,
                Status = HealthStatus.Passing,
            }
        };
        var consulClient = new ConsulClient();
        consulClient.Agent.ServiceRegister(registration);
        services.AddSingleton(consulClient);
        return services;
    }
}
