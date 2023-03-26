using CleanMicroserviceSystem.Authentication.Domain;
using CleanMicroserviceSystem.DataStructure;
using CleanMicroserviceSystem.Themis.Application.Services;
using CleanMicroserviceSystem.Themis.Contract.ApiResources;
using CleanMicroserviceSystem.Themis.Domain.Entities.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanMicroserviceSystem.Themis.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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
    [Authorize(Policy = IdentityContract.ThemisAPIReadPolicyName)]
    public async Task<ActionResult<ApiResourceInformationResponse>> Get(int id)
    {
        this.logger.LogInformation($"Get API Resource: {id}");
        var resource = await this.apiResourceManager.FindByIdAsync(id);
        return resource is null
            ? this.NotFound()
            : this.Ok(new ApiResourceInformationResponse()
            {
                Id = resource.Id,
                Name = resource.Name,
                Enabled = resource.Enabled,
                Description = resource.Description,
            });
    }

    /// <summary>
    /// Search api resources information
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet(nameof(Search))]
    [Authorize(Policy = IdentityContract.ThemisAPIReadPolicyName)]
    public async Task<ActionResult<PaginatedEnumerable<ApiResourceInformationResponse>>> Search([FromQuery] ApiResourceSearchRequest request)
    {
        this.logger.LogInformation($"Search API Resources: {request.Id}, {request.Name}, {request.Enabled}");
        var result = await this.apiResourceManager.SearchAsync(
            request.Id, request.Name, request.Enabled, request.Start, request.Count);
        var resources = result.Values.Select(resource => new ApiResourceInformationResponse()
        {
            Id = resource.Id,
            Name = resource.Name,
            Enabled = resource.Enabled,
            Description = resource.Description,
        }).ToArray();
        var paginatedResources = new PaginatedEnumerable<ApiResourceInformationResponse>(
            resources, result.StartItemIndex, result.PageSize, result.OriginItemCount);
        return this.Ok(paginatedResources);
    }

    /// <summary>
    /// Create api resource information
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Policy = IdentityContract.ThemisAPIWritePolicyName)]
    public async Task<ActionResult<CommonResult<ApiResourceInformationResponse>>> Post([FromBody] ApiResourceCreateRequest request)
    {
        this.logger.LogInformation($"Create API Resource: {request.Name}");
        var newResource = new ApiResource()
        {
            Name = request.Name,
            Enabled = request.Enabled,
            Description = request.Description,
        };
        var result = await this.apiResourceManager.CreateAsync(newResource);
        var commonResult = new CommonResult<ApiResourceInformationResponse>(result.Errors);
        if (!result.Succeeded)
        {
            return this.BadRequest(commonResult);
        }
        else
        {
            newResource = await this.apiResourceManager.FindByIdAsync(newResource.Id);
            commonResult.Entity = new ApiResourceInformationResponse()
            {
                Id = newResource!.Id,
                Name = newResource.Name,
                Enabled = newResource.Enabled,
                Description = newResource.Description,
            };
            return this.Ok(commonResult);
        }
    }

    /// <summary>
    /// Update api resource information
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [Authorize(Policy = IdentityContract.ThemisAPIWritePolicyName)]
    public async Task<ActionResult<CommonResult>> Put(int id, [FromBody] ApiResourceUpdateRequest request)
    {
        this.logger.LogInformation($"Update API Resource: {id}");
        var resource = await this.apiResourceManager.FindByIdAsync(id);
        if (resource is null)
            return this.NotFound(new CommonResult(new CommonResultError($"Can not find Api Resource with id: {id}")));

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

        var commonResult = await this.apiResourceManager.UpdateAsync(resource);
        return commonResult.Succeeded ? this.Ok(commonResult) : this.BadRequest(commonResult);
    }

    /// <summary>
    /// Delete api resource information
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [Authorize(Policy = IdentityContract.ThemisAPIWritePolicyName)]
    public async Task<ActionResult<CommonResult>> Delete(int id)
    {
        this.logger.LogInformation($"Delete API Resource: {id}");
        var resource = await this.apiResourceManager.FindByIdAsync(id);
        if (resource is null)
            return this.NotFound();
        var commonResult = await this.apiResourceManager.DeleteAsync(resource);
        return this.Ok(commonResult);
    }
    #endregion
}
