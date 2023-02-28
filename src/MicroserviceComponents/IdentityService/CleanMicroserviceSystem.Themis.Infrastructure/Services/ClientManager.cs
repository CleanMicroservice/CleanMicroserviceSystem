using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Entities;
using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Models;
using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Extensions;
using CleanMicroserviceSystem.Themis.Application.Repository;
using CleanMicroserviceSystem.Themis.Application.Services;
using CleanMicroserviceSystem.Themis.Domain.Entities.Configuration;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Themis.Infrastructure.Services;

public class ClientManager : IClientManager
{
    private readonly ILogger<ClientManager> logger;
    private readonly IClientRepository clientRepository;
    private readonly IClientClaimRepository clientClaimRepository;

    public ClientManager(
        ILogger<ClientManager> logger,
        IClientRepository clientRepository,
        IClientClaimRepository clientClaimRepository)
    {
        this.logger = logger;
        this.clientRepository = clientRepository;
        this.clientClaimRepository = clientClaimRepository;
    }

    public async Task<CommonResult<Client>> SignInAsync(string clientName, string clientSecret)
    {
        var client = await this.clientRepository.FindClientByNameAsync(clientName);
        var result = new CommonResult<Client>() { Entity = client };
        if (client == null)
        {
            result.Error = $"Can't find client with name {clientName}.";
            return result;
        }
        var secret = clientSecret?.Sha256();
        if (client.Secret != secret)
        {
            result.Error = $"Incorrect secret for client {clientName}.";
            return result;
        }
        if (!client.Enabled)
        {
            result.Error = $"Client {clientName} is locked.";
            return result;
        }

        return result;
    }

    public async Task SignOutAsync()
        => await Task.CompletedTask;

    public async Task<PaginatedEnumerable<Client>> SearchAsync(
        int? id, string? name, bool? enabled, int start, int count)
        => await this.clientRepository.SearchAsync(id, name, enabled, start, count);

    public async Task<Client?> FindByIdAsync(int clientId)
        => await this.clientRepository.FindAsync(clientId);

    public async Task<Client?> FindByNameAsync(string clientName)
        => await this.clientRepository.FindClientByNameAsync(clientName);

    public async Task<CommonResult<Client>> CreateAsync(Client client)
    {
        client = await this.clientRepository.AddAsync(client);
        await this.clientRepository.SaveChangesAsync();
        return new CommonResult<Client>() { Entity = client };
    }

    public async Task<CommonResult<Client>> UpdateAsync(Client client)
    {
        client = await this.clientRepository.UpdateAsync(client);
        await this.clientRepository.SaveChangesAsync();
        return new CommonResult<Client>() { Entity = client };
    }

    public async Task<CommonResult<Client>> DeleteAsync(Client client)
    {
        await this.clientRepository.RemoveAsync(client);
        await this.clientRepository.SaveChangesAsync();
        return new CommonResult<Client>() { Entity = client };
    }

    public async Task<IEnumerable<ClientClaim>> GetClaimsAsync(int clientId)
        => this.clientClaimRepository.AsQueryable().Where(claim => claim.ClientId == clientId);

    public async Task<int> AddClaimsAsync(IEnumerable<ClientClaim> claims)
    {
        var result = await this.clientClaimRepository.AddRangeAsync(claims);
        await this.clientClaimRepository.SaveChangesAsync();
        return result;
    }

    public async Task<int> RemoveClaimsAsync(IEnumerable<int> claimIds)
    {
        foreach (var claimId in claimIds)
        {
            var claim = await this.clientClaimRepository.FindAsync(claimId);
            await this.clientClaimRepository.RemoveAsync(claim);
        }
        var result = await this.clientClaimRepository.SaveChangesAsync();
        return result;
    }
}
