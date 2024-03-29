﻿using System.Net.Http.Json;
using CleanMicroserviceSystem.DataStructure;
using CleanMicroserviceSystem.Oceanus.Client.Abstraction;
using CleanMicroserviceSystem.Themis.Contract.ApiResources;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CleanMicroserviceSystem.Themis.Client;

public class ThemisApiResourceClient : OceanusServiceClientBase
{
    public ILogger<ThemisApiResourceClient> Logger { get; }

    public ThemisApiResourceClient(
        ILogger<ThemisApiResourceClient> logger,
        IHttpClientFactory httpApiResourceFactory,
        IOptionsSnapshot<ThemisClientConfiguration> options) :
        base(logger, httpApiResourceFactory, options.Value.GatewayClientName, ThemisClientContract.ThemisUriPrefix)
    {
        this.Logger = logger;
    }

    public async Task<CommonResult<ApiResourceInformationResponse>?> CreateApiResourceAsync(ApiResourceCreateRequest request)
    {
        var uri = this.BuildUri("/api/ApiResource");
        var response = await this.httpClient.PostAsJsonAsync(uri, request);
        return await this.GetCommonResult<ApiResourceInformationResponse>(response);
    }

    public async Task<ApiResourceInformationResponse?> GetApiResourceAsync(int id)
    {
        var uri = this.BuildUri($"/api/ApiResource/{id}");
        var apiResource = await this.httpClient.GetFromJsonAsync<ApiResourceInformationResponse>(uri);
        return apiResource;
    }

    public async Task<PaginatedEnumerable<ApiResourceInformationResponse>?> SearchApiResourcesAsync(ApiResourceSearchRequest request)
    {
        var queryString = new QueryString();
        if (request.Id.HasValue) queryString += QueryString.Create(nameof(request.Id), request.Id.ToString());
        if (request.Count.HasValue) queryString += QueryString.Create(nameof(request.Count), request.Count.ToString());
        if (request.Start.HasValue) queryString += QueryString.Create(nameof(request.Start), request.Start.ToString());
        if (request.Enabled.HasValue) queryString += QueryString.Create(nameof(request.Enabled), request.Enabled.ToString());
        if (!string.IsNullOrEmpty(request.Name)) queryString += QueryString.Create(nameof(request.Name), request.Name.ToString());
        var uri = this.BuildUri($"/api/ApiResource/Search{queryString.ToUriComponent()}");
        var apiResource = await this.httpClient.GetFromJsonAsync<PaginatedEnumerable<ApiResourceInformationResponse>>(uri);
        return apiResource;
    }

    public async Task<CommonResult?> UpdateApiResourceAsync(int id, ApiResourceUpdateRequest request)
    {
        var uri = this.BuildUri($"/api/ApiResource/{id}");
        var response = await this.httpClient.PutAsJsonAsync(uri, request);
        return await this.GetCommonResult(response);
    }

    public async Task<CommonResult?> DeleteApiResourceAsync(int id)
    {
        var uri = this.BuildUri($"/api/ApiResource/{id}");
        var response = await this.httpClient.DeleteAsync(uri);
        return await this.GetCommonResult(response);
    }
}
