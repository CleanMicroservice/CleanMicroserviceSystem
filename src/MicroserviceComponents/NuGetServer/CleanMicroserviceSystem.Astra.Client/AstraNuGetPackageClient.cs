using System.Net.Http.Json;
using CleanMicroserviceSystem.Astra.Contract.NuGetPackages;
using CleanMicroserviceSystem.Oceanus.Client.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CleanMicroserviceSystem.Astra.Contract;

namespace CleanMicroserviceSystem.Astra.Client;

public class AstraNuGetPackageClient : OceanusServiceClientBase
{
    public ILogger<AstraNuGetPackageClient> Logger { get; }

    public AstraNuGetPackageClient(
        ILogger<AstraNuGetPackageClient> logger,
        IHttpClientFactory httpApiResourceFactory,
        IOptionsSnapshot<AstraClientConfiguration> options) :
        base(logger, httpApiResourceFactory, options.Value.GatewayClientName, AstraClientContract.AstraUriPrefix)
    {
        this.Logger = logger;
        this.httpClient.DefaultRequestHeaders.Add(NuGetServerContract.ApiKeyHeader, options.Value.ApiKey);
    }

    public async Task<SearchResponse?> SearchAsync(
        string? q = null,
        int? skip = null,
        int? take = null,
        bool? prerelease = true,
        bool? semVerLevel = true,
        CancellationToken cancellationToken = default)
    {
        var queryString = new QueryString();
        if (!string.IsNullOrEmpty(q)) queryString += QueryString.Create(nameof(q), q);
        if (skip.HasValue && skip.Value > 0) queryString += QueryString.Create(nameof(skip), skip.ToString());
        if (take.HasValue) queryString += QueryString.Create(nameof(take), take.ToString());
        if (prerelease.HasValue) queryString += QueryString.Create(nameof(prerelease), prerelease.ToString());
        if (semVerLevel.HasValue) queryString += QueryString.Create(nameof(semVerLevel), NuGetServerContract.SemVerLevel);

        var uri = this.BuildUri($"/v3/search{queryString.ToUriComponent()}");
        var searchResponse = await this.httpClient.GetFromJsonAsync<SearchResponse>(uri, cancellationToken);
        return searchResponse;
    }

    public async Task<HttpResponseMessage> PublishAsync(
        string apiKey,
        Stream stream,
        CancellationToken cancellationToken = default)
    {
        var uri = this.BuildUri($"/api/v2/package");
        var content = new StreamContent(stream);
        content.Headers.Add(NuGetServerContract.ApiKeyHeader, apiKey);
        var responseMessage = await this.httpClient.PutAsync(uri, content, cancellationToken);
        return responseMessage;
    }

    public async Task<HttpResponseMessage> DeleteAsync(
        string apiKey,
        string packageId,
        string packageVersion,
        CancellationToken cancellationToken = default)
    {
        var uri = this.BuildUri($"/api/v2/package/{packageId}/{packageVersion}");
        var content = new StringContent(string.Empty);
        content.Headers.Add(NuGetServerContract.ApiKeyHeader, apiKey);
        var responseMessage = await this.httpClient.DeleteAsync(uri, content, cancellationToken);
        return responseMessage;
    }

    public async Task<HttpResponseMessage> RelistAsync(
        string apiKey,
        string packageId,
        string packageVersion,
        CancellationToken cancellationToken = default)
    {
        var uri = this.BuildUri($"/api/v2/package/{packageId}/{packageVersion}");
        var content = new StringContent(string.Empty);
        content.Headers.Add(NuGetServerContract.ApiKeyHeader, apiKey);
        var responseMessage = await this.httpClient.PostAsync(uri, content, cancellationToken);
        return responseMessage;
    }
}
