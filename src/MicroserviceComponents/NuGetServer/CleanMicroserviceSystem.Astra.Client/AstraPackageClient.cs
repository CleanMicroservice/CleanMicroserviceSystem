using System.Net.Http.Json;
using CleanMicroserviceSystem.DataStructure;
using CleanMicroserviceSystem.Oceanus.Client.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CleanMicroserviceSystem.Astra.Client;

public class AstraPackageClient : OceanusServiceClientBase
{
    public ILogger<AstraPackageClient> Logger { get; }

    public AstraPackageClient(
        ILogger<AstraPackageClient> logger,
        IHttpClientFactory httpApiResourceFactory,
        IOptionsSnapshot<AstraClientConfiguration> options) :
        base(logger, httpApiResourceFactory, options.Value.GatewayClientName, AstraClientContract.AstraUriPrefix)
    {
        this.Logger = logger;
    }

    /*
    public async Task<PackageInformationResponse?> CreateApiResourceAsync(PackageCreateRequest request)
    {
        var uri = this.BuildUri("/api/Package");
        var response = await this.httpClient.PostAsJsonAsync(uri, request);
        var apiResource = await response.Content.ReadFromJsonAsync<PackageInformationResponse>();
        return apiResource;
    }

    public async Task<PackageInformationResponse?> GetApiResourceAsync(int id)
    {
        var uri = this.BuildUri($"/api/Package/{id}");
        var apiResource = await this.httpClient.GetFromJsonAsync<PackageInformationResponse>(uri);
        return apiResource;
    }

    public async Task<PaginatedEnumerable<PackageInformationResponse>?> SearchApiResourcesAsync(PackageSearchRequest request)
    {
        var queryString = new QueryString();
        if (!string.IsNullOrEmpty(request.KeyWord)) queryString += QueryString.Create(nameof(request.KeyWord), request.KeyWord.ToString());
        var uri = this.BuildUri($"/api/Package/Search{queryString.ToUriComponent()}");
        var apiResource = await this.httpClient.GetFromJsonAsync<PaginatedEnumerable<PackageInformationResponse>>(uri);
        return apiResource;
    }

    public async Task<HttpResponseMessage?> UpdateApiResourceAsync(int id, PackageUpdateRequest request)
    {
        var uri = this.BuildUri($"/api/Package/{id}");
        var response = await this.httpClient.PutAsJsonAsync(uri, request);
        return response;
    }

    public async Task<HttpResponseMessage?> DeleteApiResourceAsync(int id)
    {
        var uri = this.BuildUri($"/api/Package/{id}");
        var response = await this.httpClient.DeleteAsync(uri);
        return response;
    }
    */
}
