using CleanMicroserviceSystem.Oceanus.Application.Abstraction.Repository;
using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Repository;

public class WebAPILogRepository : RepositoryBase<WebAPILog>, IWebAPILogRepository
{
    public WebAPILogRepository(
        ILogger<WebAPILogRepository> logger,
        DbContext dbContext)
        : base(logger, dbContext)
    {
    }
}
