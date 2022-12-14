using System.Security.Claims;
using CleanMicroserviceSystem.Common.Contracts;
using CleanMicroserviceSystem.Themis.Application.DataTransferObjects.Claims;
using CleanMicroserviceSystem.Themis.Application.DataTransferObjects.Users;
using CleanMicroserviceSystem.Themis.Application.Repository;
using CleanMicroserviceSystem.Themis.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CleanMicroserviceSystem.Themis.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> logger;
    private readonly UserManager<OceanusUser> userManager;
    private readonly RoleManager<OceanusRole> roleManager;
    private readonly IOceanusUserRepository oceanusUserRepository;

    public UserController(
        ILogger<UserController> logger,
        UserManager<OceanusUser> userManager,
        RoleManager<OceanusRole> roleManager,
        IOceanusUserRepository oceanusUserRepository)
    {
        this.logger = logger;
        this.userManager = userManager;
        this.roleManager = roleManager;
        this.oceanusUserRepository = oceanusUserRepository;
    }

    #region UserSelf

    /// <summary>
    /// Get user information
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var userName = this.HttpContext.User?.Identity?.Name;
        if (string.IsNullOrEmpty(userName))
            return this.BadRequest(new ArgumentException());

        var user = await this.userManager.FindByNameAsync(userName!);
        return user is null
            ? this.NotFound()
            : this.Ok(new UserInformationResponse()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            });
    }

    /// <summary>
    /// Update user information
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<IActionResult> Put([FromBody] UserUpdateRequest request)
    {
        var userName = this.HttpContext.User?.Identity?.Name;
        if (string.IsNullOrEmpty(userName))
            return this.BadRequest(new ArgumentException());

        var user = await this.userManager.FindByNameAsync(userName!);
        if (user is null)
            return this.NotFound();

        if (!string.IsNullOrEmpty(request.UserName))
        {
            user.UserName = request.UserName;
        }
        if (!string.IsNullOrEmpty(request.PhoneNumber))
        {
            user.PhoneNumber = request.PhoneNumber;
        }
        if (!string.IsNullOrEmpty(request.Email))
        {
            user.Email = request.Email;
        }

        var result = await this.userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return this.BadRequest(result);
        }
        else
        {
            user = await this.userManager.FindByIdAsync(user.Id.ToString());
            return this.Ok(new UserInformationResponse()
            {
                Id = user!.Id,
                UserName = user!.UserName,
                Email = user!.Email,
                PhoneNumber = user!.PhoneNumber
            });
        }
    }
    #endregion

    #region Users

    /// <summary>
    /// Get user information
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [Authorize(Policy = IdentityContract.AccessUsersPolicy)]
    public async Task<IActionResult> Get(string id)
    {
        var user = await this.userManager.FindByIdAsync(id);
        return user is null
            ? this.NotFound()
            : this.Ok(new UserInformationResponse()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            });
    }

    /// <summary>
    /// Search users information
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet(nameof(Search))]
    [Authorize(Policy = IdentityContract.AccessUsersPolicy)]
    public async Task<IActionResult> Search([FromQuery] UserSearchRequest request)
    {
        var result = await this.oceanusUserRepository.Search(
            request.Id, request.UserName, request.Email, request.PhoneNumber, request.Start, request.Count);
        var users = result.Select(user => new UserInformationResponse()
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        });
        return this.Ok(users);
    }

    /// <summary>
    /// Register user information
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Post([FromBody] UserRegisterRequest request)
    {
        var newUser = new OceanusUser()
        {
            UserName = request.UserName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
        };
        var result = await this.userManager.CreateAsync(newUser, request.Password);
        if (!result.Succeeded)
        {
            return this.BadRequest(result);
        }
        else
        {
            newUser = await this.userManager.FindByNameAsync(request.UserName);
            return this.Ok(new UserInformationResponse()
            {
                Id = newUser!.Id,
                UserName = newUser!.UserName,
                Email = newUser!.Email,
                PhoneNumber = newUser!.PhoneNumber
            });
        }
    }

    /// <summary>
    /// Update user information
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [Authorize(Policy = IdentityContract.AccessUsersPolicy)]
    public async Task<IActionResult> Put(string id, [FromBody] UserUpdateRequest request)
    {
        var user = await this.userManager.FindByIdAsync(id);
        if (user is null)
            return this.NotFound();

        if (!string.IsNullOrEmpty(request.UserName))
        {
            var existedUser = await this.userManager.FindByNameAsync(request.UserName);
            if (existedUser is not null &&
                existedUser.Id != user.Id)
            {
                return this.BadRequest();
            }
            user.UserName = request.UserName;
        }
        if (!string.IsNullOrEmpty(request.Email))
        {
            var existedUser = await this.userManager.FindByEmailAsync(request.Email);
            if (existedUser is not null &&
                existedUser.Id != user.Id)
            {
                return this.BadRequest();
            }
            user.Email = request.Email;
        }
        if (!string.IsNullOrEmpty(request.PhoneNumber))
        {
            userManager.FindByEmailAsync(request.PhoneNumber).Wait();
            user.PhoneNumber = request.PhoneNumber;
        }

        var result = await this.userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return this.BadRequest(result);
        }
        else
        {
            user = await this.userManager.FindByIdAsync(user.Id.ToString());
            return this.Ok(new UserInformationResponse()
            {
                Id = user!.Id,
                UserName = user!.UserName,
                Email = user!.Email,
                PhoneNumber = user!.PhoneNumber
            });
        }
    }

    /// <summary>
    /// Delete user information
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [Authorize(Policy = IdentityContract.AccessUsersPolicy)]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await this.userManager.FindByIdAsync(id);
        if (user is null)
            return this.NotFound();
        await this.userManager.DeleteAsync(user);
        return this.Ok(true);
    }
    #endregion

    #region UserClaims

    /// <summary>
    /// Get user claims
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}/Claims")]
    [Authorize(Policy = IdentityContract.AccessUsersPolicy)]
    public async Task<IActionResult> GetClaims(string id)
    {
        var user = await this.userManager.FindByIdAsync(id);
        if (user is null)
            return this.NotFound();

        var result = await this.userManager.GetClaimsAsync(user);
        var claims = result.Select(claim => new ClaimInformationResponse()
        {
            Type = claim.Type,
            Value = claim.Value
        });
        return this.Ok(claims);
    }

    /// <summary>
    /// Update user claims
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requests"></param>
    /// <returns></returns>
    [HttpPut("{id}/Claims")]
    [Authorize(Policy = IdentityContract.AccessUsersPolicy)]
    public async Task<IActionResult> PutClaims(string id, [FromBody] IEnumerable<ClaimsUpdateRequest> requests)
    {
        var user = await this.userManager.FindByIdAsync(id);
        if (user is null)
            return this.NotFound();

        var existingClaims = await this.userManager.GetClaimsAsync(user);
        var existingClaimSet = existingClaims.Select(claim => (claim.Type, claim.Value)).ToHashSet();
        var requestClaimSet = requests.Select(claim => (claim.Type, claim.Value)).ToHashSet();

        {
            var claimsToRemove = existingClaims.Where(claim => !requestClaimSet.Contains((claim.Type, claim.Value))).ToArray();
            if (claimsToRemove.Any())
            {
                var result = await this.userManager.RemoveClaimsAsync(user, claimsToRemove);
                if (!result.Succeeded)
                {
                    return this.BadRequest(result);
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
                var result = await this.userManager.AddClaimsAsync(user, claimsToAdd);
                if (!result.Succeeded)
                {
                    return this.BadRequest(result);
                }
            }
        }

        existingClaims = await this.userManager.GetClaimsAsync(user);
        var claims = existingClaims.Select(claim => new ClaimInformationResponse()
        {
            Type = claim.Type,
            Value = claim.Value
        });
        return this.Ok(claims);
    }

    /// <summary>
    /// Add user claims
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requests"></param>
    /// <returns></returns>
    [HttpPost("{id}/Claims")]
    [Authorize(Policy = IdentityContract.AccessUsersPolicy)]
    public async Task<IActionResult> PostClaims(string id, [FromBody] IEnumerable<ClaimsUpdateRequest> requests)
    {
        var user = await this.userManager.FindByIdAsync(id);
        if (user is null)
            return this.NotFound();

        var existingClaims = await this.userManager.GetClaimsAsync(user);
        var existingClaimSet = existingClaims.Select(claim => (claim.Type, claim.Value)).ToHashSet();

        var claimsToAdd = requests
            .Where(claim => !existingClaimSet.Contains((claim.Type, claim.Value)))
            .Select(claim => new Claim(claim.Type, claim.Value))
            .ToArray();
        if (claimsToAdd.Any())
        {
            var result = await this.userManager.AddClaimsAsync(user, claimsToAdd);
            return result.Succeeded ? this.Ok(result) : this.BadRequest(result);
        }

        return this.NoContent();
    }

    /// <summary>
    /// Delete user claims
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requests"></param>
    /// <returns></returns>
    [HttpDelete("{id}/Claims")]
    [Authorize(Policy = IdentityContract.AccessUsersPolicy)]
    public async Task<IActionResult> DeleteClaims(string id, [FromBody] IEnumerable<ClaimsUpdateRequest> requests)
    {
        var user = await this.userManager.FindByIdAsync(id);
        if (user is null)
            return this.NotFound();

        var existingClaims = await this.userManager.GetClaimsAsync(user);
        var existingClaimMap = existingClaims.ToDictionary(claim => (claim.Type, claim.Value), claim => claim);
        var claimsToRemove = requests
            .Select(claim => existingClaimMap.TryGetValue((claim.Type, claim.Value), out var existingClaim) ? existingClaim : null)
            .OfType<Claim>()
            .ToArray();
        if (claimsToRemove.Any())
        {
            var result = await this.userManager.RemoveClaimsAsync(user, claimsToRemove);
            return result.Succeeded ? this.Ok(result) : this.BadRequest(result);
        }

        return this.NoContent();
    }
    #endregion

    #region UserRoles

    /// <summary>
    /// Get user roles
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}/Roles")]
    [Authorize(Policy = IdentityContract.AccessUsersPolicy)]
    public async Task<IActionResult> GetRoles(string id)
    {
        var user = await this.userManager.FindByIdAsync(id);
        if (user is null)
            return this.NotFound();

        var roles = await this.userManager.GetRolesAsync(user);
        return this.Ok(roles);
    }

    /// <summary>
    /// Update user roles
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requests"></param>
    /// <returns></returns>
    [HttpPut("{id}/Roles")]
    [Authorize(Policy = IdentityContract.AccessUsersPolicy)]
    public async Task<IActionResult> PutRoles(string id, [FromBody] IEnumerable<string> requests)
    {
        var user = await this.userManager.FindByIdAsync(id);
        if (user is null)
            return this.NotFound();

        var existingRoles = await this.userManager.GetRolesAsync(user);
        var existingRoleSet = existingRoles.ToHashSet();
        var requestRoleSet = requests.ToHashSet();

        {
            var rolesToRemove = existingRoleSet
                .Except(requestRoleSet)
                .Where(role => this.userManager.IsInRoleAsync(user, role).Result)
                .ToArray();
            if (rolesToRemove.Any())
            {
                var result = await this.userManager.RemoveFromRolesAsync(user, rolesToRemove);
                if (!result.Succeeded)
                {
                    return this.BadRequest(result);
                }
            }
        }

        {
            var rolesToAdd = requestRoleSet
                .Except(existingRoleSet)
                .Where(role =>
                    this.roleManager.RoleExistsAsync(role).Result &&
                    !this.userManager.IsInRoleAsync(user, role).Result)
                .ToArray();
            if (rolesToAdd.Any())
            {
                var result = await this.userManager.AddToRolesAsync(user, rolesToAdd);
                if (!result.Succeeded)
                {
                    return this.BadRequest(result);
                }
            }
        }

        existingRoles = await this.userManager.GetRolesAsync(user);
        return this.Ok(existingRoles);
    }

    /// <summary>
    /// Add user roles
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requests"></param>
    /// <returns></returns>
    [HttpPost("{id}/Roles")]
    [Authorize(Policy = IdentityContract.AccessUsersPolicy)]
    public async Task<IActionResult> PostRoles(string id, [FromBody] IEnumerable<string> requests)
    {
        var user = await this.userManager.FindByIdAsync(id);
        if (user is null)
            return this.NotFound();

        requests = requests
            .Where(role =>
                this.roleManager.RoleExistsAsync(role).Result &&
                !this.userManager.IsInRoleAsync(user, role).Result)
            .ToArray();

        if (!requests.Any())
            return this.NoContent();

        var result = await this.userManager.AddToRolesAsync(user, requests);
        return result.Succeeded ? this.Ok(result) : this.BadRequest(result);
    }

    /// <summary>
    /// Delete user roles
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requests"></param>
    /// <returns></returns>
    [HttpDelete("{id}/Roles")]
    [Authorize(Policy = IdentityContract.AccessUsersPolicy)]
    public async Task<IActionResult> DeleteRoles(string id, [FromBody] IEnumerable<string> requests)
    {
        var user = await this.userManager.FindByIdAsync(id);
        if (user is null)
            return this.NotFound();

        requests = requests
            .Where(role => this.userManager.IsInRoleAsync(user, role).Result)
            .ToArray();

        if (!requests.Any())
            return this.NoContent();

        var result = await this.userManager.RemoveFromRolesAsync(user, requests);
        return result.Succeeded ? this.Ok(result) : this.BadRequest(result);
    }
    #endregion
}
