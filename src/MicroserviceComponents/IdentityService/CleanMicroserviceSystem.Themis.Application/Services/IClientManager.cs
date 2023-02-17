using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Entities;
using CleanMicroserviceSystem.Themis.Application.DataTransferObjects.Clients;
using CleanMicroserviceSystem.Themis.Domain.Entities.Configuration;

namespace CleanMicroserviceSystem.Themis.Application.Services
{
    public interface IClientManager
    {
        Task<IEnumerable<ApiScope>?> GetClientScopesAsync(int clientId);

        Task<PaginatedEnumerable<Client>> SearchAsync(int? id, string? name, bool? enabled, int start, int count);

        Task<ClientResult> SignInAsync(string clientName, string clientSecret);

        Task<Client?> FindByIdAsync(int clientId);

        Task<ClientResult> CreateAsync(Client client);

        Task<ClientResult> UpdateAsync(Client client);

        Task<ClientResult> DeleteAsync(Client client);

        Task<bool> CheckScopeAsync(int clientId, int scopeId);

        Task<ClientApiScopeMap> CreateScopeAsync(int clientId, int scopeId);

        Task<ClientApiScopeMap?> DeleteScopeAsync(int clientId, int scopeId);
    }
}
