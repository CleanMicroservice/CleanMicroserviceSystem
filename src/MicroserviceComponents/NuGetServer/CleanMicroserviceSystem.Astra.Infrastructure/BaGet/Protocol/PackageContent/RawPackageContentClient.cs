using System.Net;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Extensions;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;
using NuGet.Versioning;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.PackageContent;

public class RawPackageContentClient : IPackageContentClient
{
    private readonly HttpClient _httpClient;
    private readonly string _packageContentUrl;

    public RawPackageContentClient(HttpClient httpClient, string packageContentUrl)
    {
        this._httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        this._packageContentUrl = packageContentUrl?.TrimEnd('/')
            ?? throw new ArgumentNullException(nameof(packageContentUrl));
    }

    public async Task<PackageVersionsResponse> GetPackageVersionsOrNullAsync(
        string packageId,
        CancellationToken cancellationToken = default)
    {
        var id = packageId.ToLowerInvariant();
        var url = $"{this._packageContentUrl}/{id}/index.json";

        return await this._httpClient.GetFromJsonOrDefaultAsync<PackageVersionsResponse>(url, cancellationToken);
    }

    public async Task<Stream> DownloadPackageOrNullAsync(
        string packageId,
        NuGetVersion packageVersion,
        CancellationToken cancellationToken = default)
    {
        var id = packageId.ToLowerInvariant();
        var version = packageVersion.ToNormalizedString().ToLowerInvariant();

        var url = $"{this._packageContentUrl}/{id}/{version}/{id}.{version}.nupkg";
        var response = await this._httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        return response.StatusCode == HttpStatusCode.NotFound ? null : await response.Content.ReadAsStreamAsync();
    }

    public async Task<Stream> DownloadPackageManifestOrNullAsync(
        string packageId,
        NuGetVersion packageVersion,
        CancellationToken cancellationToken = default)
    {
        var id = packageId.ToLowerInvariant();
        var version = packageVersion.ToNormalizedString().ToLowerInvariant();

        var url = $"{this._packageContentUrl}/{id}/{version}/{id}.nuspec";
        var response = await this._httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        return response.StatusCode == HttpStatusCode.NotFound ? null : await response.Content.ReadAsStreamAsync();
    }
}