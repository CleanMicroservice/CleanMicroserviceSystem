using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.ServiceIndex;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol;

public partial class NuGetClientFactory
{
    private class ServiceIndexClient : IServiceIndexClient
    {
        private readonly NuGetClientFactory _clientFactory;

        public ServiceIndexClient(NuGetClientFactory clientFactory)
        {
            this._clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
        }

        public async Task<ServiceIndexResponse> GetAsync(CancellationToken cancellationToken = default)
        {
            return await this._clientFactory.GetServiceIndexAsync(cancellationToken);
        }
    }
}
