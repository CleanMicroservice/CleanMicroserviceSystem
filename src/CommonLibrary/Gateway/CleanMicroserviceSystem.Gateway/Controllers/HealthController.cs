using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Gateway.Controllers;

[AllowAnonymous]
[ApiController]
[Route("[controller]/[action]")]
public class HealthController : ControllerBase
{
    private readonly ILogger<HealthController> logger;

    public HealthController(
        ILogger<HealthController> logger)
    {
        this.logger = logger;
    }

    /// <summary>
    /// Health check
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult HealthCheck()
    {
        return this.Ok($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] I'm fine, thanks! And you?");
    }
}
