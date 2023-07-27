using System.Net.Http.Json;
using System.Net;
using CleanMicroserviceSystem.DataStructure;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Oceanus.Client.Abstraction;

public abstract class OceanusServiceClientBase
{
    protected readonly ILogger<OceanusServiceClientBase> logger;
    protected readonly string serviceUriPrefix;
    protected readonly HttpClient httpClient;
    protected readonly Uri baseUriPrefix;

    public OceanusServiceClientBase(
        ILogger<OceanusServiceClientBase> logger,
        IHttpClientFactory httpClientFactory,
        string serviceClientName,
        string serviceUriPrefix)
    {
        this.logger = logger;
        this.serviceUriPrefix = serviceUriPrefix;
        this.httpClient = httpClientFactory.CreateClient(serviceClientName);
        if (!serviceUriPrefix.EndsWith("/")) serviceUriPrefix += "/";
        this.baseUriPrefix = new Uri(this.httpClient.BaseAddress!, serviceUriPrefix);
    }

    protected virtual Uri BuildUri(string uri) => new(this.baseUriPrefix, uri.TrimStart('/'));

    protected virtual async Task<CommonResult?> GetCommonResult(HttpResponseMessage response)
    {
        if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
        {
            var result = await response.Content.ReadFromJsonAsync<WebApiValidateResult>();
            throw new ArgumentException($"{response.StatusCode} : {result?.ToString()}");
        }
        else
        {
            var result = await response.Content.ReadFromJsonAsync<CommonResult>();
            return result;
        }
    }

    protected virtual async Task<CommonResult<TEntity>?> GetCommonResult<TEntity>(HttpResponseMessage response)
    {
        if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
        {
            var result = await response.Content.ReadFromJsonAsync<WebApiValidateResult>();
            throw new ArgumentException($"{response.StatusCode} : {result?.ToString()}");
        }
        else
        {
            try
            {
                var result = await response.Content.ReadFromJsonAsync<CommonResult<TEntity>>();
                return result;
            }
            catch
            {
                var content = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"[{response.StatusCode}] {content}");
            }
        }
    }
}
