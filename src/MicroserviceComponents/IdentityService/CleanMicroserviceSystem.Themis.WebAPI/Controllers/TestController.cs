using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanMicroserviceSystem.Themis.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{

    public TestController()
    {
    }

    [HttpGet]
    [Authorize(Policy = "TestPolicy")]
    public async Task<IActionResult> Get()
    {
        return this.Ok(DateTime.Now.ToString());
    }
}
