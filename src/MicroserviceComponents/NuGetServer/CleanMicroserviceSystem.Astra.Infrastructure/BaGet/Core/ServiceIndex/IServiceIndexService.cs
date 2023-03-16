using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.ServiceIndex;

public interface IServiceIndexService
{
    Task<ServiceIndexResponse> GetAsync(CancellationToken cancellationToken = default);
}