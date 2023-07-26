using CleanMicroserviceSystem.DataStructure;
using CleanMicroserviceSystem.Themis.Client;
using CleanMicroserviceSystem.Themis.NTLMWeb.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace CleanMicroserviceSystem.Themis.NTLMWeb.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> logger;
    private readonly ThemisClientTokenClient clientTokenClient;
    private readonly ThemisUserClient userClient;

    public string? ReturnUrl { get; protected set; }
    public bool Authenticated { get; protected set; }
    public CommonResult<string>? ClientLoginResult { get; protected set; }

    public IndexModel(
        ILogger<IndexModel> logger,
        IOptions<GatewayAPIConfiguration> gatewayApiOptions,
        ThemisUserClient userClient,
        ThemisClientTokenClient clientTokenClient)
    {
        this.logger = logger;
        this.userClient = userClient;
        this.clientTokenClient = clientTokenClient;
    }

    public void OnGet([FromQuery] string returnUrl)
    {
        this.logger.LogInformation($"User [{this.User.Identity?.Name}] accessed, {nameof(this.User.Identity.IsAuthenticated)}={this.User.Identity?.IsAuthenticated}, with {this.User.Claims.Count()} claims, from: {returnUrl}.");
        this.Authenticated = this.User.Identity?.IsAuthenticated ?? false;
        this.ReturnUrl = returnUrl;

        if (this.Authenticated)
        {
            try
            {
                this.logger.LogInformation("Client logging in ...");
                this.ClientLoginResult = this.clientTokenClient.LoginClientAsync(ApiContract.ThemisNTLMClientName, "ThemisNTLMSecret").Result;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Failed to login client:");
                this.ClientLoginResult = new CommonResult<string>(new List<CommonResultError>() { new CommonResultError() {
                Code = "Failed",
                Message = ex.Message
            }});
                return;
            }
        }
    }
}
