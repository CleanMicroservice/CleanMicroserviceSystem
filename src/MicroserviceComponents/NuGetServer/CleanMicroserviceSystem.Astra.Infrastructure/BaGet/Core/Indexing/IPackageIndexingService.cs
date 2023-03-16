namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Indexing;

public enum PackageIndexingResult
{
    InvalidPackage,
    PackageAlreadyExists,
    Success,
}

public interface IPackageIndexingService
{
    Task<PackageIndexingResult> IndexAsync(Stream stream, CancellationToken cancellationToken);
}