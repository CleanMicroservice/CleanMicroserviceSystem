﻿using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Entities;
using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Repository;
using CleanMicroserviceSystem.Themis.Application.Repository;
using CleanMicroserviceSystem.Themis.Domain.Entities.Configuration;
using CleanMicroserviceSystem.Themis.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Themis.Infrastructure.Repository
{
    public class ClientRepository : RepositoryBase<Client>, IClientRepository
    {
        public ClientRepository(
            ILogger<ClientRepository> logger,
            ConfigurationDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public async Task<Client?> FindClientAsync(string name)
        {
            return await this.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<PaginatedEnumerable<Client>> SearchAsync(
            int? id,
            string? name,
            bool? enabled,
            int start,
            int count)
        {
            var clients = this.AsQueryable().AsNoTracking();
            if (id.HasValue)
                clients = clients.Where(client => client.ID == id);
            if (!string.IsNullOrEmpty(name))
                clients = clients.Where(client => EF.Functions.Like(client.Name, $"%{name}%"));
            var originCounts = await clients.CountAsync();
            clients = clients.Skip(start).Take(count);
            return new PaginatedEnumerable<Client>(clients.ToArray(), start, count, originCounts);
        }
    }
}