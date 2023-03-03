using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Entities;
using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Repository;
using CleanMicroserviceSystem.Themis.Application.Repository;
using CleanMicroserviceSystem.Themis.Domain.Entities.Configuration;
using CleanMicroserviceSystem.Themis.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Themis.Infrastructure.Repository;

public class ClientRepository : RepositoryBase<Client>, IClientRepository
{
    public ClientRepository(
        ILogger<ClientRepository> logger,
        ConfigurationDbContext dbContext)
        : base(logger, dbContext)
    {
    }

    public async Task<Client?> FindClientByNameAsync(string name)
        => await this.FirstOrDefaultAsync(x => x.Name == name);

    public async Task<PaginatedEnumerable<Client>> SearchAsync(
        int? id,
        string? name,
        bool? enabled,
        int? start,
        int? count)
    {
        var clients = this.AsQueryable();
        if (id.HasValue)
            clients = clients.Where(client => client.Id == id);
        if (!string.IsNullOrEmpty(name))
            clients = clients.Where(client => EF.Functions.Like(client.Name, $"%{name}%"));
        if (enabled.HasValue)
            clients = clients.Where(client => client.Enabled == enabled.Value);
        var originCounts = await clients.CountAsync();
        clients = clients.OrderBy(user => user.Id);
        if (start.HasValue)
            clients = clients.Skip(start.Value);
        if (count.HasValue)
            clients = clients.Take(count.Value);
        return new PaginatedEnumerable<Client>(clients.ToArray(), start, count, originCounts);
    }
}
