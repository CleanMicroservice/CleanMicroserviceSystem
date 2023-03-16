using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Configuration;
using Microsoft.Extensions.Options;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Authentication;

public class ApiKeyAuthenticationService : IAuthenticationService
{
    private readonly string _apiKey;

    public ApiKeyAuthenticationService(IOptionsSnapshot<BaGetOptions> options)
    {
        if (options == null) throw new ArgumentNullException(nameof(options));

        this._apiKey = string.IsNullOrEmpty(options.Value.ApiKey) ? null : options.Value.ApiKey;
    }

    public Task<bool> AuthenticateAsync(string apiKey, CancellationToken cancellationToken)
    {
        return Task.FromResult(this.Authenticate(apiKey));
    }

    private bool Authenticate(string apiKey)
    {
        return this._apiKey == null || this._apiKey == apiKey;
    }
}