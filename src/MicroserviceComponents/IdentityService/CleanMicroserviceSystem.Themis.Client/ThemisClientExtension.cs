using Microsoft.Extensions.DependencyInjection;

namespace CleanMicroserviceSystem.Themis.Client;

public static class ThemisClientExtension
{
    public static IServiceCollection AddThemisClients(this IServiceCollection services, Action<ThemisClientConfiguration> options)
    {
        _ = services.Configure(options);
        services
            .AddScoped<ThemisApiResourceClient>()
            .AddScoped<ThemisClientClient>()
            .AddScoped<ThemisClientTokenClient>()
            .AddScoped<ThemisRoleClient>()
            .AddScoped<ThemisUserClient>()
            .AddScoped<ThemisUserTokenClient>();
        return services;
    }
}
