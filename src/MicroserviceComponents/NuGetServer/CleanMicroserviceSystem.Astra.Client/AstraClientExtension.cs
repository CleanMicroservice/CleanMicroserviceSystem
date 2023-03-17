using Microsoft.Extensions.DependencyInjection;

namespace CleanMicroserviceSystem.Astra.Client;

public static class AstraClientExtension
{
    public static IServiceCollection AddAstraClients(this IServiceCollection services, AstraClientConfiguration configuration)
    {
        _ = services.Configure(new Action<AstraClientConfiguration>(options =>
        {
            options.GatewayClientName = configuration.GatewayClientName;
            options.ApiKey = configuration.ApiKey;
        }));
        services.AddScoped<AstraNuGetPackageClient>();
        return services;
    }
}
