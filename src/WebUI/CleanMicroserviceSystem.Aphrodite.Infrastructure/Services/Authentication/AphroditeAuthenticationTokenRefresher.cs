using CleanMicroserviceSystem.Aphrodite.Application.Configurations;
using CleanMicroserviceSystem.Authentication.Application;
using CleanMicroserviceSystem.Themis.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CleanMicroserviceSystem.Aphrodite.Infrastructure.Services.Authentication;

public class AphroditeAuthenticationTokenRefresher : IAuthenticationTokenRefresher
{
    private readonly ILogger<AphroditeAuthenticationTokenRefresher> logger;
    private readonly IOptionsMonitor<GatewayAPIConfiguration> gatewayOptionsMonitor;
    private readonly ThemisUserTokenClient themisUserTokenClient;
    private readonly IAuthenticationTokenStore authenticationTokenStore;
    private readonly Timer refreshTimer;

    public bool IsRunning { get; private set; }

    public AphroditeAuthenticationTokenRefresher(
        ILogger<AphroditeAuthenticationTokenRefresher> logger,
        IOptionsMonitor<GatewayAPIConfiguration> gatewayOptionsMonitor,
        ThemisUserTokenClient themisUserTokenClient,
        IAuthenticationTokenStore authenticationTokenStore)
    {
        this.logger = logger;
        this.gatewayOptionsMonitor = gatewayOptionsMonitor;
        this.themisUserTokenClient = themisUserTokenClient;
        this.authenticationTokenStore = authenticationTokenStore;

        this.refreshTimer = new Timer(new TimerCallback(this.RefreshTokenCallBack), null, Timeout.Infinite, int.MaxValue);
        gatewayOptionsMonitor.OnChange((configuration, name) => this.UpdateRefresher());
    }

    private void UpdateRefresher()
    {
        var dueTime = this.IsRunning ? 5 * 1000 : Timeout.Infinite;
        this.refreshTimer.Change(dueTime, gatewayOptionsMonitor.CurrentValue.TokenRefreshInMinutes * 1000 * 60);
    }

    public void StartRefresher()
    {
        if (this.IsRunning) return;
        this.logger.LogInformation($"Start token refresher ...");
        this.IsRunning = true;
        this.UpdateRefresher();
    }

    public void StopRefresher()
    {
        if (!this.IsRunning) return;
        this.logger.LogInformation($"Stop token refresher ...");
        this.IsRunning = false;
        this.refreshTimer.Change(Timeout.Infinite, int.MaxValue);
    }

    protected void RefreshTokenCallBack(object? state)
    {
        _ = this.RefreshTokenAsync().ConfigureAwait(false);
    }

    public virtual async Task RefreshTokenAsync()
    {
        this.logger.LogInformation($"Refresh Token ...");
        var response = await themisUserTokenClient.RefreshUserTokenAsync();
        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            this.logger.LogWarning($"Failed to refresh Token: ({(int)response.StatusCode}) {response.StatusCode} => {content}");
        }
        else
        {
            this.logger.LogInformation($"Refresh and update Token successfully.");
            await this.authenticationTokenStore.UpdateTokenAsync(content);
        }
    }
}
