using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.ServiceIndex;

public interface IServiceIndexClient
{
    Task<ServiceIndexResponse> GetAsync(CancellationToken cancellationToken = default);
}