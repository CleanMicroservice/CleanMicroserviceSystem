using System.Net;
using System.Text.Json;

namespace CleanMicroserviceSystem.Astra.Client.Extensions;

internal static class HttpClientExtensions
{
    public static async Task<TResult> GetFromJsonAsync<TResult>(
        this HttpClient httpClient,
        string requestUri,
        CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.GetAsync(
            requestUri,
            HttpCompletionOption.ResponseHeadersRead,
            cancellationToken);
        _ = response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync();
        return await JsonSerializer.DeserializeAsync<TResult>(stream, cancellationToken: cancellationToken);
    }

    public static async Task<TResult> GetFromJsonOrDefaultAsync<TResult>(
        this HttpClient httpClient,
        string requestUri,
        CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.GetAsync(
            requestUri,
            HttpCompletionOption.ResponseHeadersRead,
            cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
            return default;

        _ = response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync();
        return await JsonSerializer.DeserializeAsync<TResult>(stream, cancellationToken: cancellationToken);
    }
}