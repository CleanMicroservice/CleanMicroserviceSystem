namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Storage;

public interface ISymbolStorageService
{
    Task SavePortablePdbContentAsync(
        string file,
        string key,
        Stream pdbStream,
        CancellationToken cancellationToken);
    
    Task<Stream> GetPortablePdbContentStreamOrNullAsync(string file, string key);
}