using System.Security.Claims;
using CleanMicroserviceSystem.Authentication.Services;
using CleanMicroserviceSystem.Themis.Domain.DTOs.Users;
using CleanMicroserviceSystem.Themis.Domain.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CleanMicroserviceSystem.Themis.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserTokenController : ControllerBase
{
    private readonly ILogger<UserTokenController> logger;
    private readonly IJwtBearerTokenGenerator jwtBearerTokenGenerator;
    private readonly UserManager<OceanusUser> userManager;
    private readonly RoleManager<OceanusRole> roleManager;
    private readonly SignInManager<OceanusUser> signInManager;

    public UserTokenController(
        ILogger<UserTokenController> logger,
        IJwtBearerTokenGenerator jwtBearerTokenGenerator,
        UserManager<OceanusUser> userManager,
        RoleManager<OceanusRole> roleManager,
        SignInManager<OceanusUser> signInManager)
    {
        this.logger = logger;
        this.jwtBearerTokenGenerator = jwtBearerTokenGenerator;
        this.userManager = userManager;
        this.roleManager = roleManager;
        this.signInManager = signInManager;
    }

    private async Task<IEnumerable<Claim>> GetClaimsAsync(OceanusUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
        };

        if (!string.IsNullOrEmpty(user.Email))
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
        if (!string.IsNullOrEmpty(user.PhoneNumber))
            claims.Add(new Claim(ClaimTypes.HomePhone, user.PhoneNumber));

        var userClaims = await this.userManager.GetClaimsAsync(user);
        claims.AddRange(userClaims);

        var roleNames = await this.userManager.GetRolesAsync(user);
        claims.AddRange(roleNames.Select(role => new Claim(ClaimTypes.Role, role)));
        var roleClaims = roleNames
            .Select(roleName => this.roleManager.FindByNameAsync(roleName).Result)
            .OfType<OceanusRole>()
            .SelectMany(role => this.roleManager.GetClaimsAsync(role).Result)
            .DistinctBy(claim => (claim.Type, claim.Value));
        claims.AddRange(roleClaims);

        return claims;
    }

    /// <summary>
    /// Login user
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Post([FromBody] UserTokenLoginRequest request)
    {
        var result = await this.signInManager.PasswordSignInAsync(request.UserName, request.Password, true, false);
        if (!result.Succeeded)
        {
            var errorMessage = result switch
            {
                { } when result.IsLockedOut => "Account locked out",
                { } when result.IsNotAllowed => "Account not allowed",
                { } when result.RequiresTwoFactor => "Account requires two factor",
                _ => "Invalid user information"
            };

            return this.BadRequest(errorMessage);
        }

        var user = await this.userManager.FindByNameAsync(request.UserName);
        var claims = await this.GetClaimsAsync(user!);
        var token = this.jwtBearerTokenGenerator.GenerateUserSecurityToken(claims);
        return this.Ok(token);
    }

    /// <summary>
    /// Refresh user token
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> Put()
    {
        var userName = this.HttpContext.User?.Identity?.Name;
        if (string.IsNullOrEmpty(userName))
            return this.BadRequest(new ArgumentException());

        var user = await this.userManager.FindByNameAsync(userName);
        var claims = await this.GetClaimsAsync(user!);
        var token = this.jwtBearerTokenGenerator.GenerateUserSecurityToken(claims);
        return this.Ok(token);
    }

    /// <summary>
    /// Logout user
    /// </summary>
    [HttpDelete]
    public async Task<IActionResult> Delete()
    {
        await this.signInManager.SignOutAsync();
        return this.Ok();
    }
}
