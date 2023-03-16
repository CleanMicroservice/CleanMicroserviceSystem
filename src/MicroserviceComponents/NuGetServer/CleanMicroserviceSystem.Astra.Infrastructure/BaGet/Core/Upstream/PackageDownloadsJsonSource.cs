using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Versioning;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Upstream;

public class PackageDownloadsJsonSource : IPackageDownloadsSource
{
    public const string PackageDownloadsV1Url = "https://nugetprod0.blob.core.windows.net/ng-search-data/downloads.v1.json";
    private readonly HttpClient _httpClient;
    private readonly ILogger<PackageDownloadsJsonSource> _logger;

    public PackageDownloadsJsonSource(HttpClient httpClient, ILogger<PackageDownloadsJsonSource> logger)
    {
        this._httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Dictionary<string, Dictionary<string, long>>> GetPackageDownloadsAsync()
    {
        this._logger.LogInformation("Fetching package downloads...");

        var results = new Dictionary<string, Dictionary<string, long>>();

        using (var downloadsStream = await this.GetDownloadsStreamAsync())
        using (var downloadStreamReader = new StreamReader(downloadsStream))
        using (var jsonReader = new JsonTextReader(downloadStreamReader))
        {
            this._logger.LogInformation("Parsing package downloads...");

            _ = jsonReader.Read();

            while (jsonReader.Read())
            {
                try
                {
                    if (jsonReader.TokenType == JsonToken.StartArray)
                    {
                        var record = JToken.ReadFrom(jsonReader);
                        var id = string.Intern(record[0].ToString().ToLowerInvariant());

                        if (record.Count() == 2 && record[1].Type != JTokenType.Array)
                            continue;

                        if (!results.ContainsKey(id))
                            results.Add(id, new Dictionary<string, long>());

                        foreach (var token in record)
                        {
                            if (token != null && token.Count() == 2)
                            {
                                var version = string.Intern(NuGetVersion.Parse(token[0].ToString()).ToNormalizedString().ToLowerInvariant());
                                var downloads = token[1].ToObject<int>();

                                results[id][version] = downloads;
                            }
                        }
                    }
                }
                catch (JsonReaderException e)
                {
                    this._logger.LogError(e, "Invalid entry in downloads.v1.json");
                }
            }

            this._logger.LogInformation("Parsed package downloads");
        }

        return results;
    }

    private async Task<Stream> GetDownloadsStreamAsync()
    {
        this._logger.LogInformation("Downloading downloads.v1.json...");

        var fileStream = File.Open(Path.GetTempFileName(), FileMode.Create);
        var response = await this._httpClient.GetAsync(PackageDownloadsV1Url, HttpCompletionOption.ResponseHeadersRead);

        _ = response.EnsureSuccessStatusCode();

        using (var networkStream = await response.Content.ReadAsStreamAsync())
        {
            await networkStream.CopyToAsync(fileStream);
        }

        _ = fileStream.Seek(0, SeekOrigin.Begin);

        this._logger.LogInformation("Downloaded downloads.v1.json");

        return fileStream;
    }
}