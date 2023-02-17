using System.Security.Claims;
using CleanMicroserviceSystem.Authentication.Services;
using CleanMicroserviceSystem.Themis.Application.DataTransferObjects.Clients;
using CleanMicroserviceSystem.Themis.Application.Services;
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
        if (!result.Success)
        {
            return this.BadRequest(result.Error);
        }
        var client = result.Client!;
        var clientScopes = await this.clientManager.GetClientApiScopes(client.ID);
        var claims = new List<Claim>() { new Claim(ClaimTypes.Name, client.Name) };
        claims.AddRange(clientScopes?.Select(scope => new Claim(scope.Name, "true"))?.ToArray() ?? Enumerable.Empty<Claim>());
        var token = this.jwtBearerTokenGenerator.GenerateSecurityToken(claims);
        return this.Ok(token);
    }
}
