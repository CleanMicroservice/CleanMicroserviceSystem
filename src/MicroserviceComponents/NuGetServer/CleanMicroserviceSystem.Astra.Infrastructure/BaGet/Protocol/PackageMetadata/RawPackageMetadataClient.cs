using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Extensions;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.PackageMetadata;

public class RawPackageMetadataClient : IPackageMetadataClient
{
    private readonly HttpClient _httpClient;
    private readonly string _packageMetadataUrl;

    public RawPackageMetadataClient(HttpClient httpClient, string registrationBaseUrl)
    {
        this._httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        this._packageMetadataUrl = registrationBaseUrl ?? throw new ArgumentNullException(nameof(registrationBaseUrl));
    }

    public async Task<RegistrationIndexResponse> GetRegistrationIndexOrNullAsync(
        string packageId,
        CancellationToken cancellationToken = default)
    {
        var url = $"{this._packageMetadataUrl}/{packageId.ToLowerInvariant()}/index.json";

        return await this._httpClient.GetFromJsonOrDefaultAsync<RegistrationIndexResponse>(url, cancellationToken);
    }

    public async Task<RegistrationPageResponse> GetRegistrationPageAsync(
        string pageUrl,
        CancellationToken cancellationToken = default)
    {
        return await this._httpClient.GetFromJsonAsync<RegistrationPageResponse>(pageUrl, cancellationToken);
    }

    public async Task<RegistrationLeafResponse> GetRegistrationLeafAsync(
        string leafUrl,
        CancellationToken cancellationToken = default)
    {
        return await this._httpClient.GetFromJsonAsync<RegistrationLeafResponse>(leafUrl, cancellationToken);
    }
}