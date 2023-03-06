using CleanMicroserviceSystem.DataStructure;
using CleanMicroserviceSystem.Oceanus.Application.Abstraction.Repository;
using CleanMicroserviceSystem.Themis.Domain.Entities.Configuration;

namespace CleanMicroserviceSystem.Themis.Application.Repository;

public interface IClientRepository : IRepositoryBase<Client>
{
    Task<Client?> FindClientByNameAsync(string name);

    Task<PaginatedEnumerable<Client>> SearchAsync(
        int? id,
        string? name,
        bool? enabled,
        int? start,
        int? count);
}
