﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CleanMicroserviceSystem.Authentication.Application;
using CleanMicroserviceSystem.Authentication.Domain;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Aphrodite.Infrastructure.Services.Authentication;

public class AphroditeAuthenticationStateProvider : AuthenticationStateProvider
{
    public static readonly AuthenticationState AnonymousState = new(
        new ClaimsPrincipal(new[] { new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "Anonymous") }) }));

    private readonly ILogger<AphroditeAuthenticationStateProvider> logger;
    private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler;
    private readonly IAuthenticationTokenRefresher tokenRefresher;
    private readonly IAuthenticationTokenStore authenticationTokenStore;
    private readonly AphroditeJwtSecurityTokenValidator jwtSecurityTokenValidator;
    private AuthenticationState? authenticationState = default;
    private JwtSecurityToken? jwtSecurityToken = default;

    public AphroditeAuthenticationStateProvider(
        ILogger<AphroditeAuthenticationStateProvider> logger,
        JwtSecurityTokenHandler jwtSecurityTokenHandler,
        IAuthenticationTokenRefresher tokenRefresher,
        IAuthenticationTokenStore authenticationTokenStore,
        AphroditeJwtSecurityTokenValidator jwtSecurityTokenValidator)
    {
        this.logger = logger;
        this.jwtSecurityTokenHandler = jwtSecurityTokenHandler;
        this.tokenRefresher = tokenRefresher;
        this.authenticationTokenStore = authenticationTokenStore;
        this.jwtSecurityTokenValidator = jwtSecurityTokenValidator;
        this.authenticationTokenStore.TokenUpdated += this.AuthenticationTokenStoreTokenUpdated;
    }

    private async void AuthenticationTokenStoreTokenUpdated(object? sender, string token)
    {
        this.logger.LogInformation($"Authentication token updated...");
        await this.ApplyTokenAsync(token);
        this.NotifyAuthenticationStateChanged(this.GetAuthenticationStateAsync());
    }

    private async Task ApplyTokenAsync(string token)
    {
        this.logger.LogInformation($"Apply token...");

        try
        {
            this.jwtSecurityToken = this.jwtSecurityTokenHandler.ReadJwtToken(token);
            this.authenticationState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(
                this.jwtSecurityToken!.Claims,
                IdentityContract.JwtAuthenticationType)));
            if (!this.tokenRefresher.IsRunning)
                this.tokenRefresher.StartRefresher();
        }
        catch
        {
            this.ClearState();
        }
        await Task.CompletedTask;
    }

    private void ClearState()
    {
        if (this.tokenRefresher.IsRunning)
            this.tokenRefresher.StopRefresher();
        this.jwtSecurityToken = default;
        this.authenticationState = AnonymousState;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        this.logger.LogInformation($"Get authentication state...");

        if (this.authenticationState is null)
        {
            var existedToken = await this.authenticationTokenStore.GetTokenAsync();
            await this.ApplyTokenAsync(existedToken);
        }

        var validated = await this.jwtSecurityTokenValidator.ValidateAsync(this.jwtSecurityToken);
        if (!validated)
        {
            this.ClearState();
        }
        return this.authenticationState!;
    }
}
