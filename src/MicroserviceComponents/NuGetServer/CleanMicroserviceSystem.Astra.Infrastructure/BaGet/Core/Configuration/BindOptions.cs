using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Configuration;

public class BindOptions<TOptions> : IConfigureOptions<TOptions> where TOptions : class
{
    private readonly IConfiguration _config;

    public BindOptions(IConfiguration config)
    {
        this._config = config ?? throw new ArgumentNullException(nameof(config));
    }

    public void Configure(TOptions options)
    {
        this._config.Bind(options);
    }
}