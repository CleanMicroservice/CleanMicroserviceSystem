using CleanMicroserviceSystem.Oceanus.Application.Abstraction.Repository;
using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Repository;

public class GenericOptionRepository : RepositoryBase<GenericOption>, IGenericOptionRepository
{
    public GenericOptionRepository(
        ILogger<GenericOptionRepository> logger,
        DbContext dbContext)
        : base(logger, dbContext)
    {
    }

    public async Task<GenericOption?> QueryGenericOptionAsync(string optionName, string? owner = null, string? category = null)
        => await this
            .AsQueryable()
            .Where(
                o => (o.OptionName == optionName) &&
                (string.IsNullOrEmpty(o.OwnerLevel) || o.OwnerLevel == owner) &&
                (string.IsNullOrEmpty(o.Category) || o.Category == category))
            .OrderByDescending(o => o.OwnerLevel)
            .ThenByDescending(o => o.Category)
            .FirstOrDefaultAsync();
}
