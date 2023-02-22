﻿using CleanMicroserviceSystem.Aphrodite.Application.Configurations;
using CleanMicroserviceSystem.Aphrodite.Domain;
using CleanMicroserviceSystem.Aphrodite.Infrastructure.Services;
using CleanMicroserviceSystem.Aphrodite.Infrastructure.Services.Authentication;
using CleanMicroserviceSystem.Authentication.Domain;
using Microsoft.AspNetCore.Components.Authorization;
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
        services.AddAuthorizationCore(options =>
        {
            options.AddPolicy(IdentityContract.ThemisAPIReadPolicyName, IdentityContract.ThemisAPIReadPolicy);
            options.AddPolicy(IdentityContract.ThemisAPIWritePolicyName, IdentityContract.ThemisAPIWritePolicy);
        });
        services.AddSingleton<CookieStorage>();
        services.AddSingleton<AphroditeJsonWebTokenParser>();
        services.AddSingleton<AphroditeAuthenticationTokenStore>();
        services.AddSingleton<AuthenticationStateProvider, AphroditeAuthenticationStateProvider>();
        services.AddSingleton<AphroditeAuthenticationClaimsIdentityValidator>();
        services.AddHttpClient(
            ApiContract.AphroditeHttpClientName,
            client => client.BaseAddress = new Uri(configuration.WebUIBaseAddress));
        services.AddHttpClient<HttpClient>(
            ApiContract.GatewayHttpClientName,
            client => client.BaseAddress = new Uri(configuration.GatewayBaseAddress));
        return services;
    }
}
