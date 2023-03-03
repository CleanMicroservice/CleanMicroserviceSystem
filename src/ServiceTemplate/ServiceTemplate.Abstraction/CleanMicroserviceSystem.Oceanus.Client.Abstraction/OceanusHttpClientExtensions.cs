using System.Net.Http.Json;

namespace CleanMicroserviceSystem.Oceanus.Client.Abstraction;

public static class OceanusHttpClientExtensions
{
    public static Task<HttpResponseMessage> DeleteAsJsonAsync<TValue>(
        this HttpClient client, Uri? requestUri, TValue value)
        where TValue : class
    {
        ArgumentNullException.ThrowIfNull(client);
        var request = new HttpRequestMessage(HttpMethod.Delete, requestUri)
        {
            Content = JsonContent.Create(value)
        };
        return client.SendAsync(request);
    }

    public static Task<HttpResponseMessage> DeleteAsJsonAsync<TValue>(
        this HttpClient client, Uri? requestUri, TValue value, CancellationToken cancellationToken)
        where TValue : class
    {
        ArgumentNullException.ThrowIfNull(client);
        var request = new HttpRequestMessage(HttpMethod.Delete, requestUri)
        {
            Content = JsonContent.Create(value)
        };
        return client.SendAsync(request, cancellationToken);
    }

    public static Task<HttpResponseMessage> DeleteAsJsonAsync<TValue>(
        this HttpClient client, Uri? requestUri, TValue value, HttpCompletionOption option, CancellationToken cancellationToken)
        where TValue : class
    {
        ArgumentNullException.ThrowIfNull(client);
        var request = new HttpRequestMessage(HttpMethod.Delete, requestUri)
        {
            Content = JsonContent.Create(value)
        };
        return client.SendAsync(request, option, cancellationToken);
    }
}
