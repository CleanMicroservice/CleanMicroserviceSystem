using System.Security.Claims;
using CleanMicroserviceSystem.Authentication.Services;
using CleanMicroserviceSystem.Themis.Application.Services;
using CleanMicroserviceSystem.Themis.Contract.Clients;
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
        var clientClaims = await this.clientManager.GetClaimsAsync(client.Id);
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, client.Name),
            new Claim(nameof(DateTime.UtcNow), DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.ffffff"))
        };
        claims.AddRange(clientClaims?.Select(claim => new Claim(claim.ClaimType, claim.ClaimValue))?.ToArray() ?? Enumerable.Empty<Claim>());
        var token = this.jwtBearerTokenGenerator.GenerateClientSecurityToken(claims);
        return this.Ok(token);
    }
}
