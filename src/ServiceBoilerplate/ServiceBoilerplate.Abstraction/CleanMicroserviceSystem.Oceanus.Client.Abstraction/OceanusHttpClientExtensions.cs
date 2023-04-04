using System.Net.Http.Json;

namespace CleanMicroserviceSystem.Oceanus.Client.Abstraction;

public static class OceanusHttpClientExtensions
{
    public static async Task<HttpResponseMessage> DeleteAsync(
    this HttpClient client, Uri? requestUri, HttpContent? content)
    {
        ArgumentNullException.ThrowIfNull(client);
        var request = new HttpRequestMessage(HttpMethod.Delete, requestUri)
        {
            Content = content
        };
        return await client.SendAsync(request);
    }

    public static async Task<HttpResponseMessage> DeleteAsync(
        this HttpClient client, Uri? requestUri, HttpContent? content, CancellationToken cancellation)
    {
        ArgumentNullException.ThrowIfNull(client);
        var request = new HttpRequestMessage(HttpMethod.Delete, requestUri)
        {
            Content = content
        };
        return await client.SendAsync(request, cancellation);
    }

    public static async Task<HttpResponseMessage> DeleteAsJsonAsync<TValue>(
        this HttpClient client, Uri? requestUri, TValue value)
        where TValue : class
    {
        ArgumentNullException.ThrowIfNull(client);
        var request = new HttpRequestMessage(HttpMethod.Delete, requestUri)
        {
            Content = JsonContent.Create(value)
        };
        return await client.SendAsync(request);
    }

    public static async Task<HttpResponseMessage> DeleteAsJsonAsync<TValue>(
        this HttpClient client, Uri? requestUri, TValue value, CancellationToken cancellationToken)
        where TValue : class
    {
        ArgumentNullException.ThrowIfNull(client);
        var request = new HttpRequestMessage(HttpMethod.Delete, requestUri)
        {
            Content = JsonContent.Create(value)
        };
        return await client.SendAsync(request, cancellationToken);
    }

    public static async Task<HttpResponseMessage> DeleteAsJsonAsync<TValue>(
        this HttpClient client, Uri? requestUri, TValue value, HttpCompletionOption option, CancellationToken cancellationToken)
        where TValue : class
    {
        ArgumentNullException.ThrowIfNull(client);
        var request = new HttpRequestMessage(HttpMethod.Delete, requestUri)
        {
            Content = JsonContent.Create(value)
        };
        return await client.SendAsync(request, option, cancellationToken);
    }
}
