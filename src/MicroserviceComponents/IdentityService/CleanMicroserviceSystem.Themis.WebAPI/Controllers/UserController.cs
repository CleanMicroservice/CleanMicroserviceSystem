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
    public async Task<ActionResult<UserInformationResponse>> Get()
    {
        var userName = this.HttpContext.User?.Identity?.Name;
        this.logger.LogInformation($"Get current user: {userName}");
        var user = await this.userManager.FindByNameAsync(userName!);
        return user is null
            ? this.NotFound()
            : this.Ok(new UserInformationResponse()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Enabled = !(user.LockoutEnabled && user.LockoutEnd.HasValue && user.LockoutEnd.Value >= DateTime.UtcNow),
            });
    }

    /// <summary>
    /// Update user information
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<ActionResult<CommonResult>> Put([FromBody] UserUpdateRequest request)
    {
        var userName = this.HttpContext.User?.Identity?.Name;
        this.logger.LogInformation($"Update current user: {userName}");
        var user = await this.userManager.FindByNameAsync(userName!);
        if (user is null)
            return this.NotFound(new CommonResult(new CommonResultError($"Can not find current User: {userName}")));

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
        if (request.Enabled.HasValue)
        {
            user.LockoutEnd = request.Enabled.Value ? null : DateTimeOffset.MaxValue;
        }

        var result = await this.userManager.UpdateAsync(user);
        var commonResult = new CommonResult(result.Errors.Select(error => new CommonResultError(error.Code, error.Description)).ToList());
        if (!string.IsNullOrEmpty(request.NewPassword))
        {
            if (string.IsNullOrEmpty(request.CurrentPassword))
            {
                commonResult.Errors.Add(new CommonResultError("Current password is required to change password."));
            }
            else
            {
                result = await this.userManager.ChangePasswordAsync(user, request.CurrentPassword!, request.NewPassword!);
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

    #region Users

    /// <summary>
    /// Get user information
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [Authorize(Policy = IdentityContract.ThemisAPIReadPolicyName)]
    public async Task<ActionResult<UserInformationResponse>> Get(string id)
    {
        this.logger.LogInformation($"Get User: {id}");
        var user = await this.userManager.FindByIdAsync(id);
        return user is null
            ? this.NotFound()
            : this.Ok(new UserInformationResponse()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Enabled = !(user.LockoutEnabled && user.LockoutEnd.HasValue && user.LockoutEnd.Value >= DateTime.UtcNow),
            });
    }

    /// <summary>
    /// Search users information
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet(nameof(Search))]
    [Authorize(Policy = IdentityContract.ThemisAPIReadPolicyName)]
    public async Task<ActionResult<PaginatedEnumerable<UserInformationResponse>>> Search([FromQuery] UserSearchRequest request)
    {
        this.logger.LogInformation($"Search Users: {request.Id}, {request.UserName}, {request.Email}, {request.PhoneNumber}, {request.Start}, {request.Count}");
        var result = await this.oceanusUserRepository.Search(
            request.Id, request.UserName, request.Email, request.PhoneNumber, request.Start, request.Count);
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
    /// Register user information
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<CommonResult<UserInformationResponse>>> Post([FromBody] UserRegisterRequest request)
    {
        this.logger.LogInformation($"Create User: {request.UserName}");
        var newUser = new OceanusUser()
        {
            UserName = request.UserName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
        };
        var result = await this.userManager.CreateAsync(newUser, request.Password);
        var commonResult = new CommonResult<UserInformationResponse>();
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                commonResult.Errors.Add(new CommonResultError(error.Code, error.Description));
            }
            return this.BadRequest(commonResult);
        }

        newUser = await this.oceanusUserRepository.FindAsync(newUser!.Id);
        // TODO: Split logic by intermediator
        result = await this.userManager.AddToRoleAsync(newUser!, IdentityContract.CommonRole);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                commonResult.Errors.Add(new CommonResultError(error.Code, error.Description));
            }
            return this.BadRequest(commonResult);
        }

        commonResult.Entity = new UserInformationResponse()
        {
            Id = newUser!.Id,
            UserName = newUser!.UserName,
            Email = newUser!.Email,
            PhoneNumber = newUser!.PhoneNumber,
            Enabled = !(newUser.LockoutEnabled && newUser.LockoutEnd.HasValue && newUser.LockoutEnd.Value >= DateTime.UtcNow),
        };

        return commonResult.Succeeded ? this.Ok(commonResult) : this.BadRequest(commonResult);
    }

    /// <summary>
    /// Synchronize user information
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost(nameof(Synchronize))]
    [Authorize(Policy = IdentityContract.ThemisAPIWritePolicyName)]
    public async Task<ActionResult<CommonResult<UserInformationResponse>>> Synchronize([FromBody] UserSynchronizeRequest request)
    {
        this.logger.LogInformation($"Synchronize User: {request.UserName}");
        var user = await this.userManager.FindByNameAsync(request.UserName);
        var commonResult = new CommonResult<UserInformationResponse>();
        if (user is null)
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
                foreach (var error in result.Errors)
                {
                    commonResult.Errors.Add(new CommonResultError(error.Code, error.Description));
                }
                return this.BadRequest(commonResult);
            }
            user = await this.oceanusUserRepository.FindAsync(newUser!.Id);
            result = await this.userManager.AddToRoleAsync(user!, IdentityContract.CommonRole);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    commonResult.Errors.Add(new CommonResultError(error.Code, error.Description));
                }
                return this.BadRequest(commonResult);
            }
        }
        else
        {
            user.PhoneNumber = request.PhoneNumber;
            user.Email = request.Email;

            var result = await this.userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    commonResult.Errors.Add(new CommonResultError(error.Code, error.Description));
                }
                return this.BadRequest(commonResult);
            }
            var resetPasswordToken = await this.userManager.GeneratePasswordResetTokenAsync(user);
            result = await this.userManager.ResetPasswordAsync(user, resetPasswordToken, request.Password!);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    commonResult.Errors.Add(new CommonResultError(error.Code, error.Description));
                }
                return this.BadRequest(commonResult);
            }
        }

        commonResult.Entity = new UserInformationResponse()
        {
            Id = user!.Id,
            UserName = user!.UserName,
            Email = user!.Email,
            PhoneNumber = user!.PhoneNumber,
            Enabled = !(user.LockoutEnabled && user.LockoutEnd.HasValue && user.LockoutEnd.Value >= DateTime.UtcNow),
        };

        var updateResult = await this.UpdateUserClaims(user!, request.Claims);
        if (!updateResult.Succeeded)
        {
            foreach (var error in updateResult.Errors)
            {
                commonResult.Errors.Add(error);
            }
            return this.BadRequest(commonResult);
        }
        updateResult = await this.UpdateUserRoles(user!, request.Roles?.Select(role => role.RoleName) ?? Enumerable.Empty<string>());
        if (!updateResult.Succeeded)
        {
            foreach (var error in updateResult.Errors)
            {
                commonResult.Errors.Add(error);
            }
            return this.BadRequest(commonResult);
        }
        return commonResult.Succeeded ? this.Ok(commonResult) : this.BadRequest(commonResult);
    }

    /// <summary>
    /// Update user information
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [Authorize(Policy = IdentityContract.ThemisAPIWritePolicyName)]
    public async Task<ActionResult<CommonResult>> Put(string id, [FromBody] UserUpdateRequest request)
    {
        this.logger.LogInformation($"Update User: {id}");
        var user = await this.userManager.FindByIdAsync(id);
        if (user is null)
            return this.NotFound(new CommonResult(new CommonResultError($"Can not find User with id: {id}")));

        if (!string.IsNullOrEmpty(request.UserName))
        {
            user.UserName = request.UserName;
        }
        if (!string.IsNullOrEmpty(request.Email))
        {
            user.Email = request.Email;
        }
        if (!string.IsNullOrEmpty(request.PhoneNumber))
        {
            user.PhoneNumber = request.PhoneNumber;
        }
        if (request.Enabled.HasValue)
        {
            user.LockoutEnd = request.Enabled.Value ? null : DateTimeOffset.MaxValue;
        }

        var result = await this.userManager.UpdateAsync(user);
        var commonResult = new CommonResult(
            result.Errors.Select(error => new CommonResultError(error.Code, error.Description)).ToList());
        if (!string.IsNullOrEmpty(request.NewPassword))
        {
            var resetPasswordToken = await this.userManager.GeneratePasswordResetTokenAsync(user);
            result = await this.userManager.ResetPasswordAsync(user, resetPasswordToken, request.NewPassword!);
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
    /// Delete user information
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [Authorize(Policy = IdentityContract.ThemisAPIWritePolicyName)]
    public async Task<ActionResult<CommonResult>> Delete(string id)
    {
        this.logger.LogInformation($"Delete User: {id}");
        var user = await this.userManager.FindByIdAsync(id);
        if (user is null)
            return this.NotFound(new CommonResult(new CommonResultError($"Can not find User with id: {id}")));
        var result = await this.userManager.DeleteAsync(user);
        var commonResult = new CommonResult(result.Errors.Select(error => new CommonResultError(error.Code, error.Description)).ToList());
        return this.Ok(commonResult);
    }
    #endregion

    #region UserClaims

    /// <summary>
    /// Get user claims
    /// </summary>
    /// <returns></returns>
    [HttpGet("Claims")]
    [Authorize(Policy = IdentityContract.ThemisAPIReadPolicyName)]
    public async Task<ActionResult<PaginatedEnumerable<ClaimInformationResponse>>> SearchClaims([FromQuery] ClaimSearchRequest request)
    {
        this.logger.LogInformation($"Search User Claims ...");
        var result = await this.oceanusUserRepository.SearchClaims(
            request.IdentityId, request.Type, request.Value, request.Start, request.Count);

        var claims = result.Values.Select(claim => new ClaimInformationResponse()
        {
            Type = claim.Type,
            Value = claim.Value,
        });
        var paginatedClaims = new PaginatedEnumerable<ClaimInformationResponse>(
            claims, result.StartItemIndex, result.PageSize, result.OriginItemCount);
        return this.Ok(paginatedClaims);
    }

    /// <summary>
    /// Get user claims
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}/Claims")]
    [Authorize(Policy = IdentityContract.ThemisAPIReadPolicyName)]
    public async Task<ActionResult<IEnumerable<ClaimInformationResponse>>> GetClaims(string id)
    {
        this.logger.LogInformation($"Get User Claims: {id}");
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
    [Authorize(Policy = IdentityContract.ThemisAPIWritePolicyName)]
    public async Task<ActionResult<CommonResult>> PutClaims(string id, [FromBody] IEnumerable<ClaimUpdateRequest> requests)
    {
        this.logger.LogInformation($"Update User Claims: {id}");
        var user = await this.userManager.FindByIdAsync(id);
        if (user is null)
            return this.NotFound(new CommonResult(new CommonResultError($"Can not find User with id: {id}")));
        var commonResult = await this.UpdateUserClaims(user, requests);
        return commonResult.Succeeded ? this.Ok(commonResult) : this.BadRequest(commonResult);
    }

    private async Task<CommonResult> UpdateUserClaims(OceanusUser user, IEnumerable<ClaimUpdateRequest> requests)
    {
        var existingClaims = await this.userManager.GetClaimsAsync(user);
        var existingClaimSet = existingClaims.Select(claim => (claim.Type, claim.Value)).ToHashSet();
        var requestClaimSet = requests.Select(claim => (claim.Type, claim.Value)).ToHashSet();
        var commonResult = new CommonResult();

        var claimsToRemove = existingClaims.Where(claim => !requestClaimSet.Contains((claim.Type, claim.Value))).ToArray();
        if (claimsToRemove.Any())
        {
            var result = await this.userManager.RemoveClaimsAsync(user, claimsToRemove);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    commonResult.Errors.Add(new CommonResultError(error.Code, error.Description));
                }
            }
        }

        var claimsToAdd = requests
            .Where(claim => !existingClaimSet.Contains((claim.Type, claim.Value)))
            .Select(claim => new Claim(claim.Type, claim.Value))
            .ToArray();
        if (claimsToAdd.Any())
        {
            var result = await this.userManager.AddClaimsAsync(user, claimsToAdd);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    commonResult.Errors.Add(new CommonResultError(error.Code, error.Description));
                }
            }
        }
        return commonResult;
    }

    /// <summary>
    /// Add user claims
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requests"></param>
    /// <returns></returns>
    [HttpPost("{id}/Claims")]
    [Authorize(Policy = IdentityContract.ThemisAPIWritePolicyName)]
    public async Task<ActionResult<CommonResult>> PostClaims(string id, [FromBody] IEnumerable<ClaimUpdateRequest> requests)
    {
        this.logger.LogInformation($"Create User Claims: {id}");
        var user = await this.userManager.FindByIdAsync(id);
        if (user is null)
            return this.NotFound(new CommonResult(new CommonResultError($"Can not find User with id: {id}")));

        var existingClaims = await this.userManager.GetClaimsAsync(user);
        var existingClaimSet = existingClaims.Select(claim => (claim.Type, claim.Value)).ToHashSet();
        var commonResult = new CommonResult();

        var claimsToAdd = requests
            .Where(claim => !existingClaimSet.Contains((claim.Type, claim.Value)))
            .Select(claim => new Claim(claim.Type, claim.Value))
            .ToArray();
        if (claimsToAdd.Any())
        {
            var result = await this.userManager.AddClaimsAsync(user, claimsToAdd);
            foreach (var error in result.Errors)
            {
                commonResult.Errors.Add(new CommonResultError(error.Code, error.Description));
            }
        }

        return commonResult.Succeeded ? this.Ok(commonResult) : this.BadRequest(commonResult);
    }

    /// <summary>
    /// Delete user claims
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requests"></param>
    /// <returns></returns>
    [HttpDelete("{id}/Claims")]
    [Authorize(Policy = IdentityContract.ThemisAPIWritePolicyName)]
    public async Task<ActionResult<CommonResult>> DeleteClaims(string id, [FromBody] IEnumerable<ClaimUpdateRequest> requests)
    {
        this.logger.LogInformation($"Delete User Claims: {id}");
        var user = await this.userManager.FindByIdAsync(id);
        if (user is null)
            return this.NotFound(new CommonResult(new CommonResultError($"Can not find User with id: {id}")));

        var existingClaims = await this.userManager.GetClaimsAsync(user);
        var existingClaimMap = existingClaims.ToDictionary(claim => (claim.Type, claim.Value), claim => claim);
        var commonResult = new CommonResult();

        var claimsToRemove = requests
            .Select(claim => existingClaimMap.TryGetValue((claim.Type, claim.Value), out var existingClaim) ? existingClaim : null)
            .OfType<Claim>()
            .ToArray();
        if (claimsToRemove.Any())
        {
            var result = await this.userManager.RemoveClaimsAsync(user, claimsToRemove);
            foreach (var error in result.Errors)
            {
                commonResult.Errors.Add(new CommonResultError(error.Code, error.Description));
            }
        }

        return commonResult.Succeeded ? this.Ok(commonResult) : this.BadRequest(commonResult);
    }
    #endregion

    #region UserRoles

    /// <summary>
    /// Get user roles
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}/Roles")]
    [Authorize(Policy = IdentityContract.ThemisAPIReadPolicyName)]
    public async Task<ActionResult<PaginatedEnumerable<RoleInformationResponse>>> GetRoles(int id, [FromQuery] RoleSearchRequest request)
    {
        this.logger.LogInformation($"Get Role Users: {id}");
        var result = await this.oceanusUserRepository.SearchRolesAsync(
            id, request.Id, request.RoleName, request.Start, request.Count);
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
    /// Update user roles
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requests"></param>
    /// <returns></returns>
    [HttpPut("{id}/Roles")]
    [Authorize(Policy = IdentityContract.ThemisAPIWritePolicyName)]
    public async Task<ActionResult<CommonResult>> PutRoles(string id, [FromBody] IEnumerable<string> requests)
    {
        this.logger.LogInformation($"Update User Roles: {id}");
        var user = await this.userManager.FindByIdAsync(id);
        if (user is null)
            return this.NotFound(new CommonResult(new CommonResultError($"Can not find User with id: {id}")));

        var commonResult = await this.UpdateUserRoles(user, requests);
        return commonResult.Succeeded ? this.Ok(commonResult) : this.BadRequest(commonResult);
    }

    private async Task<CommonResult> UpdateUserRoles(OceanusUser user, IEnumerable<string> requests)
    {
        var existingRoles = await this.userManager.GetRolesAsync(user);
        var existingRoleSet = existingRoles.ToHashSet();
        var requestRoleSet = requests.ToHashSet();
        var commonResult = new CommonResult();

        var rolesToRemove = existingRoleSet
            .Except(requestRoleSet)
            .Where(role => this.userManager.IsInRoleAsync(user, role).Result)
            .ToArray();
        if (rolesToRemove.Any())
        {
            var result = await this.userManager.RemoveFromRolesAsync(user, rolesToRemove);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    commonResult.Errors.Add(new CommonResultError(error.Code, error.Description));
                }
            }
        }

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
                foreach (var error in result.Errors)
                {
                    commonResult.Errors.Add(new CommonResultError(error.Code, error.Description));
                }
            }
        }
        return commonResult;
    }

    /// <summary>
    /// Add user roles
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requests"></param>
    /// <returns></returns>
    [HttpPost("{id}/Roles")]
    [Authorize(Policy = IdentityContract.ThemisAPIWritePolicyName)]
    public async Task<ActionResult<CommonResult>> PostRoles(string id, [FromBody] IEnumerable<string> requests)
    {
        this.logger.LogInformation($"Create User Roles: {id}");
        var user = await this.userManager.FindByIdAsync(id);
        if (user is null)
            return this.NotFound(new CommonResult(new CommonResultError($"Can not find User with id: {id}")));

        requests = requests
            .Where(role =>
                this.roleManager.RoleExistsAsync(role).Result &&
                !this.userManager.IsInRoleAsync(user, role).Result)
            .ToArray();

        var result = await this.userManager.AddToRolesAsync(user, requests);
        var commonResult = new CommonResult(
            result.Errors.Select(error => new CommonResultError(error.Code, error.Description)).ToList());
        return result.Succeeded ? this.Ok(commonResult) : this.BadRequest(commonResult);
    }

    /// <summary>
    /// Delete user roles
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requests"></param>
    /// <returns></returns>
    [HttpDelete("{id}/Roles")]
    [Authorize(Policy = IdentityContract.ThemisAPIWritePolicyName)]
    public async Task<ActionResult<CommonResult>> DeleteRoles(string id, [FromBody] IEnumerable<string> requests)
    {
        this.logger.LogInformation($"Delete User Roles: {id}");
        var user = await this.userManager.FindByIdAsync(id);
        if (user is null)
            return this.NotFound(new CommonResult(new CommonResultError($"Can not find User with id: {id}")));

        requests = requests
            .Where(role => this.userManager.IsInRoleAsync(user, role).Result)
            .ToArray();

        var result = await this.userManager.RemoveFromRolesAsync(user, requests);
        var commonResult = new CommonResult(
            result.Errors.Select(error => new CommonResultError(error.Code, error.Description)).ToList());
        return result.Succeeded ? this.Ok(commonResult) : this.BadRequest(commonResult);
    }
    #endregion
}
