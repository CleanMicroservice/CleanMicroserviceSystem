using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Extensions;
using CleanMicroserviceSystem.Themis.Application.DataTransferObjects.ApiScopes;
using CleanMicroserviceSystem.Themis.Application.Models;
using CleanMicroserviceSystem.Themis.Application.Repository;
using CleanMicroserviceSystem.Themis.Application.Services;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Themis.Infrastructure.Services
{
    public class ClientManager : IClientManager
    {
        private readonly ILogger<ClientManager> logger;
        private readonly IApiResourceRepository apiResourceRepository;
        private readonly IApiScopeRepository apiScopeRepository;
        private readonly IClientRepository clientRepository;
        private readonly IClientApiScopeMapRepository clientApiScopeMapRepository;

        public ClientManager(
            ILogger<ClientManager> logger,
            IApiResourceRepository apiResourceRepository,
            IApiScopeRepository apiScopeRepository,
            IClientRepository clientRepository,
            IClientApiScopeMapRepository clientApiScopeMapRepository)
        {
            this.logger = logger;
            this.apiResourceRepository = apiResourceRepository;
            this.apiScopeRepository = apiScopeRepository;
            this.clientRepository = clientRepository;
            this.clientApiScopeMapRepository = clientApiScopeMapRepository;
        }

        public async Task<ClientSigninResult> SignInAsync(string clientName, string clientSecret)
        {
            var client = await this.clientRepository.FindClientAsync(clientName);
            var result = new ClientSigninResult(client);
            if (client == null)
            {
                result.Error = $"Can't find client with name {clientName}.";
                return result;
            }
            var secret = clientSecret?.Sha256();
            if (client.Secret != secret)
            {
                result.Error = $"Incorrect secret for client {clientName}.";
                return result;
            }
            if (!client.Enabled)
            {
                result.Error = $"Client {clientName} is locked.";
                return result;
            }

            return result;
        }

        public async Task<IEnumerable<ApiScopeInformationResponse>?> GetClientApiScopes(int clientId)
        {
            var client = await this.clientRepository.FindAsync(clientId);
            if (client is null) return null;

            var maps = await this.clientApiScopeMapRepository.GetClientApiScopeMaps(clientId);
            var scopes = maps.Select(map => new ApiScopeInformationResponse()
            {
                Description = map.ApiScope.Description,
                Name = map.ApiScope.Name,
                Enabled = map.ApiScope.Enabled
            });
            return scopes;
        }
    }
}
