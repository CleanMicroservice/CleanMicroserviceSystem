using System.Net.Http.Json;
using CleanMicroserviceSystem.DataStructure;
using CleanMicroserviceSystem.Oceanus.Client.Abstraction;
using CleanMicroserviceSystem.Themis.Contract.Claims;
using CleanMicroserviceSystem.Themis.Contract.Clients;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CleanMicroserviceSystem.Themis.Client;

public class ThemisClientClient : OceanusServiceClientBase
{
    public ThemisClientClient(
        ILogger<ThemisClientClient> logger,
        IHttpClientFactory httpClientFactory,
        IOptionsSnapshot<ThemisClientConfiguration> options) :
        base(logger, httpClientFactory, options.Value.GatewayClientName, ThemisClientContract.ThemisUriPrefix)
    {
    }

    public async Task<CommonResult<ClientInformationResponse>?> CreateClientAsync(ClientCreateRequest request)
    {
        var uri = this.BuildUri("/api/Client");
        var response = await this.httpClient.PostAsJsonAsync(uri, request);
        return await this.GetCommonResult<ClientInformationResponse>(response);
    }

    public async Task<ClientInformationResponse?> GetClientAsync(int id)
    {
        var uri = this.BuildUri($"/api/Client/{id}");
        var client = await this.httpClient.GetFromJsonAsync<ClientInformationResponse>(uri);
        return client;
    }

    public async Task<PaginatedEnumerable<ClientInformationResponse>?> SearchClientsAsync(ClientSearchRequest request)
    {
        var queryString = new QueryString();
        if (request.Id.HasValue) queryString += QueryString.Create(nameof(request.Id), request.Id.ToString());
        if (request.Count.HasValue) queryString += QueryString.Create(nameof(request.Count), request.Count.ToString());
        if (request.Start.HasValue) queryString += QueryString.Create(nameof(request.Start), request.Start.ToString());
        if (request.Enabled.HasValue) queryString += QueryString.Create(nameof(request.Enabled), request.Enabled.ToString());
        if (!string.IsNullOrEmpty(request.Name)) queryString += QueryString.Create(nameof(request.Name), request.Name.ToString());
        var uri = this.BuildUri($"/api/Client/Search{queryString.ToUriComponent()}");
        var client = await this.httpClient.GetFromJsonAsync<PaginatedEnumerable<ClientInformationResponse>>(uri);
        return client;
    }

    public async Task<CommonResult?> UpdateClientAsync(int id, ClientUpdateRequest request)
    {
        var uri = this.BuildUri($"/api/Client/{id}");
        var response = await this.httpClient.PutAsJsonAsync(uri, request);
        return await this.GetCommonResult(response);
    }

    public async Task<CommonResult?> DeleteClientAsync(int id)
    {
        var uri = this.BuildUri($"/api/Client/{id}");
        var response = await this.httpClient.DeleteAsync(uri);
        return await this.GetCommonResult(response);
    }

    public async Task<IEnumerable<ClaimInformationResponse>?> GetClientClaimsAsync(int id)
    {
        var uri = this.BuildUri($"/api/Client/{id}/Claims");
        var claims = await this.httpClient.GetFromJsonAsync<IEnumerable<ClaimInformationResponse>>(uri);
        return claims;
    }

    public async Task<CommonResult?> AddClientClaimsAsync(int id, IEnumerable<ClaimsUpdateRequest> requests)
    {
        var uri = this.BuildUri($"/api/Client/{id}/Claims");
        var response = await this.httpClient.PostAsJsonAsync(uri, requests);
        return await this.GetCommonResult(response);
    }

    public async Task<CommonResult?> UpdateClientClaimsAsync(int id, IEnumerable<ClaimsUpdateRequest> requests)
    {
        var uri = this.BuildUri($"/api/Client/{id}/Claims");
        var response = await this.httpClient.PutAsJsonAsync(uri, requests);
        return await this.GetCommonResult(response);
    }

    public async Task<CommonResult?> DeleteClientClaimsAsync(int id, IEnumerable<ClaimsUpdateRequest> requests)
    {
        var uri = this.BuildUri($"/api/Client/{id}/Claims");
        var response = await this.httpClient.DeleteAsJsonAsync(uri, requests);
        return await this.GetCommonResult(response);
    }
}
