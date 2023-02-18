using CleanMicroserviceSystem.Authentication.Domain;
using CleanMicroserviceSystem.Themis.Application.DataTransferObjects.ApiScopes;
using CleanMicroserviceSystem.Themis.Application.DataTransferObjects.Clients;
using CleanMicroserviceSystem.Themis.Application.Services;
using CleanMicroserviceSystem.Themis.Domain.Entities.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanMicroserviceSystem.Themis.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = IdentityContract.AccessRolesPolicy)]
public class ClientController : ControllerBase
{
    private readonly ILogger<ClientController> logger;
    private readonly IApiResourceManager apiResourceManager;
    private readonly IClientManager clientManager;

    public ClientController(
        ILogger<ClientController> logger,
        IApiResourceManager apiResourceManager,
        IClientManager clientManager)
    {
        this.logger = logger;
        this.apiResourceManager = apiResourceManager;
        this.clientManager = clientManager;
    }

    #region Clients

    /// <summary>
    /// Get client information
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var client = await this.clientManager.FindByIdAsync(id);
        return client is null
            ? this.NotFound()
            : this.Ok(new ClientInformationResponse()
            {
                Id = client.Id,
                Name = client.Name,
                Enabled = client.Enabled,
                Description = client.Description,
            });
    }

    /// <summary>
    /// Search clients information
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet(nameof(Search))]
    public async Task<IActionResult> Search([FromQuery] ClientSearchRequest request)
    {
        var result = await this.clientManager.SearchAsync(
            request.Id, request.Name, request.Enabled, request.Start, request.Count);
        var clients = result.Select(client => new ClientInformationResponse()
        {
            Id = client.Id,
            Name = client.Name,
            Enabled = client.Enabled,
            Description = client.Description,
        });
        return this.Ok(clients);
    }

    /// <summary>
    /// Create client information
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ClientCreateRequest request)
    {
        var newClient = new Client()
        {
            Name = request.Name,
            Enabled = request.Enabled,
            Description = request.Description,
        };
        var result = await this.clientManager.CreateAsync(newClient);
        if (!result.Succeeded)
        {
            return this.BadRequest(result);
        }
        else
        {
            newClient = await this.clientManager.FindByIdAsync(newClient.Id);
            return this.Ok(new ClientInformationResponse()
            {
                Id = newClient.Id,
                Name = newClient.Name,
                Enabled = newClient.Enabled,
                Description = newClient.Description,
            });
        }
    }

    /// <summary>
    /// Update client information
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] ClientUpdateRequest request)
    {
        var client = await this.clientManager.FindByIdAsync(id);
        if (client is null)
            return this.NotFound();

        if (request.Enabled.HasValue)
        {
            client.Enabled = request.Enabled.Value;
        }
        if (request.Description is not null)
        {
            client.Description = request.Description;
        }
        if (request.Name is not null)
        {
            client.Name = request.Name;
        }
        if (request.Secret is not null)
        {
            client.Secret = request.Secret;
        }

        var result = await this.clientManager.UpdateAsync(client);
        if (!result.Succeeded)
        {
            return this.BadRequest(result);
        }
        else
        {
            client = await this.clientManager.FindByIdAsync(client.Id);
            return this.Ok(new ClientInformationResponse()
            {
                Id = client.Id,
                Name = client.Name,
                Enabled = client.Enabled,
                Description = client.Description,
            });
        }
    }

    /// <summary>
    /// Delete client information
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var client = await this.clientManager.FindByIdAsync(id);
        if (client is null)
            return this.NotFound();
        await this.clientManager.DeleteAsync(client);
        return this.Ok(true);
    }
    #endregion

    #region ClientScopes

    /// <summary>
    /// Get client scopes
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id}/Scopes")]
    public async Task<IActionResult> GetScopes(int id)
    {
        var scopes = await this.clientManager.GetClientScopesAsync(id);
        var scopeDtos = scopes?.Select(scope => new ApiScopeInformationResponse()
        {
            ID = scope.ID,
            Description = scope.Description,
            Name = scope.Name,
            Enabled = scope.Enabled
        })?.ToArray();
        return this.Ok(scopeDtos);
    }

    /// <summary>
    /// Add client scopes
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requests"></param>
    /// <returns></returns>
    [HttpPost("{id}/Scopes")]
    public async Task<IActionResult> PostScopes(int id, [FromBody] IEnumerable<int> requests)
    {
        if (!requests.Any())
            return this.NoContent();

        var client = await this.clientManager.FindByIdAsync(id);
        if (client is null)
            return this.NotFound();

        foreach (var scopeId in requests)
        {
            var mapped = await this.clientManager.CheckScopeAsync(id, scopeId);
            if (mapped) continue;

            var scope = await apiResourceManager.FindScopeByIdAsync(scopeId);
            if (scope is null) continue;

            await this.clientManager.CreateScopeAsync(id, scopeId);
        }
        return this.Ok();
    }

    /// <summary>
    /// Delete client scopes
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requests"></param>
    /// <returns></returns>
    [HttpDelete("{id}/Scopes")]
    public async Task<IActionResult> DeleteScopes(int id, [FromBody] IEnumerable<int> requests)
    {
        if (!requests.Any())
            return this.NoContent();

        var client = await this.clientManager.FindByIdAsync(id);
        if (client is null)
            return this.NotFound();

        foreach (var scopeId in requests)
        {
            var mapped = await this.clientManager.CheckScopeAsync(id, scopeId);
            if (!mapped) continue;

            await this.clientManager.DeleteScopeAsync(id, scopeId);
        }
        return this.Ok();
    }
    #endregion
}
