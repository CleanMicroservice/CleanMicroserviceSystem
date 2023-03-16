using System.Text;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Extensions;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Search;

public class RawSearchClient : ISearchClient
{
    private readonly HttpClient _httpClient;
    private readonly string _searchUrl;

    public RawSearchClient(HttpClient httpClient, string searchUrl)
    {
        this._httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        this._searchUrl = searchUrl ?? throw new ArgumentNullException(nameof(searchUrl));
    }

    public async Task<SearchResponse> SearchAsync(
        string query = null,
        int skip = 0,
        int take = 20,
        bool includePrerelease = true,
        bool includeSemVer2 = true,
        CancellationToken cancellationToken = default)
    {
        var url = AddSearchQueryString(this._searchUrl, query, skip, take, includePrerelease, includeSemVer2, "q");

        return await this._httpClient.GetFromJsonAsync<SearchResponse>(url, cancellationToken);
    }

    internal static string AddSearchQueryString(
        string uri,
        string query,
        int? skip,
        int? take,
        bool includePrerelease,
        bool includeSemVer2,
        string queryParamName)
    {
        var queryString = new Dictionary<string, string>();

        if (skip.HasValue && skip.Value > 0) queryString["skip"] = skip.ToString();
        if (take.HasValue) queryString["take"] = take.ToString();
        if (includePrerelease) queryString["prerelease"] = true.ToString();
        if (includeSemVer2) queryString["semVerLevel"] = "2.0.0";

        if (!string.IsNullOrEmpty(query))
            queryString[queryParamName] = query;

        return AddQueryString(uri, queryString);
    }

    private static string AddQueryString(string uri, Dictionary<string, string> queryString)
    {
        if (uri.IndexOf('#') != -1) throw new InvalidOperationException("URL anchors are not supported");
        if (uri.IndexOf('?') != -1) throw new InvalidOperationException("Adding query strings to URL with query strings is not supported");

        var builder = new StringBuilder(uri);
        var hasQuery = false;

        foreach (var parameter in queryString)
        {
            _ = builder.Append(hasQuery ? '&' : '?');
            _ = builder.Append(Uri.EscapeDataString(parameter.Key));
            _ = builder.Append('=');
            _ = builder.Append(Uri.EscapeDataString(parameter.Value));
            hasQuery = true;
        }

        return builder.ToString();
    }
}