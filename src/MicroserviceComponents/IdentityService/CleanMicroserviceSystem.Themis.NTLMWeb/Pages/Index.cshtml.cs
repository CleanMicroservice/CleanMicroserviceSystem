using System.Security.Claims;
using System.Text.RegularExpressions;
using CleanMicroserviceSystem.Authentication.Application;
using CleanMicroserviceSystem.DataStructure;
using CleanMicroserviceSystem.Themis.Client;
using CleanMicroserviceSystem.Themis.Contract.Claims;
using CleanMicroserviceSystem.Themis.Contract.Roles;
using CleanMicroserviceSystem.Themis.Contract.Users;
using CleanMicroserviceSystem.Themis.NTLMWeb.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CleanMicroserviceSystem.Themis.NTLMWeb.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> logger;
    private readonly IAuthenticationTokenStore authenticationTokenStore;
    private readonly ThemisClientTokenClient clientTokenClient;
    private readonly ThemisUserClient userClient;

    public string? ReturnUrl { get; protected set; }
    public bool Authenticated { get; protected set; }
    public CommonResult<string>? ClientLoginResult { get; protected set; }
    public CommonResult<UserInformationResponse>? UserSynchronizeResult { get; protected set; }
    public string TemporaryPassword { get; protected set; }

    public IndexModel(
        ILogger<IndexModel> logger,
        IAuthenticationTokenStore authenticationTokenStore,
        ThemisUserClient userClient,
        ThemisClientTokenClient clientTokenClient)
    {
        this.logger = logger;
        this.authenticationTokenStore = authenticationTokenStore;
        this.userClient = userClient;
        this.clientTokenClient = clientTokenClient;
    }

    public void OnGet([FromQuery] string returnUrl)
    {
        this.logger.LogInformation($"User [{this.User.Identity?.Name}] accessed, {nameof(this.User.Identity.IsAuthenticated)}={this.User.Identity?.IsAuthenticated}, with {this.User.Claims.Count()} claims, from: {returnUrl}.");
        this.Authenticated = this.User.Identity?.IsAuthenticated ?? false;
        this.ReturnUrl = returnUrl;

        if (!this.Authenticated)
        {
            return;
        }

        try
        {
            this.logger.LogInformation("Client logging in ...");
            this.ClientLoginResult = this.clientTokenClient.LoginClientAsync(ApiContract.ThemisNTLMClientName, "ThemisNTLMSecret").Result;
            this.authenticationTokenStore.UpdateTokenAsync(this.ClientLoginResult?.Entity ?? string.Empty).Wait();
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
        try
        {
            this.logger.LogInformation("User synchronizing ...");
            this.TemporaryPassword = Guid.NewGuid().ToString("n");
            var userName = Regex.Replace(this.User.Identity?.Name ?? string.Empty, "[^0-9A-Za-z]", string.Empty);
            var request = new UserSynchronizeRequest()
            {
                UserName = userName,
                Email = $"{userName}@ICANN.COM",
                PhoneNumber = "1234567890",
                SynchronizeSource = ApiContract.ThemisNTLMClientName,
                Password = this.TemporaryPassword,
                ConfirmPassword = this.TemporaryPassword,
                Roles = new List<RoleCreateRequest>(),
                Claims = new List<ClaimUpdateRequest>()
            };
            foreach (var claim in this.User.Claims)
            {
                if (claim.Type == ClaimTypes.PrimaryGroupSid ||
                    claim.Type == ClaimTypes.GroupSid)
                {
                    request.Roles.Add(new RoleCreateRequest() { RoleName = claim.Value });
                }
                else
                {
                    request.Claims.Add(new ClaimUpdateRequest() { Type = claim.Type, Value = claim.Value });
                }
            }
            this.UserSynchronizeResult = this.userClient.SynchronizeUserAsync(request).Result;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Failed to synchronize user:");
            this.UserSynchronizeResult = new CommonResult<UserInformationResponse>(new List<CommonResultError>() { new CommonResultError() {
                    Code = "Failed",
                    Message = ex.Message
                }});
            return;
        }
    }
}
