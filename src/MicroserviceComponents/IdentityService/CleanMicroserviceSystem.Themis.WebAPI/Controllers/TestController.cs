using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanMicroserviceSystem.Themis.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        // [Authorize(policy: "TestApiScope")]
        public async Task<IActionResult> TestApiScope()
        {
            return this.Ok("TestApiScope Great!");
        }
    }
}
