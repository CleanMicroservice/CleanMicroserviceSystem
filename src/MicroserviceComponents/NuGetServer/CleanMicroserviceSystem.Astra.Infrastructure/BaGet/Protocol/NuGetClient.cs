using System.Net;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Extensions;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.PackageContent;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.PackageMetadata;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Search;
using NuGet.Versioning;
using PackageMetadataModel = CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models.PackageMetadata;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol;

public class NuGetClient
{
    private readonly IPackageContentClient _contentClient;
    private readonly IPackageMetadataClient _metadataClient;
    private readonly ISearchClient _searchClient;
    private readonly IAutocompleteClient _autocompleteClient;

    protected NuGetClient()
    {
    }

    public NuGetClient(string serviceIndexUrl)
    {
        var httpClient = new HttpClient(new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
        });

        var clientFactory = new NuGetClientFactory(httpClient, serviceIndexUrl);

        this._contentClient = clientFactory.CreatePackageContentClient();
        this._metadataClient = clientFactory.CreatePackageMetadataClient();
        this._searchClient = clientFactory.CreateSearchClient();
        this._autocompleteClient = clientFactory.CreateAutocompleteClient();
    }

    public NuGetClient(NuGetClientFactory clientFactory)
    {
        if (clientFactory == null) throw new ArgumentNullException(nameof(clientFactory));

        this._contentClient = clientFactory.CreatePackageContentClient();
        this._metadataClient = clientFactory.CreatePackageMetadataClient();
        this._searchClient = clientFactory.CreateSearchClient();
        this._autocompleteClient = clientFactory.CreateAutocompleteClient();
    }

    public virtual async Task<bool> ExistsAsync(
        string packageId,
        CancellationToken cancellationToken = default)
    {
        var versions = await this._contentClient.GetPackageVersionsOrNullAsync(packageId, cancellationToken);

        return versions != null && versions.Versions.Any();
    }

    public virtual async Task<bool> ExistsAsync(
        string packageId,
        NuGetVersion packageVersion,
        CancellationToken cancellationToken = default)
    {
        var versions = await this._contentClient.GetPackageVersionsOrNullAsync(packageId, cancellationToken);

        return versions != null
&& versions
            .ParseVersions()
            .Any(v => v == packageVersion);
    }

    public virtual async Task<Stream> DownloadPackageAsync(string packageId, NuGetVersion packageVersion, CancellationToken cancellationToken = default)
    {
        var stream = await this._contentClient.DownloadPackageOrNullAsync(packageId, packageVersion, cancellationToken);

        return stream ?? throw new PackageNotFoundException(packageId, packageVersion);
    }

    public virtual async Task<Stream> DownloadPackageManifestAsync(
        string packageId,
        NuGetVersion packageVersion,
        CancellationToken cancellationToken = default)
    {
        var stream = await this._contentClient.DownloadPackageManifestOrNullAsync(packageId, packageVersion, cancellationToken);

        return stream ?? throw new PackageNotFoundException(packageId, packageVersion);
    }

    public virtual async Task<IReadOnlyList<NuGetVersion>> ListPackageVersionsAsync(
        string packageId,
        CancellationToken cancellationToken = default)
    {
        var packages = await this.GetPackageMetadataAsync(packageId, cancellationToken);

        return packages
            .Where(p => p.IsListed())
            .Select(p => p.ParseVersion())
            .ToList();
    }

    public virtual async Task<IReadOnlyList<NuGetVersion>> ListPackageVersionsAsync(
        string packageId,
        bool includeUnlisted,
        CancellationToken cancellationToken = default)
    {
        if (!includeUnlisted)
            return await this.ListPackageVersionsAsync(packageId, cancellationToken);

        var response = await this._contentClient.GetPackageVersionsOrNullAsync(packageId, cancellationToken);

        return response == null ? new List<NuGetVersion>() : response.ParseVersions();
    }

    public virtual async Task<IReadOnlyList<PackageMetadataModel>> GetPackageMetadataAsync(
        string packageId,
        CancellationToken cancellationToken = default)
    {
        var result = new List<PackageMetadataModel>();

        var registrationIndex = await this._metadataClient.GetRegistrationIndexOrNullAsync(packageId, cancellationToken);

        if (registrationIndex == null)
            return result;

        foreach (var registrationIndexPage in registrationIndex.Pages)
        {
            var items = registrationIndexPage.ItemsOrNull;
            if (items == null)
            {
                var externalRegistrationPage = await this._metadataClient.GetRegistrationPageAsync(
                    registrationIndexPage.RegistrationPageUrl,
                    cancellationToken);

                if (externalRegistrationPage?.ItemsOrNull == null) continue;

                items = externalRegistrationPage.ItemsOrNull;
            }

            result.AddRange(items.Select(i => i.PackageMetadata));
        }

        return result;
    }

    public virtual async Task<PackageMetadataModel> GetPackageMetadataAsync(
        string packageId,
        NuGetVersion packageVersion,
        CancellationToken cancellationToken = default)
    {
        var registrationIndex = await this._metadataClient.GetRegistrationIndexOrNullAsync(packageId, cancellationToken);

        if (registrationIndex == null)
            throw new PackageNotFoundException(packageId, packageVersion);

        foreach (var registrationIndexPage in registrationIndex.Pages)
        {
            var pageLowerVersion = registrationIndexPage.ParseLower();
            var pageUpperVersion = registrationIndexPage.ParseUpper();

            if (pageLowerVersion > packageVersion) continue;
            if (pageUpperVersion < packageVersion) continue;

            var items = registrationIndexPage.ItemsOrNull;
            if (items == null)
            {
                var externalRegistrationPage = await this._metadataClient.GetRegistrationPageAsync(
                    registrationIndexPage.RegistrationPageUrl,
                    cancellationToken);

                if (externalRegistrationPage?.ItemsOrNull == null) continue;

                items = externalRegistrationPage.ItemsOrNull;
            }

            var result = items.SingleOrDefault(i => i.PackageMetadata.ParseVersion() == packageVersion);
            if (result == null)
                break;

            return result.PackageMetadata;
        }

        throw new PackageNotFoundException(packageId, packageVersion);
    }

    public virtual async Task<IReadOnlyList<SearchResult>> SearchAsync(
        string query = null,
        CancellationToken cancellationToken = default)
    {
        var response = await this._searchClient.SearchAsync(query, cancellationToken: cancellationToken);

        return response.Data;
    }

    public virtual async Task<IReadOnlyList<SearchResult>> SearchAsync(
        string query,
        int skip,
        int take,
        CancellationToken cancellationToken = default)
    {
        var response = await this._searchClient.SearchAsync(
            query,
            skip,
            take,
            includePrerelease: true,
            includeSemVer2: true,
            cancellationToken: cancellationToken);

        return response.Data;
    }

    public virtual async Task<IReadOnlyList<SearchResult>> SearchAsync(
        string query,
        bool includePrerelease,
        CancellationToken cancellationToken = default)
    {
        var response = await this._searchClient.SearchAsync(
            query,
            includePrerelease: includePrerelease,
            cancellationToken: cancellationToken);

        return response.Data;
    }

    public virtual async Task<IReadOnlyList<SearchResult>> SearchAsync(
        string query,
        int skip,
        int take,
        bool includePrerelease,
        CancellationToken cancellationToken = default)
    {
        var response = await this._searchClient.SearchAsync(
            query,
            skip,
            take,
            includePrerelease,
            includeSemVer2: true,
            cancellationToken);

        return response.Data;
    }

    public virtual async Task<IReadOnlyList<string>> AutocompleteAsync(
        string query = null,
        CancellationToken cancellationToken = default)
    {
        var response = await this._autocompleteClient.AutocompleteAsync(query, cancellationToken: cancellationToken);

        return response.Data;
    }

    public virtual async Task<IReadOnlyList<string>> AutocompleteAsync(
        string query,
        int skip,
        int take,
        CancellationToken cancellationToken = default)
    {
        var response = await this._autocompleteClient.AutocompleteAsync(
            query,
            skip,
            take,
            includePrerelease: true,
            includeSemVer2: true,
            cancellationToken);

        return response.Data;
    }

    public virtual async Task<IReadOnlyList<string>> AutocompleteAsync(
        string query,
        bool includePrerelease,
        CancellationToken cancellationToken = default)
    {
        var response = await this._autocompleteClient.AutocompleteAsync(
            query,
            includePrerelease: includePrerelease,
            cancellationToken: cancellationToken);

        return response.Data;
    }

    public virtual async Task<IReadOnlyList<string>> AutocompleteAsync(
        string query,
        int skip,
        int take,
        bool includePrerelease,
        CancellationToken cancellationToken = default)
    {
        var response = await this._autocompleteClient.AutocompleteAsync(
            query,
            skip,
            take,
            includePrerelease,
            includeSemVer2: true,
            cancellationToken);

        return response.Data;
    }
}