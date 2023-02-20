using CleanMicroserviceSystem.Aphrodite.Application.Configurations;
using CleanMicroserviceSystem.Aphrodite.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace CleanMicroserviceSystem.Aphrodite.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureServices(
        this IServiceCollection services,
        AphroditeConfiguration configuration)
    {
        services.AddMasaBlazor();
        services.AddLogging();
        services.AddHttpClient(
            ApiContract.AphroditeHttpClientName,
            client => client.BaseAddress = new Uri(configuration.WebUIBaseAddress));
        services.AddHttpClient<HttpClient>(
            client => client.BaseAddress = new Uri(configuration.GatewayBaseAddress));
        return services;
    }
}
