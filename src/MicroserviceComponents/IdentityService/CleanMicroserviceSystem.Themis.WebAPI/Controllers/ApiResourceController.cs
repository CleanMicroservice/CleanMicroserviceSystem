using CleanMicroserviceSystem.Authentication.Domain;
using CleanMicroserviceSystem.Themis.Application.DataTransferObjects.ApiResources;
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
public class ApiResourceController : ControllerBase
{
    private readonly ILogger<ApiResourceController> logger;
    private readonly IApiResourceManager apiResourceManager;

    public ApiResourceController(
        ILogger<ApiResourceController> logger,
        IApiResourceManager apiResourceManager)
    {
        this.logger = logger;
        this.apiResourceManager = apiResourceManager;
    }

    #region ApiResources

    /// <summary>
    /// Get api resource information
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var resource = await this.apiResourceManager.FindResourceByIdAsync(id);
        return resource is null
            ? this.NotFound()
            : this.Ok(new ApiResourceInformationResponse()
            {
                ID = resource.ID,
                Name = resource.Name,
                Enabled = resource.Enabled,
                Description = resource.Description,
                ApiScopes = resource.ApiScopes?.Select(x => new ApiScopeInformationResponse()
                {
                    ID = x.ID,
                    Name = x.Name,
                    Enabled = x.Enabled,
                    Description = x.Description,
                }),
            });
    }

    /// <summary>
    /// Search api resources information
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet(nameof(Search))]
    public async Task<IActionResult> Search([FromQuery] ApiResourceSearchRequest request)
    {
        var result = await this.apiResourceManager.SearchAsync(
            request.Id, request.Name, request.Enabled, request.Start, request.Count);
        var resources = result.Select(resource => new ApiResourceInformationResponse()
        {
            ID = resource.ID,
            Name = resource.Name,
            Enabled = resource.Enabled,
            Description = resource.Description,
            ApiScopes = resource.ApiScopes?.Select(x => new ApiScopeInformationResponse()
            {
                ID = x.ID,
                Name = x.Name,
                Enabled = x.Enabled,
                Description = x.Description,
            }),
        }).ToArray();
        return this.Ok(resources);
    }

    /// <summary>
    /// Create api resource information
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ApiResourceCreateRequest request)
    {
        var newResource = new ApiResource()
        {
            Name = request.Name,
            Enabled = request.Enabled,
            Description = request.Description,
        };
        var result = await this.apiResourceManager.CreateAsync(newResource);
        if (!result.Succeeded)
        {
            return this.BadRequest(result);
        }
        else
        {
            newResource = await this.apiResourceManager.FindResourceByIdAsync(newResource.ID);
            return this.Ok(new ApiResourceInformationResponse()
            {
                ID = newResource.ID,
                Name = newResource.Name,
                Enabled = newResource.Enabled,
                Description = newResource.Description,
            });
        }
    }

    /// <summary>
    /// Update api resource information
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] ApiResourceUpdateRequest request)
    {
        var resource = await this.apiResourceManager.FindResourceByIdAsync(id);
        if (resource is null)
            return this.NotFound();

        if (request.Enabled.HasValue)
        {
            resource.Enabled = request.Enabled.Value;
        }
        if (request.Description is not null)
        {
            resource.Description = request.Description;
        }
        if (request.Name is not null)
        {
            resource.Name = request.Name;
        }

        var result = await this.apiResourceManager.UpdateAsync(resource);
        if (!result.Succeeded)
        {
            return this.BadRequest(result);
        }
        else
        {
            resource = await this.apiResourceManager.FindResourceByIdAsync(resource.ID);
            return this.Ok(new ApiResourceInformationResponse()
            {
                ID = resource.ID,
                Name = resource.Name,
                Enabled = resource.Enabled,
                Description = resource.Description,
            });
        }
    }

    /// <summary>
    /// Delete api resource information
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var resource = await this.apiResourceManager.FindResourceByIdAsync(id);
        if (resource is null)
            return this.NotFound();
        await this.apiResourceManager.DeleteAsync(resource);
        return this.Ok(true);
    }
    #endregion

    #region ApiResourceScopes

    /// <summary>
    /// Get api resource scopes
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id}/Scopes")]
    public async Task<IActionResult> GetScopes(int id)
    {
        var scopes = await this.apiResourceManager.GetResourceScopesAsync(id);
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
    /// Add api resource scope
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("{id}/Scopes")]
    public async Task<IActionResult> PostScope(int id, [FromBody] ApiScopeCreateRequest request)
    {
        var resource = await this.apiResourceManager.FindResourceByIdAsync(id);
        if (resource is null)
            return this.NotFound();

        var newScope = new ApiScope()
        {
            ApiResourceID = resource.ID,
            Name = request.Name,
            Enabled = request.Enabled,
            Description = request.Description,
        };
        var result = await this.apiResourceManager.CreateScopeAsync(newScope);
        if (!result.Succeeded)
        {
            return this.BadRequest(result);
        }
        else
        {
            newScope = await this.apiResourceManager.FindScopeByIdAsync(newScope.ID);
            return this.Ok(new ApiScopeInformationResponse()
            {
                ID = newScope.ID,
                Name = newScope.Name,
                Enabled = newScope.Enabled,
                Description = newScope.Description,
            });
        }
    }

    /// <summary>
    /// Delete api resource scope
    /// </summary>
    /// <param name="id"></param>
    /// <param name="scopeId"></param>
    /// <returns></returns>
    [HttpDelete("{id}/Scopes/{scopeId}")]
    public async Task<IActionResult> DeleteScope(int id, int scopeId)
    {
        var resource = await this.apiResourceManager.FindResourceByIdAsync(id);
        if (resource is null)
            return this.NotFound();
        var scope = await this.apiResourceManager.FindScopeByIdAsync(scopeId);
        if (scope is null)
            return this.NotFound();
        await this.apiResourceManager.DeleteScopeAsync(scope);
        return this.Ok(true);
    }
    #endregion
}
