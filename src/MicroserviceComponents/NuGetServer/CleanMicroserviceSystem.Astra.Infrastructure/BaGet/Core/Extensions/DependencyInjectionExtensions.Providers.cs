using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Configuration;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Entities;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Extensions;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Search;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core;

public static partial class DependencyInjectionExtensions
{
    private static readonly string DatabaseTypeKey = $"{nameof(BaGetOptions.Database)}:{nameof(DatabaseOptions.Type)}";
    private static readonly string SearchTypeKey = $"{nameof(BaGetOptions.Search)}:{nameof(SearchOptions.Type)}";
    private static readonly string StorageTypeKey = $"{nameof(BaGetOptions.Storage)}:{nameof(StorageOptions.Type)}";

    private static readonly string DatabaseSearchType = "Database";

    public static IServiceCollection AddProvider<TService>(
        this IServiceCollection services,
        Func<IServiceProvider, IConfiguration, TService> func)
        where TService : class
    {
        _ = services.AddSingleton<IProvider<TService>>(new DelegateProvider<TService>(func));

        return services;
    }

    public static bool HasDatabaseType(this IConfiguration config, string value)
    {
        return config[DatabaseTypeKey].Equals(value, StringComparison.OrdinalIgnoreCase);
    }

    public static bool HasSearchType(this IConfiguration config, string value)
    {
        return config[SearchTypeKey].Equals(value, StringComparison.OrdinalIgnoreCase);
    }

    public static bool HasStorageType(this IConfiguration config, string value)
    {
        return config[StorageTypeKey].Equals(value, StringComparison.OrdinalIgnoreCase);
    }

    public static IServiceCollection AddBaGetDbContextProvider<TContext>(
        this IServiceCollection services,
        string databaseType,
        Action<IServiceProvider, DbContextOptionsBuilder> configureContext)
        where TContext : DbContext, IContext
    {
        services.TryAddScoped<IContext>(provider => provider.GetRequiredService<TContext>());
        services.TryAddTransient<IPackageDatabase>(provider => provider.GetRequiredService<PackageDatabase>());

        _ = services.AddDbContext<TContext>(configureContext);

        _ = services.AddProvider<IContext>((provider, config) =>
        {
            return !config.HasDatabaseType(databaseType) ? null : (IContext)provider.GetRequiredService<TContext>();
        });

        _ = services.AddProvider<IPackageDatabase>((provider, config) =>
        {
            return !config.HasDatabaseType(databaseType) ? null : (IPackageDatabase)provider.GetRequiredService<PackageDatabase>();
        });

        _ = services.AddProvider<ISearchIndexer>((provider, config) =>
        {
            if (!config.HasSearchType(DatabaseSearchType)) return null;
            return !config.HasDatabaseType(databaseType) ? null : (ISearchIndexer)provider.GetRequiredService<NullSearchIndexer>();
        });

        _ = services.AddProvider<ISearchService>((provider, config) =>
        {
            if (!config.HasSearchType(DatabaseSearchType)) return null;
            return !config.HasDatabaseType(databaseType) ? null : (ISearchService)provider.GetRequiredService<DatabaseSearchService>();
        });

        return services;
    }

    public static TService GetServiceFromProviders<TService>(IServiceProvider services)
        where TService : class
    {
        var providers = services.GetRequiredService<IEnumerable<IProvider<TService>>>();
        var configuration = services.GetRequiredService<IConfiguration>();

        foreach (var provider in providers)
        {
            var result = provider.GetOrNull(services, configuration);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }
}