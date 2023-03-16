using Microsoft.Extensions.Configuration;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Extensions;

public interface IProvider<TService>
{
    TService GetOrNull(IServiceProvider provider, IConfiguration configuration);
}

internal class DelegateProvider<TService> : IProvider<TService>
{
    private readonly Func<IServiceProvider, IConfiguration, TService> _func;

    public DelegateProvider(Func<IServiceProvider, IConfiguration, TService> func)
    {
        this._func = func ?? throw new ArgumentNullException(nameof(func));
    }

    public TService GetOrNull(IServiceProvider provider, IConfiguration configuration)
    {
        return this._func(provider, configuration);
    }
}