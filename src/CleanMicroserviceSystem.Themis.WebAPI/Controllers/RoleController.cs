using CleanMicroserviceSystem.Common.Contracts;
using CleanMicroserviceSystem.Themis.Application.DataTransferObjects.Roles;
using CleanMicroserviceSystem.Themis.Application.Repository;
using CleanMicroserviceSystem.Themis.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CleanMicroserviceSystem.Themis.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = IdentityContract.AccessRolesPolicy)]
public class RoleController : ControllerBase
{
    private readonly ILogger<RoleController> logger;
    private readonly UserManager<OceanusUser> userManager;
    private readonly RoleManager<OceanusRole> roleManager;
    private readonly IOceanusRoleRepository oceanusRoleRepository;

    public RoleController(
        ILogger<RoleController> logger,
        UserManager<OceanusUser> userManager,
        RoleManager<OceanusRole> roleManager,
        IOceanusRoleRepository oceanusRoleRepository)
    {
        this.logger = logger;
        this.userManager = userManager;
        this.roleManager = roleManager;
        this.oceanusRoleRepository = oceanusRoleRepository;
    }

    #region Roles

    /// <summary>
    /// Get role information
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var role = await this.roleManager.FindByIdAsync(id);
        return role is null
            ? this.NotFound()
            : this.Ok(new RoleInformationResponse()
            {
                Id = role.Id,
                RoleName = role.Name
            });
    }

    /// <summary>
    /// Search roles information
    /// </summary>
    /// <param name="id"></param>
    /// <param name="roleName"></param>
    /// <param name="start"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    [HttpGet(nameof(Search))]
    public async Task<IActionResult> Search(
        int? id,
        string? roleName = null,
        int start = 0,
        int count = 10)
    {
        var result = await this.oceanusRoleRepository.Search(id, roleName, start, count);
        var roles = result.Select(role => new RoleInformationResponse()
        {
            Id = role.Id,
            RoleName = role.Name,
        });
        return this.Ok(roles);
    }

    /// <summary>
    /// Create role information
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Post([FromBody] string request)
    {
        var newRole = new OceanusRole()
        {
            Name = request
        };
        var result = await this.roleManager.CreateAsync(newRole);
        if (!result.Succeeded)
        {
            return this.BadRequest(result);
        }
        else
        {
            newRole = await this.roleManager.FindByNameAsync(request);
            return this.Ok(new RoleInformationResponse()
            {
                Id = newRole!.Id,
                RoleName = newRole!.Name,
            });
        }
    }

    /// <summary>
    /// Update role information
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] string request)
    {
        var role = await this.roleManager.FindByIdAsync(id);
        if (role is null)
            return this.NotFound();

        role.Name = request;
        var result = await this.roleManager.UpdateAsync(role);
        if (!result.Succeeded)
        {
            return this.BadRequest(result);
        }
        else
        {
            role = await this.roleManager.FindByIdAsync(role.Id.ToString());
            return this.Ok(new RoleInformationResponse()
            {
                Id = role!.Id,
                RoleName = role!.Name
            });
        }
    }

    /// <summary>
    /// Delete role information
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var role = await this.roleManager.FindByIdAsync(id);
        if (role is null)
            return this.NotFound();
        await this.roleManager.DeleteAsync(role);
        return this.Ok(true);
    }
    #endregion

    #region RoleClaims

    #endregion
}
