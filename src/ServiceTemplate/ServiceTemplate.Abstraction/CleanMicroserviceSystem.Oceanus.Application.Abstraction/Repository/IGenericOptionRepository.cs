using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Entities;

namespace CleanMicroserviceSystem.Oceanus.Application.Abstraction.Repository;

public interface IGenericOptionRepository : IRepositoryBase<GenericOption>
{
    Task<GenericOption?> QueryGenericOptionAsync(string optionName, string? owner = null, string? category = null);
}
