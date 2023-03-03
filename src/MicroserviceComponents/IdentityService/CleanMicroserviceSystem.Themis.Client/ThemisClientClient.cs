using System.Net.Http.Json;
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

    public async Task<ClientInformationResponse?> CreateClientAsync(ClientCreateRequest request)
    {
        var uri = this.BuildUri("/api/Client");
        var response = await this.httpClient.PostAsJsonAsync(uri, request);
        var client = await response.Content.ReadFromJsonAsync<ClientInformationResponse>();
        return client;
    }

    public async Task<ClientInformationResponse?> GetClientAsync(int id)
    {
        var uri = this.BuildUri($"/api/Client/{id}");
        var client = await this.httpClient.GetFromJsonAsync<ClientInformationResponse>(uri);
        return client;
    }

    public async Task<ClientInformationResponse?> SearchClientsAsync(ClientSearchRequest request)
    {
        var queryString = new QueryString();
        if (request.Id.HasValue) _ = queryString.Add(nameof(request.Id), request.Id.ToString());
        if (request.Count.HasValue) _ = queryString.Add(nameof(request.Count), request.Count.ToString());
        if (request.Start.HasValue) _ = queryString.Add(nameof(request.Start), request.Start.ToString());
        if (request.Enabled.HasValue) _ = queryString.Add(nameof(request.Enabled), request.Enabled.ToString());
        if (!string.IsNullOrEmpty(request.Name)) _ = queryString.Add(nameof(request.Name), request.Name.ToString());
        var uri = this.BuildUri($"/api/Client/Search?{queryString.ToUriComponent()}");
        var client = await this.httpClient.GetFromJsonAsync<ClientInformationResponse>(uri);
        return client;
    }

    public async Task<HttpResponseMessage?> UpdateClientAsync(int id, ClientUpdateRequest request)
    {
        var uri = this.BuildUri($"/api/Client/{id}");
        var response = await this.httpClient.PutAsJsonAsync(uri, request);
        return response;
    }

    public async Task<HttpResponseMessage?> DeleteClientAsync(int id)
    {
        var uri = this.BuildUri($"/api/Client/{id}");
        var response = await this.httpClient.DeleteAsync(uri);
        return response;
    }

    public async Task<IEnumerable<ClaimInformationResponse>?> GetClientClaimsAsync(int id)
    {
        var uri = this.BuildUri($"/api/Client/{id}/Claims");
        var claims = await this.httpClient.GetFromJsonAsync<IEnumerable<ClaimInformationResponse>>(uri);
        return claims;
    }

    public async Task<HttpResponseMessage> AddClientClaimsAsync(int id, IEnumerable<ClaimsUpdateRequest> requests)
    {
        var uri = this.BuildUri($"/api/Client/{id}/Claims");
        var response = await this.httpClient.PostAsJsonAsync(uri, requests);
        return response;
    }

    public async Task<HttpResponseMessage> UpdateClientClaimsAsync(int id, IEnumerable<ClaimsUpdateRequest> requests)
    {
        var uri = this.BuildUri($"/api/Client/{id}/Claims");
        var response = await this.httpClient.PutAsJsonAsync(uri, requests);
        return response;
    }

    public async Task<HttpResponseMessage?> DeleteClientClaimsAsync(int id, IEnumerable<ClaimsUpdateRequest> requests)
    {
        var uri = this.BuildUri($"/api/Client/{id}/Claims");
        var response = await this.httpClient.DeleteAsJsonAsync(uri, requests);
        return response;
    }
}
