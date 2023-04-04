using System.Security.Claims;
using CleanMicroserviceSystem.Authentication.Domain;
using CleanMicroserviceSystem.DataStructure;
using CleanMicroserviceSystem.Themis.Application.Repository;
using CleanMicroserviceSystem.Themis.Contract.Claims;
using CleanMicroserviceSystem.Themis.Contract.Roles;
using CleanMicroserviceSystem.Themis.Contract.Users;
using CleanMicroserviceSystem.Themis.Domain.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CleanMicroserviceSystem.Themis.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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
    [Authorize(Policy = IdentityContract.ThemisAPIReadPolicyName)]
    public async Task<ActionResult<RoleInformationResponse>> Get(string id)
    {
        this.logger.LogInformation($"Get Role: {id}");
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
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet(nameof(Search))]
    [Authorize(Policy = IdentityContract.ThemisAPIReadPolicyName)]
    public async Task<ActionResult<PaginatedEnumerable<RoleInformationResponse>>> Search([FromQuery] RoleSearchRequest request)
    {
        this.logger.LogInformation($"Search Roles: {request.Id}, {request.RoleName}, {request.Start}, {request.Count}");
        var result = await this.oceanusRoleRepository.SearchAsync(
            request.Id, request.RoleName, request.Start, request.Count);
        var roles = result.Values.Select(role => new RoleInformationResponse()
        {
            Id = role.Id,
            RoleName = role.Name,
        });
        var paginatedRoles = new PaginatedEnumerable<RoleInformationResponse>(
            roles, result.StartItemIndex, result.PageSize, result.OriginItemCount);
        return this.Ok(paginatedRoles);
    }

    /// <summary>
    /// Create role information
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Policy = IdentityContract.ThemisAPIWritePolicyName)]
    public async Task<ActionResult<CommonResult<RoleInformationResponse>>> Post([FromBody] RoleCreateRequest request)
    {
        this.logger.LogInformation($"Create Role: {request.RoleName}");
        var newRole = new OceanusRole()
        {
            Name = request.RoleName
        };
        var commonResult = new CommonResult<RoleInformationResponse>();
        var result = await this.roleManager.CreateAsync(newRole);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                commonResult.Errors.Add(new CommonResultError(error.Code, error.Description));
            }
        }
        else
        {
            newRole = await this.roleManager.FindByNameAsync(newRole.Name);
            commonResult.Entity = new RoleInformationResponse()
            {
                Id = newRole!.Id,
                RoleName = newRole!.Name,
            };
        }

        return commonResult.Succeeded ? this.Ok(commonResult) : this.BadRequest(commonResult);
    }

    /// <summary>
    /// Update role information
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [Authorize(Policy = IdentityContract.ThemisAPIWritePolicyName)]
    public async Task<ActionResult<CommonResult>> Put(string id, [FromBody] RoleUpdateRequest request)
    {
        this.logger.LogInformation($"Update Role: {request.RoleName}");
        var role = await this.roleManager.FindByIdAsync(id);
        if (role is null)
            return this.NotFound(new CommonResult(new CommonResultError($"Can not find Role with id: {id}")));

        role.Name = request.RoleName;
        var result = await this.roleManager.UpdateAsync(role);
        var commonResult = new CommonResult(
            result.Errors.Select(error => new CommonResultError(error.Code, error.Description)).ToList());
        return result.Succeeded ? this.Ok(commonResult) : this.BadRequest(commonResult);
    }

    /// <summary>
    /// Delete role information
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [Authorize(Policy = IdentityContract.ThemisAPIWritePolicyName)]
    public async Task<ActionResult<CommonResult>> Delete(string id)
    {
        this.logger.LogInformation($"Delete Role: {id}");
        var role = await this.roleManager.FindByIdAsync(id);
        if (role is null)
            return this.NotFound(new CommonResult(new CommonResultError($"Can not find Role with id: {id}")));
        var result = await this.roleManager.DeleteAsync(role);
        var commonResult = new CommonResult(result.Errors.Select(error => new CommonResultError(error.Code, error.Description)).ToList());
        return this.Ok(commonResult);
    }
    #endregion

    #region RoleClaims

    /// <summary>
    /// Get role claims
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}/Claims")]
    [Authorize(Policy = IdentityContract.ThemisAPIReadPolicyName)]
    public async Task<ActionResult<IEnumerable<ClaimInformationResponse>>> GetClaims(string id)
    {
        this.logger.LogInformation($"Add Role Claims: {id}");
        var role = await this.roleManager.FindByIdAsync(id);
        if (role is null)
            return this.NotFound();

        var result = await this.roleManager.GetClaimsAsync(role);
        var claims = result.Select(claim => new ClaimInformationResponse()
        {
            Type = claim.Type,
            Value = claim.Value
        });
        return this.Ok(claims);
    }

    /// <summary>
    /// Update role claims
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requests"></param>
    /// <returns></returns>
    [HttpPut("{id}/Claims")]
    [Authorize(Policy = IdentityContract.ThemisAPIWritePolicyName)]
    public async Task<ActionResult<CommonResult>> PutClaims(string id, [FromBody] IEnumerable<ClaimUpdateRequest> requests)
    {
        this.logger.LogInformation($"Update Role Claims: {id}");
        var role = await this.roleManager.FindByIdAsync(id);
        if (role is null)
            return this.NotFound(new CommonResult(new CommonResultError($"Can not find Role with id: {id}")));

        var existingClaims = await this.roleManager.GetClaimsAsync(role);
        var existingClaimSet = existingClaims.Select(claim => (claim.Type, claim.Value)).ToHashSet();
        var requestClaimSet = requests.Select(claim => (claim.Type, claim.Value)).ToHashSet();
        var commonResult = new CommonResult();

        var claimsToRemove = existingClaims.Where(claim => !requestClaimSet.Contains((claim.Type, claim.Value))).ToArray();
        if (claimsToRemove.Any())
        {
            foreach (var claimToRemove in claimsToRemove)
            {
                var result = await this.roleManager.RemoveClaimAsync(role, claimToRemove);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        commonResult.Errors.Add(new CommonResultError(error.Code, error.Description));
                    }
                }
            }
        }
        var claimsToAdd = requests
            .Where(claim => !existingClaimSet.Contains((claim.Type, claim.Value)))
            .Select(claim => new Claim(claim.Type, claim.Value))
            .ToArray();
        if (claimsToAdd.Any())
        {
            foreach (var claimToAdd in claimsToAdd)
            {
                var result = await this.roleManager.AddClaimAsync(role, claimToAdd);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        commonResult.Errors.Add(new CommonResultError(error.Code, error.Description));
                    }
                }
            }
        }

        return commonResult.Succeeded ? this.Ok(commonResult) : this.BadRequest(commonResult);
    }

    /// <summary>
    /// Add role claims
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requests"></param>
    /// <returns></returns>
    [HttpPost("{id}/Claims")]
    [Authorize(Policy = IdentityContract.ThemisAPIWritePolicyName)]
    public async Task<ActionResult<CommonResult>> PostClaims(int id, [FromBody] IEnumerable<ClaimUpdateRequest> requests)
    {
        this.logger.LogInformation($"Create Role Claims: {id}");
        var role = await this.roleManager.FindByIdAsync(id.ToString());
        if (role is null)
            return this.NotFound(new CommonResult(new CommonResultError($"Can not find Role with id: {id}")));

        var existingClaims = await this.roleManager.GetClaimsAsync(role);
        var existingClaimSet = existingClaims.Select(claim => (claim.Type, claim.Value)).ToHashSet();
        var commonResult = new CommonResult();

        var claimsToAdd = requests
            .Where(claim => !existingClaimSet.Contains((claim.Type, claim.Value)))
            .Select(claim => new Claim(claim.Type, claim.Value))
            .ToArray();
        if (claimsToAdd.Any())
        {
            foreach (var claimToAdd in claimsToAdd)
            {
                var result = await this.roleManager.AddClaimAsync(role, claimToAdd);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        commonResult.Errors.Add(new CommonResultError(error.Code, error.Description));
                    }
                }
            }
        }

        return commonResult.Succeeded ? this.Ok(commonResult) : this.BadRequest(commonResult);
    }

    /// <summary>
    /// Delete role claims
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requests"></param>
    /// <returns></returns>
    [HttpDelete("{id}/Claims")]
    [Authorize(Policy = IdentityContract.ThemisAPIWritePolicyName)]
    public async Task<ActionResult<CommonResult>> DeleteClaims(int id, [FromBody] IEnumerable<ClaimUpdateRequest> requests)
    {
        this.logger.LogInformation($"Delete Role Claims: {id}");
        var role = await this.roleManager.FindByIdAsync(id.ToString());
        if (role is null)
            return this.NotFound(new CommonResult(new CommonResultError($"Can not find Role with id: {id}")));

        var existingClaims = await this.roleManager.GetClaimsAsync(role);
        var existingClaimMap = existingClaims.ToDictionary(claim => (claim.Type, claim.Value), claim => claim);
        var commonResult = new CommonResult();

        var claimsToRemove = requests
            .Select(claim => existingClaimMap.TryGetValue((claim.Type, claim.Value), out var existingClaim) ? existingClaim : null)
            .OfType<Claim>()
            .ToArray();
        if (claimsToRemove.Any())
        {
            foreach (var claimToRemove in claimsToRemove)
            {
                var result = await this.roleManager.RemoveClaimAsync(role, claimToRemove);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        commonResult.Errors.Add(new CommonResultError(error.Code, error.Description));
                    }
                }
            }
        }

        return commonResult.Succeeded ? this.Ok(commonResult) : this.BadRequest(commonResult);

    }
    #endregion

    #region RoleUsers

    /// <summary>
    /// Get role users
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}/Users")]
    [Authorize(Policy = IdentityContract.ThemisAPIReadPolicyName)]
    public async Task<ActionResult<PaginatedEnumerable<UserInformationResponse>>> GetUsers(int id, [FromQuery] UserSearchRequest request)
    {
        this.logger.LogInformation($"Get Role Users: {id}");
        var result = await this.oceanusRoleRepository.SearchUsersAsync(
            id, request.Id, request.UserName, request.Email, request.PhoneNumber, request.Start, request.Count);
        var users = result.Values.Select(user => new UserInformationResponse()
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Enabled = !(user.LockoutEnabled && user.LockoutEnd.HasValue && user.LockoutEnd.Value >= DateTime.UtcNow),
        });
        var paginatedUsers = new PaginatedEnumerable<UserInformationResponse>(
            users, result.StartItemIndex, result.PageSize, result.OriginItemCount);
        return this.Ok(paginatedUsers);
    }

    /// <summary>
    /// Add role users
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requests"></param>
    /// <returns></returns>
    [HttpPost("{id}/Users")]
    [Authorize(Policy = IdentityContract.ThemisAPIWritePolicyName)]
    public async Task<ActionResult<CommonResult>> PostUsers(string id, [FromBody] IEnumerable<int> requests)
    {
        this.logger.LogInformation($"Create Role Users: {id}");
        var role = await this.roleManager.FindByIdAsync(id);
        if (role is null)
            return this.NotFound(new CommonResult(new CommonResultError($"Can not find Role with id: {id}")));

        var commonResult = new CommonResult();
        foreach (var userId in requests)
        {
            var user = await this.userManager.FindByIdAsync(userId.ToString());
            if (user is null ||
                await this.userManager.IsInRoleAsync(user, role!.Name!))
            {
                continue;
            }

            var result = await this.userManager.AddToRoleAsync(user, role!.Name!);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    commonResult.Errors.Add(new CommonResultError(error.Code, error.Description));
                }
            }
        }

        return commonResult.Succeeded ? this.Ok(commonResult) : this.BadRequest(commonResult);
    }

    /// <summary>
    /// Update role users
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requests"></param>
    /// <returns></returns>
    [HttpPut("{id}/Users")]
    [Authorize(Policy = IdentityContract.ThemisAPIWritePolicyName)]
    public async Task<ActionResult<CommonResult>> PutUsers(int id, [FromBody] IEnumerable<int> requests)
    {
        this.logger.LogInformation($"Update Role Users: {id}");
        var role = await this.roleManager.FindByIdAsync(id.ToString());
        if (role is null)
            return this.NotFound(new CommonResult(new CommonResultError($"Can not find Role with id: {id}")));

        var existingUsers = await this.oceanusRoleRepository.GetUsersAsync(id , null, null);
        var existingUserSet = existingUsers.Values.Select(user => user.Id).ToHashSet();
        var requestUserSet = requests.ToHashSet();
        var commonResult = new CommonResult();

        var usersToRemove = existingUsers.Values.Where(user => !requestUserSet.Contains(user.Id)).ToArray();
        foreach (var userToRemove in usersToRemove)
        {
            var result = await this.userManager.RemoveFromRoleAsync(userToRemove, role!.Name!);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    commonResult.Errors.Add(new CommonResultError(error.Code, error.Description));
                }
            }
        }

        var userIdsToAdd = requests
            .Where(userId => !existingUserSet.Contains(userId))
            .ToArray();
        foreach (var userId in requests)
        {
            var user = await this.userManager.FindByIdAsync(userId.ToString());
            if (user is null ||
                await this.userManager.IsInRoleAsync(user, role!.Name!))
            {
                continue;
            }

            var result = await this.userManager.AddToRoleAsync(user, role!.Name!);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    commonResult.Errors.Add(new CommonResultError(error.Code, error.Description));
                }
            }
        }

        return commonResult.Succeeded ? this.Ok(commonResult) : this.BadRequest(commonResult);
    }

    /// <summary>
    /// Delete role users
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requests"></param>
    /// <returns></returns>
    [HttpDelete("{id}/Users")]
    [Authorize(Policy = IdentityContract.ThemisAPIWritePolicyName)]
    public async Task<ActionResult<CommonResult>> DeleteUsers(string id, [FromBody] IEnumerable<int> requests)
    {
        this.logger.LogInformation($"Delete Role Users: {id}");
        var role = await this.roleManager.FindByIdAsync(id);
        if (role is null)
            return this.NotFound(new CommonResult(new CommonResultError($"Can not find Role with id: {id}")));

        var commonResult = new CommonResult();
        foreach (var userId in requests)
        {
            var user = await this.userManager.FindByIdAsync(userId.ToString());
            if (user is null ||
                !await this.userManager.IsInRoleAsync(user, role!.Name!))
            {
                continue;
            }

            var result = await this.userManager.RemoveFromRoleAsync(user, role!.Name!);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    commonResult.Errors.Add(new CommonResultError(error.Code, error.Description));
                }
            }
        }

        return commonResult.Succeeded ? this.Ok(commonResult) : this.BadRequest(commonResult);
    }
    #endregion
}
