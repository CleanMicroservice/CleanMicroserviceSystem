using CleanMicroserviceSystem.Themis.Client;
using CleanMicroserviceSystem.Themis.NTLMWeb.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace CleanMicroserviceSystem.Themis.NTLMWeb.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> logger;
    private readonly IOptions<GatewayAPIConfiguration> gatewayApiOptions;
    private readonly ThemisClientTokenClient clientTokenClient;
    private readonly ThemisUserClient userClient;

    public string? ReturnUrl { get; protected set; }
    public bool Authenticated { get; protected set; }

    public IndexModel(
        ILogger<IndexModel> logger,
        IOptions<GatewayAPIConfiguration> gatewayApiOptions,
        ThemisUserClient userClient,
        ThemisClientTokenClient clientTokenClient)
    {
        this.logger = logger;
        this.gatewayApiOptions = gatewayApiOptions;
        this.userClient = userClient;
        this.clientTokenClient = clientTokenClient;
    }

    public void OnGet([FromQuery] string returnUrl)
    {
        // this.clientTokenClient.LoginClientAsync();
        this.logger.LogInformation($"User [{this.User.Identity?.Name}] accessed, with {this.User.Claims.Count()} claims, from: {returnUrl}.");
        this.Authenticated = this.User.Identity?.IsAuthenticated ?? false;
        this.ReturnUrl = returnUrl;
    }
}
