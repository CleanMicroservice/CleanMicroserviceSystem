namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Upstream;

public interface IPackageDownloadsSource
{
    Task<Dictionary<string, Dictionary<string, long>>> GetPackageDownloadsAsync();
}