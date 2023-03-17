using CleanMicroserviceSystem.Astra.Contract.NuGetPackages;

namespace CleanMicroserviceSystem.Astra.Client.ServiceIndex;

public partial class AstraNuGetClientFactory
{
    private class ServiceIndexClient : IServiceIndexClient
    {
        private readonly AstraNuGetClientFactory _clientFactory;

        public ServiceIndexClient(AstraNuGetClientFactory clientFactory)
        {
            this._clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
        }

        public async Task<ServiceIndexResponse> GetAsync(CancellationToken cancellationToken = default)
        {
            return await this._clientFactory.GetServiceIndexAsync(cancellationToken);
        }
    }
}