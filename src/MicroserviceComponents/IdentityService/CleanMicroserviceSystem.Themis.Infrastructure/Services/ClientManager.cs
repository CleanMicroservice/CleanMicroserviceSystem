using CleanMicroserviceSystem.DataStructure;
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
        this.logger.LogDebug($"Sign in client: {clientName}");
        var client = await this.clientRepository.FindClientByNameAsync(clientName);
        var result = new CommonResult<Client>() { Entity = client };
        if (client == null)
        {
            result.Errors = new[] { new CommonResultError($"Can't find client with name {clientName}.") };
            return result;
        }
        var secret = clientSecret?.Sha256();
        if (client.Secret != secret)
        {
            result.Errors = new[] { new CommonResultError($"Incorrect secret for client {clientName}.") };
            return result;
        }
        if (!client.Enabled)
        {
            result.Errors = new[] { new CommonResultError($"Client {clientName} is locked.") };
            return result;
        }

        if (result.Succeeded)
            this.logger.LogDebug($"Client {clientName} signs in successfully.");
        else
            this.logger.LogWarning($"Client {clientName} signs in failed.:{result}");
        return result;
    }

    public async Task SignOutAsync(string clientName)
    {
        this.logger.LogDebug($"Sign out client {clientName}");
        await Task.CompletedTask;
    }

    public async Task<PaginatedEnumerable<Client>> SearchAsync(
        int? id, string? name, bool? enabled, int? start, int? count)
    {
        this.logger.LogDebug($"Search clients: {id}, {name}, {enabled}, {start}, {count}");
        return await this.clientRepository.SearchAsync(id, name, enabled, start, count);
    }

    public async Task<Client?> FindByIdAsync(int clientId)
    {
        this.logger.LogDebug($"Find client {clientId}");
        return await this.clientRepository.FindAsync(clientId);
    }

    public async Task<Client?> FindByNameAsync(string clientName)
    {
        this.logger.LogDebug($"Find client {clientName}");
        return await this.clientRepository.FindClientByNameAsync(clientName);
    }

    public async Task<CommonResult<Client>> CreateAsync(Client client)
    {
        this.logger.LogDebug($"Create client {client.Name}");
        client = await this.clientRepository.AddAsync(client);
        _ = await this.clientRepository.SaveChangesAsync();
        return new CommonResult<Client>() { Entity = client };
    }

    public async Task<CommonResult> UpdateAsync(Client client)
    {
        this.logger.LogDebug($"Update client {client.Id}");
        client = await this.clientRepository.UpdateAsync(client);
        _ = await this.clientRepository.SaveChangesAsync();
        return CommonResult.Success;
    }

    public async Task<CommonResult> DeleteAsync(Client client)
    {
        this.logger.LogDebug($"Delete client {client.Id}");
        _ = await this.clientRepository.RemoveAsync(client);
        _ = await this.clientRepository.SaveChangesAsync();
        return CommonResult.Success;
    }

    public async Task<IEnumerable<ClientClaim>> GetClaimsAsync(int clientId)
    {
        this.logger.LogDebug($"Get client claims: {clientId}");
        return this.clientClaimRepository.AsQueryable().Where(claim => claim.ClientId == clientId);
    }

    public async Task<int> AddClaimsAsync(IEnumerable<ClientClaim> claims)
    {
        this.logger.LogDebug($"Add client claims: {string.Join(";", claims.Select(claim => $"{claim.ClientId}:{claim.ClaimType}"))}");
        var result = await this.clientClaimRepository.AddRangeAsync(claims);
        _ = await this.clientClaimRepository.SaveChangesAsync();
        return result;
    }

    public async Task<int> RemoveClaimsAsync(IEnumerable<int> claimIds)
    {
        this.logger.LogDebug($"Remove client claims: {string.Join(";", claimIds)}");
        foreach (var claimId in claimIds)
        {
            var claim = await this.clientClaimRepository.FindAsync(claimId);
            if (claim is null) continue;
            _ = await this.clientClaimRepository.RemoveAsync(claim);
        }
        var result = await this.clientClaimRepository.SaveChangesAsync();
        return result;
    }
}
