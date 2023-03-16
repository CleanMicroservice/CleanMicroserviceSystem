using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Configuration;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Extensions;
using Microsoft.Extensions.Options;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Storage;

public class FileStorageService : IStorageService
{
    private const int DefaultCopyBufferSize = 81920;

    private readonly string _storePath;

    public FileStorageService(IOptionsSnapshot<FileSystemStorageOptions> options)
    {
        if (options == null) throw new ArgumentNullException(nameof(options));

        this._storePath = Path.GetFullPath(options.Value.Path);
        if (!this._storePath.EndsWith(Path.DirectorySeparatorChar.ToString()))
            this._storePath += Path.DirectorySeparatorChar;
    }

    public Task<Stream> GetAsync(string path, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        path = this.GetFullPath(path);
        var content = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);

        return Task.FromResult<Stream>(content);
    }

    public Task<Uri> GetDownloadUriAsync(string path, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var result = new Uri(this.GetFullPath(path));

        return Task.FromResult(result);
    }

    public async Task<StoragePutResult> PutAsync(
        string path,
        Stream content,
        string contentType,
        CancellationToken cancellationToken = default)
    {
        if (content == null) throw new ArgumentNullException(nameof(content));
        if (string.IsNullOrEmpty(contentType)) throw new ArgumentException("Content type is required", nameof(contentType));

        cancellationToken.ThrowIfCancellationRequested();

        path = this.GetFullPath(path);

        _ = Directory.CreateDirectory(Path.GetDirectoryName(path));

        try
        {
            using var fileStream = File.Open(path, FileMode.CreateNew);
            await content.CopyToAsync(fileStream, DefaultCopyBufferSize, cancellationToken);
            return StoragePutResult.Success;
        }
        catch (IOException) when (File.Exists(path))
        {
            using var targetStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            content.Position = 0;
            return content.Matches(targetStream)
                ? StoragePutResult.AlreadyExists
                : StoragePutResult.Conflict;
        }
    }

    public Task DeleteAsync(string path, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            File.Delete(this.GetFullPath(path));
        }
        catch (DirectoryNotFoundException)
        {
        }

        return Task.CompletedTask;
    }

    private string GetFullPath(string path)
    {
        if (string.IsNullOrEmpty(path))
            throw new ArgumentException("Path is required", nameof(path));

        var fullPath = Path.GetFullPath(Path.Combine(this._storePath, path));

        return !fullPath.StartsWith(this._storePath, StringComparison.Ordinal) ||
            fullPath.Length == this._storePath.Length
            ? throw new ArgumentException("Path resolves outside store path", nameof(path))
            : fullPath;
    }
}