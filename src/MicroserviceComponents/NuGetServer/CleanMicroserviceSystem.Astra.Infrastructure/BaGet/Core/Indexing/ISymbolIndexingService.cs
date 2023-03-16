namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Indexing;

public enum SymbolIndexingResult
{
    InvalidSymbolPackage,
    PackageNotFound,
    Success,
}

public interface ISymbolIndexingService
{
    Task<SymbolIndexingResult> IndexAsync(Stream stream, CancellationToken cancellationToken);
}