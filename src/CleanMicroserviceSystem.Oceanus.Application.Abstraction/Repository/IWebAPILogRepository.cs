using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Entities;
using static CleanMicroserviceSystem.Oceanus.Application.Abstraction.Repository.IRepositoryBase;

namespace CleanMicroserviceSystem.Oceanus.Application.Abstraction.Repository;

public interface IWebAPILogRepository : IRepositoryBase<WebAPILog>
{
}
