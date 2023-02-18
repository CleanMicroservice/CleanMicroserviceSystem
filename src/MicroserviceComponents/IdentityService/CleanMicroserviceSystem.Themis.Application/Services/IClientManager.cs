using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Entities;
using CleanMicroserviceSystem.Themis.Application.DataTransferObjects.Clients;
using CleanMicroserviceSystem.Themis.Domain.Entities.Configuration;

namespace CleanMicroserviceSystem.Themis.Application.Services
{
    public interface IClientManager
    {
        Task<PaginatedEnumerable<Client>> SearchAsync(
            int? id, string? name, bool? enabled, int start, int count);

        Task<ClientResult> SignInAsync(string clientName, string clientSecret);

        Task<Client?> FindByIdAsync(int clientId);

        Task<ClientResult> CreateAsync(Client client);

        Task<ClientResult> UpdateAsync(Client client);

        Task<ClientResult> DeleteAsync(Client client);

        Task<IEnumerable<ClientClaim>> GetClaimsAsync(int clientId);

        Task<int> AddClaimsAsync(IEnumerable<ClientClaim> claims);

        Task<int> RemoveClaimsAsync(IEnumerable<int> claimIds);
    }
}
