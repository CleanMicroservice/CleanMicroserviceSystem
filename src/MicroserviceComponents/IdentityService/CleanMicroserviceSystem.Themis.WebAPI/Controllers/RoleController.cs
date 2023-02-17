using System.Security.Claims;
using CleanMicroserviceSystem.Authentication.Domain;
using CleanMicroserviceSystem.Themis.Application.DataTransferObjects.Claims;
using CleanMicroserviceSystem.Themis.Application.DataTransferObjects.Roles;
using CleanMicroserviceSystem.Themis.Application.DataTransferObjects.Users;
using CleanMicroserviceSystem.Themis.Application.Repository;
using CleanMicroserviceSystem.Themis.Domain.Entities.Identity;
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
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet(nameof(Search))]
    public async Task<IActionResult> Search([FromQuery] RoleSearchRequest request)
    {
        var result = await this.oceanusRoleRepository.Search(
            request.Id, request.RoleName, request.Start, request.Count);
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
    public async Task<IActionResult> Post([FromBody] RoleCreateRequest request)
    {
        var newRole = new OceanusRole()
        {
            Name = request.RoleName
        };
        var result = await this.roleManager.CreateAsync(newRole);
        if (!result.Succeeded)
        {
            return this.BadRequest(result);
        }
        else
        {
            newRole = await this.roleManager.FindByNameAsync(newRole.Name);
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
    public async Task<IActionResult> Put(string id, [FromBody] RoleUpdateRequest request)
    {
        var role = await this.roleManager.FindByIdAsync(id);
        if (role is null)
            return this.NotFound();

        role.Name = request.RoleName;
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

    /// <summary>
    /// Get role claims
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}/Claims")]
    public async Task<IActionResult> GetClaims(string id)
    {
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
    public async Task<IActionResult> PutClaims(string id, [FromBody] IEnumerable<ClaimsUpdateRequest> requests)
    {
        var role = await this.roleManager.FindByIdAsync(id);
        if (role is null)
            return this.NotFound();

        var existingClaims = await this.roleManager.GetClaimsAsync(role);
        var existingClaimSet = existingClaims.Select(claim => (claim.Type, claim.Value)).ToHashSet();
        var requestClaimSet = requests.Select(claim => (claim.Type, claim.Value)).ToHashSet();

        {
            var claimsToRemove = existingClaims.Where(claim => !requestClaimSet.Contains((claim.Type, claim.Value))).ToArray();
            if (claimsToRemove.Any())
            {
                foreach (var claimToRemove in claimsToRemove)
                {
                    var result = await this.roleManager.RemoveClaimAsync(role, claimToRemove);
                    if (!result.Succeeded)
                    {
                        return this.BadRequest(result);
                    }
                }
            }
        }

        {
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
                        return this.BadRequest(result);
                    }
                }
            }
        }

        existingClaims = await this.roleManager.GetClaimsAsync(role);
        var claims = existingClaims.Select(claim => new ClaimInformationResponse()
        {
            Type = claim.Type,
            Value = claim.Value
        });
        return this.Ok(claims);
    }

    /// <summary>
    /// Add role claims
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requests"></param>
    /// <returns></returns>
    [HttpPost("{id}/Claims")]
    public async Task<IActionResult> PostClaims(int id, [FromBody] IEnumerable<ClaimsUpdateRequest> requests)
    {
        var role = await this.roleManager.FindByIdAsync(id.ToString());
        if (role is null)
            return this.NotFound();

        var existingClaims = await this.roleManager.GetClaimsAsync(role);
        var existingClaimSet = existingClaims.Select(claim => (claim.Type, claim.Value)).ToHashSet();

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
                    return this.BadRequest(result);
                }
            }
            return this.Ok(true);
        }

        return this.NoContent();
    }

    /// <summary>
    /// Delete role claims
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requests"></param>
    /// <returns></returns>
    [HttpDelete("{id}/Claims")]
    public async Task<IActionResult> DeleteClaims(int id, [FromBody] IEnumerable<ClaimsUpdateRequest> requests)
    {
        var role = await this.roleManager.FindByIdAsync(id.ToString());
        if (role is null)
            return this.NotFound();

        var existingClaims = await this.roleManager.GetClaimsAsync(role);
        var existingClaimMap = existingClaims.ToDictionary(claim => (claim.Type, claim.Value), claim => claim);
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
                    return this.BadRequest(result);
                }
            }
            return this.Ok(true);
        }

        return this.NoContent();
    }
    #endregion

    #region RoleUsers

    /// <summary>
    /// Get role users
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}/Users")]
    public async Task<IActionResult> GetUsers(int id, [FromQuery] UserSearchRequest request)
    {
        var users = await this.oceanusRoleRepository.SearchUsers(
            new[] { id }, request.Id, request.UserName, request.Email, request.PhoneNumber, request.Start, request.Count);
        return this.Ok(users);
    }

    /// <summary>
    /// Add role users
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requests"></param>
    /// <returns></returns>
    [HttpPost("{id}/Users")]
    public async Task<IActionResult> PostUsers(string id, [FromBody] IEnumerable<string> requests)
    {
        if (!requests.Any())
            return this.NoContent();

        var role = await this.roleManager.FindByIdAsync(id);
        if (role is null)
            return this.NotFound();

        IdentityResult? result = default;
        foreach (var userId in requests)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            if (user is null ||
                await this.userManager.IsInRoleAsync(user, role!.Name!)) continue;

            result = await this.userManager.AddToRoleAsync(user, role!.Name!);
            if (!result.Succeeded)
            {
                return this.BadRequest(result);
            }
        }

        return this.Ok(result ?? default);
    }

    /// <summary>
    /// Delete role users
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requests"></param>
    /// <returns></returns>
    [HttpDelete("{id}/Users")]
    public async Task<IActionResult> DeleteUsers(string id, [FromBody] IEnumerable<string> requests)
    {
        if (!requests.Any())
            return this.NoContent();

        var role = await this.roleManager.FindByIdAsync(id);
        if (role is null)
            return this.NotFound();

        IdentityResult? result = default;
        foreach (var userId in requests)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            if (user is null ||
                !await this.userManager.IsInRoleAsync(user, role!.Name!)) continue;

            result = await this.userManager.RemoveFromRoleAsync(user, role!.Name!);
            if (!result.Succeeded)
            {
                return this.BadRequest(result);
            }
        }

        return this.Ok(result ?? default);
    }
    #endregion
}
