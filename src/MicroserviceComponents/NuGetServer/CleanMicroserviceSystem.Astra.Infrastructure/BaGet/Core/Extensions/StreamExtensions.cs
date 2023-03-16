using System.Security.Cryptography;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Extensions;

public static class StreamExtensions
{
    private const int DefaultCopyBufferSize = 81920;

    public static async Task<FileStream> AsTemporaryFileStreamAsync(
        this Stream original,
        CancellationToken cancellationToken = default)
    {
        var result = new FileStream(
            Path.GetTempFileName(),
            FileMode.Create,
            FileAccess.ReadWrite,
            FileShare.None,
            DefaultCopyBufferSize,
            FileOptions.DeleteOnClose);

        try
        {
            await original.CopyToAsync(result, DefaultCopyBufferSize, cancellationToken);
            result.Position = 0;
        }
        catch (Exception)
        {
            result.Dispose();
            throw;
        }

        return result;
    }

    public static bool Matches(this Stream content, Stream target)
    {
        using var sha256 = SHA256.Create();
        var contentHash = sha256.ComputeHash(content);
        var targetHash = sha256.ComputeHash(target);

        return contentHash.SequenceEqual(targetHash);
    }
}