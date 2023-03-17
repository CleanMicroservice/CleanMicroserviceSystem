using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Astra.Client.Catalog;

public class FileCursor : ICursor
{
    private readonly string _path;
    private readonly ILogger<FileCursor> _logger;

    public FileCursor(string path, ILogger<FileCursor> logger)
    {
        this._path = path ?? throw new ArgumentNullException(nameof(path));
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<DateTimeOffset?> GetAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var file = File.OpenRead(this._path);
            var data = await JsonSerializer.DeserializeAsync<Data>(file, options: null, cancellationToken);
            this._logger.LogDebug("Read cursor value {cursor:O} from {path}.", data.Value, this._path);
            return data.Value;
        }
        catch (Exception e) when (e is FileNotFoundException or JsonException)
        {
            return null;
        }
    }

    public Task SetAsync(DateTimeOffset value, CancellationToken cancellationToken)
    {
        var data = new Data { Value = value };
        var jsonString = JsonSerializer.Serialize(data);
        File.WriteAllText(this._path, jsonString);
        this._logger.LogDebug("Wrote cursor value {cursor:O} to {path}.", data.Value, this._path);
        return Task.CompletedTask;
    }

    private class Data
    {
        [JsonPropertyName("value")]
        public DateTimeOffset Value { get; set; }
    }
}