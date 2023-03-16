using System;
using System.Threading;
using System.Threading.Tasks;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.PackageMetadata;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol
{
    public partial class NuGetClientFactory
    {
        private class PackageMetadataClient : IPackageMetadataClient
        {
            private readonly NuGetClientFactory _clientfactory;

            public PackageMetadataClient(NuGetClientFactory clientFactory)
            {
                _clientfactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
            }

            public async Task<RegistrationIndexResponse> GetRegistrationIndexOrNullAsync(
                string packageId,
                CancellationToken cancellationToken = default)
            {
                var client = await _clientfactory.GetPackageMetadataClientAsync(cancellationToken);

                return await client.GetRegistrationIndexOrNullAsync(packageId, cancellationToken);
            }

            public async Task<RegistrationPageResponse> GetRegistrationPageAsync(
                string pageUrl,
                CancellationToken cancellationToken = default)
            {
                var client = await _clientfactory.GetPackageMetadataClientAsync(cancellationToken);

                return await client.GetRegistrationPageAsync(pageUrl, cancellationToken);
            }

            public async Task<RegistrationLeafResponse> GetRegistrationLeafAsync(
                string leafUrl,
                CancellationToken cancellationToken = default)
            {
                var client = await _clientfactory.GetPackageMetadataClientAsync(cancellationToken);

                return await client.GetRegistrationLeafAsync(leafUrl, cancellationToken);
            }
        }
    }
}
