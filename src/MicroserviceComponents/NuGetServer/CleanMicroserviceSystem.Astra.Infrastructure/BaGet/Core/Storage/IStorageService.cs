namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Storage;

public interface IStorageService
{
    Task<Stream> GetAsync(string path, CancellationToken cancellationToken = default);

    Task<Uri> GetDownloadUriAsync(string path, CancellationToken cancellationToken = default);

    Task<StoragePutResult> PutAsync(
        string path,
        Stream content,
        string contentType,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(string path, CancellationToken cancellationToken = default);
}

public enum StoragePutResult
{
    Conflict,
    AlreadyExists,
    Success,
}