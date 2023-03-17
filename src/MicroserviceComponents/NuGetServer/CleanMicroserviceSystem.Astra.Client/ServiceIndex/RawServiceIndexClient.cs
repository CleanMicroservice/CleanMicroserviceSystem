using CleanMicroserviceSystem.Astra.Client.Extensions;
using CleanMicroserviceSystem.Astra.Contract.NuGetPackages;

namespace CleanMicroserviceSystem.Astra.Client.ServiceIndex;

public class RawServiceIndexClient : IServiceIndexClient
{
    private readonly HttpClient _httpClient;
    private readonly string _serviceIndexUrl;

    public RawServiceIndexClient(HttpClient httpClient, string serviceIndexUrl)
    {
        this._httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        this._serviceIndexUrl = serviceIndexUrl ?? throw new ArgumentNullException(nameof(serviceIndexUrl));
    }

    public async Task<ServiceIndexResponse> GetAsync(CancellationToken cancellationToken = default)
    {
        return await this._httpClient.GetFromJsonAsync<ServiceIndexResponse>(
            this._serviceIndexUrl,
            cancellationToken);
    }
}