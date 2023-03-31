using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace CleanMicroserviceSystem.Tethys.WebAPI.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> logger;

    public HomeController(
        ILogger<HomeController> logger)
    {
        this.logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var builder = new StringBuilder();
        builder.AppendLine($"Response from WebAPI.");
        builder.AppendLine(this.ControllerContext.ActionDescriptor.DisplayName);
        builder.AppendLine(this.HttpContext.TraceIdentifier);
        builder.AppendLine($"{this.HttpContext.Connection.RemoteIpAddress?.ToString()}:{this.HttpContext.Connection.RemotePort}");
        builder.AppendLine($"{this.HttpContext.Request.Method} {this.HttpContext.Request.Protocol}");
        builder.AppendLine(this.HttpContext.Request.Path.ToUriComponent());
        builder.AppendLine(this.HttpContext.Request.QueryString.ToUriComponent());
        var log = builder.ToString();
        return this.Ok(log);
    }
}
