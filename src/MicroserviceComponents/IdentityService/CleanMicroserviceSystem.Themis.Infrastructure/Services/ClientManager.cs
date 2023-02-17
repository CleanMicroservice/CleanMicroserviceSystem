using CleanMicroserviceSystem.Authentication.Domain;
using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Entities;
using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Extensions;
using CleanMicroserviceSystem.Themis.Application.DataTransferObjects.Clients;
using CleanMicroserviceSystem.Themis.Application.Repository;
using CleanMicroserviceSystem.Themis.Application.Services;
using CleanMicroserviceSystem.Themis.Domain.Entities.Configuration;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Themis.Infrastructure.Services
{
    public class ClientManager : IClientManager
    {
        private readonly ILogger<ClientManager> logger;
        private readonly IClientRepository clientRepository;
        private readonly IClientApiScopeMapRepository clientApiScopeMapRepository;

        public ClientManager(
            ILogger<ClientManager> logger,
            IClientRepository clientRepository,
            IClientApiScopeMapRepository clientApiScopeMapRepository)
        {
            this.logger = logger;
            this.clientRepository = clientRepository;
            this.clientApiScopeMapRepository = clientApiScopeMapRepository;
        }

        public async Task<ClientResult> SignInAsync(string clientName, string clientSecret)
        {
            var client = await this.clientRepository.FindClientByNameAsync(clientName);
            var result = new ClientResult() { Client = client };
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

        public async Task<PaginatedEnumerable<Client>> SearchAsync(
            int? id, string? name, bool? enabled, int start, int count)
        {
            return await this.clientRepository.SearchAsync(id, name, enabled, start, count);
        }

        public async Task<Client?> FindByIdAsync(int clientId)
        {
            return await this.clientRepository.FindAsync(clientId);
        }

        public async Task<ClientResult> CreateAsync(Client client)
        {
            client = await this.clientRepository.AddAsync(client);
            await this.clientRepository.SaveChangesAsync();
            return new ClientResult() { Client = client };
        }

        public async Task<ClientResult> UpdateAsync(Client client)
        {
            client = await this.clientRepository.UpdateAsync(client);
            await this.clientRepository.SaveChangesAsync();
            return new ClientResult() { Client = client };
        }

        public async Task<ClientResult> DeleteAsync(Client client)
        {
            await this.clientRepository.RemoveAsync(client);
            await this.clientRepository.SaveChangesAsync();
            return new ClientResult() { Client = client };
        }

        public async Task<bool> CheckScopeAsync(int clientId, int scopeId)
        {
            var map = await this.clientApiScopeMapRepository.FindAsync(clientId, scopeId);
            return map is not null;
        }

        public async Task<IEnumerable<ApiScope>?> GetClientScopesAsync(int clientId)
        {
            var client = await this.clientRepository.FindAsync(clientId);
            if (client is null) return null;

            var maps = await this.clientApiScopeMapRepository.GetClientApiScopeMaps(clientId);
            var scopes = maps.Select(map => map.ApiScope);
            return scopes;
        }

        public async Task<ClientApiScopeMap> CreateScopeAsync(int clientId, int scopeId)
        {
            var map = await this.clientApiScopeMapRepository.AddAsync(new ClientApiScopeMap()
            {
                ClientID = clientId,
                ApiScopeID = scopeId,
                CreatedBy = IdentityContract.SuperUserId,
                CreatedOn = DateTime.UtcNow,
            });
            await this.clientApiScopeMapRepository.SaveChangesAsync();
            return map;
        }

        public async Task<ClientApiScopeMap?> DeleteScopeAsync(int clientId, int scopeId)
        {
            var map = await this.clientApiScopeMapRepository.FindAsync(clientId, scopeId);
            if (map is null) return null;
            map = await this.clientApiScopeMapRepository.RemoveAsync(map!);
            await this.clientApiScopeMapRepository.SaveChangesAsync();
            return map;
        }
    }
}
