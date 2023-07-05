using System.IdentityModel.Tokens.Jwt;
using CleanMicroserviceSystem.Aphrodite.Application.Configurations;
using CleanMicroserviceSystem.Aphrodite.Domain;
using CleanMicroserviceSystem.Aphrodite.Infrastructure.Services;
using CleanMicroserviceSystem.Aphrodite.Infrastructure.Services.Authentication;
using CleanMicroserviceSystem.Astra.Client;
using CleanMicroserviceSystem.Authentication.Application;
using CleanMicroserviceSystem.Authentication.Domain;
using CleanMicroserviceSystem.Hermes.Client;
using CleanMicroserviceSystem.Themis.Client;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanMicroserviceSystem.Aphrodite.Infrastructure;

public static class DependencyInjection
{
    private const string NuGetServerConfigurationKey = "NuGetServerConfiguration";
    private const string GatewayAPIConfigurationKey = "GatewayAPIConfiguration";

    public static IServiceCollection ConfigureServices(this WebAssemblyHostBuilder builder)
    {
        var config = builder.Configuration;
        var services = builder.Services;
        _ = services.AddMasaBlazor(options =>
        {
            options.ConfigureTheme(theme =>
            {
                theme.Dark = false;
            });
        });
        _ = services.AddLogging();
        _ = services.AddAuthorizationCore(options =>
        {
            options.AddPolicy(IdentityContract.ThemisAPIReadPolicyName, IdentityContract.ThemisAPIReadPolicy);
            options.AddPolicy(IdentityContract.ThemisAPIWritePolicyName, IdentityContract.ThemisAPIWritePolicy);
            options.AddPolicy(IdentityContract.AstraAPIReadPolicyName, IdentityContract.AstraAPIReadPolicy);
            options.AddPolicy(IdentityContract.AstraAPIWritePolicyName, IdentityContract.AstraAPIWritePolicy);
            options.AddPolicy(IdentityContract.AstraAPIDeletePolicyName, IdentityContract.AstraAPIDeletePolicy);
        });
        _ = services
            .AddSingleton<CookieStorage>()
            .AddSingleton<JwtSecurityTokenHandler>()
            .AddSingleton<PageTabsService>()
            .AddSingleton<IAuthenticationTokenStore, AphroditeAuthenticationTokenStore>()
            .AddSingleton<IAuthenticationTokenRefresher, AphroditeAuthenticationTokenRefresher>()
            .AddSingleton<AuthenticationStateProvider, AphroditeAuthenticationStateProvider>()
            .AddSingleton<AphroditeJwtSecurityTokenValidator>();
        _ = services.AddHttpClient(
            ApiContract.AphroditeHttpClientName,
            client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
        _ = services
            .Configure<GatewayAPIConfiguration>(options => config.GetRequiredSection(GatewayAPIConfigurationKey).Bind(options))
            .AddThemisClients(options =>
            {
                options.GatewayClientName = ApiContract.GatewayHttpClientName;
            })
            .AddHermesClients(options =>
            {
                options.GatewayClientName = ApiContract.GatewayHttpClientName;
            })
            .AddAstraClients(options =>
            {
                options.GatewayClientName = ApiContract.GatewayHttpClientName;
                options.ApiKey = config.GetRequiredSection(NuGetServerConfigurationKey)!.Get<NuGetServerConfiguration>()!.ApiKey;
            })
            .AddTransient<DefaultAuthenticationDelegatingHandler>()
            .AddHttpClient<HttpClient>(
                ApiContract.GatewayHttpClientName,
                client => client.BaseAddress = new Uri(config.GetRequiredSection(GatewayAPIConfigurationKey).Get<GatewayAPIConfiguration>()!.GatewayBaseAddress))
            .AddHttpMessageHandler<DefaultAuthenticationDelegatingHandler>();
        return services;
    }
}
