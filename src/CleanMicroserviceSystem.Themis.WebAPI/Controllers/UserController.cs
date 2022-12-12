using CleanMicroserviceSystem.Common.Contracts;
using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Identity;
using CleanMicroserviceSystem.Themis.Application.DataTransferObjects.Users;
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

    public UserController(
        ILogger<UserController> logger,
        UserManager<OceanusUser> userManager,
        RoleManager<OceanusRole> roleManager)
    {
        this.logger = logger;
        this.userManager = userManager;
        this.roleManager = roleManager;
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
            return this.Ok(user);
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
            ? this.NotFound("Can not find user by user name.")
            : this.Ok(new UserInformationResponse()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            });
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
            return this.Ok(newUser);
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
        var user = await this.userManager.FindByIdAsync(id.ToString());
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
            return this.Ok(user);
        }
    }

    /// <summary>
    /// Delete user information
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [Authorize(Policy = IdentityContract.AccessUsersPolicy)]
    public async Task<bool> Delete(int id)
    {
        var user = await this.userManager.FindByIdAsync(id.ToString());
        await this.userManager.DeleteAsync(user);
        return true;
    }
    #endregion

    #region UserClaims

    #endregion

    #region UserRoles

    #endregion
}
