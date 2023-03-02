using Microsoft.Extensions.DependencyInjection;

namespace CleanMicroserviceSystem.Themis.Client;
public static class ThemisClientExtension
{
    public static IServiceCollection AddThemisClients(this IServiceCollection services, ThemisClientConfiguration configuration)
    {
        _ = services.Configure(new Action<ThemisClientConfiguration>(options =>
        {
            options.GatewayClientName = configuration.GatewayClientName;
        }));
        return services;
    }
}
