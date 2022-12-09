using CleanMicroserviceSystem.Oceanus.Application.Abstraction.Repository;
using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IO;

namespace CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Extensions;

public static class OceanusRepositoryExtension
{
    public static IServiceCollection AddOceanusRepositoryServices(
    this IServiceCollection services)
    {
        services
            .AddScoped<IGenericOptionRepository, GenericOptionRepository>()
            .AddScoped<IWebAPILogRepository, WebAPILogRepository>()
            .AddScoped<RecyclableMemoryStreamManager>();

        return services;
    }
}
