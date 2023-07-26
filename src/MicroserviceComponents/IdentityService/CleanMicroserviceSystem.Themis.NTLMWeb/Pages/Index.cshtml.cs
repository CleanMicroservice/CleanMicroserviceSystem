using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CleanMicroserviceSystem.Themis.NTLMWeb.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    public string? ReturnUrl { get; protected set; }
    public bool Authenticated { get; protected set; }

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet([FromQuery] string returnUrl)
    {
        this.Authenticated = this.User.Identity?.IsAuthenticated ?? false;
        this.ReturnUrl = returnUrl;
    }
}
