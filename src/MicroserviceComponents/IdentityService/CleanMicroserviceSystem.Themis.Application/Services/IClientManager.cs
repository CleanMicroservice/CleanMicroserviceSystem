using CleanMicroserviceSystem.DataStructure;
using CleanMicroserviceSystem.Themis.Domain.Entities.Configuration;

namespace CleanMicroserviceSystem.Themis.Application.Services;

public interface IClientManager
{
    Task<PaginatedEnumerable<Client>> SearchAsync(
        int? id, string? name, bool? enabled, int? start, int? count);

    Task<CommonResult<Client>> SignInAsync(string clientName, string clientSecret);

    Task SignOutAsync(string clientName);

    Task<Client?> FindByIdAsync(int clientId);

    Task<Client?> FindByNameAsync(string clientName);

    Task<CommonResult<Client>> CreateAsync(Client client);

    Task<CommonResult> UpdateAsync(Client client);

    Task<CommonResult> DeleteAsync(Client client);

    Task<IEnumerable<ClientClaim>> GetClaimsAsync(int clientId);

    Task<int> AddClaimsAsync(IEnumerable<ClientClaim> claims);

    Task<int> RemoveClaimsAsync(IEnumerable<int> claimIds);
}
