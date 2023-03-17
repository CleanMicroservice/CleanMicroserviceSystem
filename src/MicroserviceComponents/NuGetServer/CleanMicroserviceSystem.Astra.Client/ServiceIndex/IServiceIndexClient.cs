using CleanMicroserviceSystem.Astra.Contract.NuGetPackages;

namespace CleanMicroserviceSystem.Astra.Client.ServiceIndex;

public interface IServiceIndexClient
{
    Task<ServiceIndexResponse> GetAsync(CancellationToken cancellationToken = default);
}