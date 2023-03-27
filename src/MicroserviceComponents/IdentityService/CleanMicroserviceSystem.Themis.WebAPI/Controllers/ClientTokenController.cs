using System.Security.Claims;
using Castle.Components.DictionaryAdapter.Xml;
using CleanMicroserviceSystem.Authentication.Application;
using CleanMicroserviceSystem.DataStructure;
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
    public async Task<ActionResult<CommonResult>> Post([FromBody] ClientTokenLoginRequest request)
    {
        this.logger.LogInformation($"Sign in client: {request.Name}");
        var result = await this.clientManager.SignInAsync(request.Name, request.Secret);
        var commonResult = new CommonResult<string>(result.Errors);
        if (result.Succeeded)
        {
            var client = result.Entity!;
            var claims = await this.GetClaimsAsync(client);
            var token = this.jwtBearerTokenGenerator.GenerateClientSecurityToken(claims);
            commonResult.Entity = token;
        }
        return commonResult.Succeeded ? this.Ok(commonResult) : this.BadRequest(commonResult);
    }

    /// <summary>
    /// Refresh client token
    /// </summary>
    [HttpPut]
    public async Task<ActionResult<CommonResult>> Put()
    {
        var clientName = this.HttpContext.User?.Identity?.Name;
        this.logger.LogInformation($"Refresh Client token: {clientName}");
        var client = await this.clientManager.FindByNameAsync(clientName!);
        var claims = await this.GetClaimsAsync(client!);
        var token = this.jwtBearerTokenGenerator.GenerateClientSecurityToken(claims);
        return this.Ok(new CommonResult<string>(token));
    }

    /// <summary>
    /// Logout client
    /// </summary>
    [HttpDelete]
    public async Task<ActionResult> Delete()
    {
        var clientName = this.HttpContext.User?.Identity?.Name;
        this.logger.LogInformation($"Sign out Client: {clientName}");
        await this.clientManager.SignOutAsync(clientName!);
        return this.Ok(CommonResult.Success);
    }
}
