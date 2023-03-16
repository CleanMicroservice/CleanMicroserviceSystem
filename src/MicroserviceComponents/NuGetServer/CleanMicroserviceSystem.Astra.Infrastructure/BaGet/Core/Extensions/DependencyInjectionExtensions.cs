using System.Net;
using System.Reflection;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Authentication;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Configuration;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Content;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Entities;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Extensions;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Indexing;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Metadata;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Search;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.ServiceIndex;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Storage;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Upstream;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Upstream.Clients;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Validation;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core;

public static partial class DependencyInjectionExtensions
{
    public static IServiceCollection AddBaGetApplication(
        this IServiceCollection services,
        Action<BaGetApplication> configureAction)
    {
        var app = new BaGetApplication(services);

        services.AddConfiguration();
        services.AddBaGetServices();
        services.AddDefaultProviders();

        configureAction(app);

        services.AddFallbackServices();

        return services;
    }

    public static IServiceCollection AddBaGetOptions<TOptions>(
        this IServiceCollection services,
        string key = null)
        where TOptions : class
    {
        _ = services.AddSingleton<IValidateOptions<TOptions>>(new ValidateBaGetOptions<TOptions>(key));
        _ = services.AddSingleton<IConfigureOptions<TOptions>>(provider =>
        {
            var config = provider.GetRequiredService<IConfiguration>();
            if (key != null)
            {
                config = config.GetSection(key);
            }

            return new BindOptions<TOptions>(config);
        });

        return services;
    }

    private static void AddConfiguration(this IServiceCollection services)
    {
        _ = services.AddBaGetOptions<BaGetOptions>();
        _ = services.AddBaGetOptions<DatabaseOptions>(nameof(BaGetOptions.Database));
        _ = services.AddBaGetOptions<FileSystemStorageOptions>(nameof(BaGetOptions.Storage));
        _ = services.AddBaGetOptions<MirrorOptions>(nameof(BaGetOptions.Mirror));
        _ = services.AddBaGetOptions<SearchOptions>(nameof(BaGetOptions.Search));
        _ = services.AddBaGetOptions<StorageOptions>(nameof(BaGetOptions.Storage));
    }

    private static void AddBaGetServices(this IServiceCollection services)
    {
        services.TryAddSingleton<IFrameworkCompatibilityService, FrameworkCompatibilityService>();
        services.TryAddSingleton<IPackageDownloadsSource, PackageDownloadsJsonSource>();

        services.TryAddSingleton<ISearchResponseBuilder, SearchResponseBuilder>();
        services.TryAddSingleton<NuGetClient>();
        services.TryAddSingleton<NullSearchIndexer>();
        services.TryAddSingleton<NullSearchService>();
        services.TryAddSingleton<RegistrationBuilder>();
        services.TryAddSingleton<SystemTime>();
        services.TryAddSingleton<ValidateStartupOptions>();

        services.TryAddSingleton(HttpClientFactory);
        services.TryAddSingleton(NuGetClientFactoryFactory);

        services.TryAddScoped<DownloadsImporter>();

        services.TryAddTransient<IAuthenticationService, ApiKeyAuthenticationService>();
        services.TryAddTransient<IPackageContentService, DefaultPackageContentService>();
        services.TryAddTransient<IPackageDeletionService, PackageDeletionService>();
        services.TryAddTransient<IPackageIndexingService, PackageIndexingService>();
        services.TryAddTransient<IPackageMetadataService, DefaultPackageMetadataService>();
        services.TryAddTransient<IPackageService, PackageService>();
        services.TryAddTransient<IPackageStorageService, PackageStorageService>();
        services.TryAddTransient<IServiceIndexService, BaGetServiceIndex>();
        services.TryAddTransient<ISymbolIndexingService, SymbolIndexingService>();
        services.TryAddTransient<ISymbolStorageService, SymbolStorageService>();

        services.TryAddTransient<DatabaseSearchService>();
        services.TryAddTransient<FileStorageService>();
        services.TryAddTransient<PackageService>();
        services.TryAddTransient<V2UpstreamClient>();
        services.TryAddTransient<V3UpstreamClient>();
        services.TryAddTransient<DisabledUpstreamClient>();
        services.TryAddSingleton<NullStorageService>();
        services.TryAddTransient<PackageDatabase>();

        services.TryAddTransient(UpstreamClientFactory);
    }

    private static void AddDefaultProviders(this IServiceCollection services)
    {
        _ = services.AddProvider((provider, configuration) =>
        {
            return !configuration.HasSearchType("null") ? null : provider.GetRequiredService<NullSearchService>();
        });

        _ = services.AddProvider((provider, configuration) =>
        {
            return !configuration.HasSearchType("null") ? null : provider.GetRequiredService<NullSearchIndexer>();
        });

        _ = services.AddProvider<IStorageService>((provider, configuration) =>
        {
            if (configuration.HasStorageType("filesystem"))
            {
                return provider.GetRequiredService<FileStorageService>();
            }

            return configuration.HasStorageType("null") ? provider.GetRequiredService<NullStorageService>() : (IStorageService?)null;
        });
    }

    private static void AddFallbackServices(this IServiceCollection services)
    {
        services.TryAddScoped<IContext, NullContext>();
        services.TryAddTransient<ISearchIndexer>(provider => provider.GetRequiredService<NullSearchIndexer>());
        services.TryAddTransient<ISearchService>(provider => provider.GetRequiredService<DatabaseSearchService>());
    }

    private static HttpClient HttpClientFactory(IServiceProvider provider)
    {
        var options = provider.GetRequiredService<IOptions<MirrorOptions>>().Value;

        var assembly = Assembly.GetEntryAssembly();
        var assemblyName = assembly.GetName().Name;
        var assemblyVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "0.0.0";

        var client = new HttpClient(new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
        });

        client.DefaultRequestHeaders.Add("User-Agent", $"{assemblyName}/{assemblyVersion}");
        client.Timeout = TimeSpan.FromSeconds(options.PackageDownloadTimeoutSeconds);

        return client;
    }

    private static NuGetClientFactory NuGetClientFactoryFactory(IServiceProvider provider)
    {
        var httpClient = provider.GetRequiredService<HttpClient>();
        var options = provider.GetRequiredService<IOptions<MirrorOptions>>();

        return new NuGetClientFactory(
            httpClient,
            options.Value.PackageSource.ToString());
    }

    private static IUpstreamClient UpstreamClientFactory(IServiceProvider provider)
    {
        var options = provider.GetRequiredService<IOptionsSnapshot<MirrorOptions>>();

        if (!options.Value.Enabled)
        {
            return provider.GetRequiredService<DisabledUpstreamClient>();
        }
        else
        {
            return options.Value.Legacy ? provider.GetRequiredService<V2UpstreamClient>() : provider.GetRequiredService<V3UpstreamClient>();
        }
    }
}