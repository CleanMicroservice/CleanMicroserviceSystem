using System.Security.Claims;
using CleanMicroserviceSystem.Authentication.Services;
using CleanMicroserviceSystem.Themis.Application.Services;
using CleanMicroserviceSystem.Themis.Contract.Clients;
using CleanMicroserviceSystem.Themis.Domain.Entities.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanMicroserviceSystem.Themis.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ClientTokenController : ControllerBase
{
    private readonly ILogger<ClientTokenController> logger;
    private readonly IJwtBearerTokenGenerator jwtBearerTokenGenerator;
    private readonly IClientManager clientManager;

    public ClientTokenController(
        ILogger<ClientTokenController> logger,
        IJwtBearerTokenGenerator jwtBearerTokenGenerator,
        IClientManager clientManager)
    {
        this.logger = logger;
        this.jwtBearerTokenGenerator = jwtBearerTokenGenerator;
        this.clientManager = clientManager;
    }

    private async Task<IEnumerable<Claim>> GetClaimsAsync(Client client)
    {
        var clientClaims = await this.clientManager.GetClaimsAsync(client.Id);
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, client.Id.ToString()),
            new Claim(ClaimTypes.Name, client.Name)
        };
        claims.AddRange(clientClaims?.Select(claim => new Claim(claim.ClaimType, claim.ClaimValue))?.ToArray() ?? Enumerable.Empty<Claim>());

        return claims;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Post([FromBody] ClientTokenLoginRequest request)
    {
        var result = await this.clientManager.SignInAsync(request.Name, request.Secret);
        if (!result.Succeeded)
        {
            return this.BadRequest(result.Error);
        }
        var client = result.Client!;
        var claims = await this.GetClaimsAsync(client);
        var token = this.jwtBearerTokenGenerator.GenerateClientSecurityToken(claims);
        return this.Ok(token);
    }

    /// <summary>
    /// Refresh client token
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> Put()
    {
        var clientName = this.HttpContext.User?.Identity?.Name;
        if (string.IsNullOrEmpty(clientName))
            return this.BadRequest(new ArgumentException());

        var client = await this.clientManager.FindByNameAsync(clientName);
        var claims = await this.GetClaimsAsync(client!);
        var token = this.jwtBearerTokenGenerator.GenerateClientSecurityToken(claims);
        return this.Ok(token);
    }

    /// <summary>
    /// Logout client
    /// </summary>
    [HttpDelete]
    public async Task<IActionResult> Delete()
    {
        await this.clientManager.SignOutAsync();
        return this.Ok();
    }
}
