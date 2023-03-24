using Microsoft.Extensions.DependencyInjection;

namespace CleanMicroserviceSystem.Astra.Client;

public static class AstraClientExtension
{
    public static IServiceCollection AddAstraClients(this IServiceCollection services, Action<AstraClientConfiguration> options)
    {
        _ = services.Configure(options);
        services.AddScoped<AstraNuGetPackageClient>();
        return services;
    }
}
