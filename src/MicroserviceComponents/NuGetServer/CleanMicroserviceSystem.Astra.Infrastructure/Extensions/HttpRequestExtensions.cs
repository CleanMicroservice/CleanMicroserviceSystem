using CleanMicroserviceSystem.Astra.Contract;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Extensions;
using Microsoft.AspNetCore.Http;

namespace CleanMicroserviceSystem.Astra.Infrastructure.Extensions;

public static class HttpRequestExtensions
{
    public static async Task<Stream> GetUploadStreamOrNullAsync(this HttpRequest request, CancellationToken cancellationToken)
    {
        Stream rawUploadStream = null;
        try
        {
            rawUploadStream = request.HasFormContentType && request.Form.Files.Count > 0 ? request.Form.Files[0].OpenReadStream() : request.Body;

            // Convert the upload stream into a temporary file stream to
            // minimize memory usage.
            return await rawUploadStream?.AsTemporaryFileStreamAsync(cancellationToken);
        }
        finally
        {
            rawUploadStream?.Dispose();
        }
    }

    public static string GetApiKey(this HttpRequest request)
    {
        return request.Headers[NuGetServerContract.ApiKeyHeader];
    }
}
