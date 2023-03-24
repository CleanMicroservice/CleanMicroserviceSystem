using System.Security.Claims;
using CleanMicroserviceSystem.Authentication.Application;
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
    public async Task<ActionResult<string>> Post([FromBody] ClientTokenLoginRequest request)
    {
        this.logger.LogInformation($"Sign in client: {request.Name}");
        var result = await this.clientManager.SignInAsync(request.Name, request.Secret);
        if (!result.Succeeded)
        {
            return this.BadRequest(result);
        }
        var client = result.Entity!;
        var claims = await this.GetClaimsAsync(client);
        var token = this.jwtBearerTokenGenerator.GenerateClientSecurityToken(claims);
        return this.Ok(token);
    }

    /// <summary>
    /// Refresh client token
    /// </summary>
    [HttpPut]
    public async Task<ActionResult<string>> Put()
    {
        var clientName = this.HttpContext.User?.Identity?.Name;
        this.logger.LogInformation($"Refresh Client token: {clientName}");
        if (string.IsNullOrEmpty(clientName))
            return this.BadRequest();

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
        var clientName = this.HttpContext.User?.Identity?.Name;
        this.logger.LogInformation($"Sign out Client: {clientName}");
        if (string.IsNullOrEmpty(clientName))
            return this.BadRequest();
        await this.clientManager.SignOutAsync(clientName);
        return this.Ok();
    }
}
