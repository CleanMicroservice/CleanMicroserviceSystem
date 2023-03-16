namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Storage;

public class SymbolStorageService : ISymbolStorageService
{
    private const string SymbolsPathPrefix = "symbols";
    private const string PdbContentType = "binary/octet-stream";

    private readonly IStorageService _storage;

    public SymbolStorageService(IStorageService storage)
    {
        this._storage = storage ?? throw new ArgumentNullException(nameof(storage));
    }

    public async Task SavePortablePdbContentAsync(
        string filename,
        string key,
        Stream pdbStream,
        CancellationToken cancellationToken)
    {
        var path = this.GetPathForKey(filename, key);
        var result = await this._storage.PutAsync(path, pdbStream, PdbContentType, cancellationToken);

        if (result == StoragePutResult.Conflict)
            throw new InvalidOperationException($"Could not save PDB {filename} {key} due to conflict");
    }

    public async Task<Stream> GetPortablePdbContentStreamOrNullAsync(string filename, string key)
    {
        var path = this.GetPathForKey(filename, key);

        try
        {
            return await this._storage.GetAsync(path);
        }
        catch
        {
            return null;
        }
    }

    private string GetPathForKey(string filename, string key)
    {
        var tempPath = Path.GetDirectoryName(Path.GetTempPath());
        var expandedPath = Path.GetDirectoryName(Path.Combine(tempPath, filename));

        if (expandedPath != tempPath)
            throw new ArgumentException(nameof(filename));

        if (!key.All(char.IsLetterOrDigit))
            throw new ArgumentException(nameof(key));

        key = key[..32] + "ffffffff";

        return Path.Combine(
            SymbolsPathPrefix,
            filename.ToLowerInvariant(),
            key.ToLowerInvariant());
    }
}