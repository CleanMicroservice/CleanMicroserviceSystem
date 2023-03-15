using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanMicroserviceSystem.Astra.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PackageController : ControllerBase
{
    private readonly ILogger<PackageController> logger;

    public PackageController(
        ILogger<PackageController> logger)
    {
        this.logger = logger;
    }
}
