using CleanMicroserviceSystem.Astra.Contract.NuGetPackages;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.ServiceIndex;

public interface IServiceIndexService
{
    Task<ServiceIndexResponse> GetAsync(CancellationToken cancellationToken = default);
}