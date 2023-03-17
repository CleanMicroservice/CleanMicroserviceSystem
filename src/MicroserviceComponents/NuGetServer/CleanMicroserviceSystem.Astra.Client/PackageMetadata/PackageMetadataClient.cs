using CleanMicroserviceSystem.Astra.Contract.NuGetPackages;

namespace CleanMicroserviceSystem.Astra.Client.PackageMetadata;

public partial class AstraNuGetClientFactory
{
    private class PackageMetadataClient : IPackageMetadataClient
    {
        private readonly AstraNuGetClientFactory _clientfactory;

        public PackageMetadataClient(AstraNuGetClientFactory clientFactory)
        {
            this._clientfactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
        }

        public async Task<RegistrationIndexResponse> GetRegistrationIndexOrNullAsync(
            string packageId,
            CancellationToken cancellationToken = default)
        {
            var client = await this._clientfactory.GetPackageMetadataClientAsync(cancellationToken);

            return await client.GetRegistrationIndexOrNullAsync(packageId, cancellationToken);
        }

        public async Task<RegistrationPageResponse> GetRegistrationPageAsync(
            string pageUrl,
            CancellationToken cancellationToken = default)
        {
            var client = await this._clientfactory.GetPackageMetadataClientAsync(cancellationToken);

            return await client.GetRegistrationPageAsync(pageUrl, cancellationToken);
        }

        public async Task<RegistrationLeafResponse> GetRegistrationLeafAsync(
            string leafUrl,
            CancellationToken cancellationToken = default)
        {
            var client = await this._clientfactory.GetPackageMetadataClientAsync(cancellationToken);

            return await client.GetRegistrationLeafAsync(leafUrl, cancellationToken);
        }
    }
}