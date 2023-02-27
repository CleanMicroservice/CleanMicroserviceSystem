using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CleanMicroserviceSystem.Authentication.Domain;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Aphrodite.Infrastructure.Services.Authentication
{
    public class AphroditeAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILogger<AphroditeAuthenticationStateProvider> logger;
        private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler;
        private readonly AphroditeAuthenticationTokenStore authenticationTokenStore;
        private readonly AphroditeAuthenticationClaimsIdentityValidator claimsIdentityValidator;
        public readonly static AuthenticationState AnonymousState = new(
            new ClaimsPrincipal(new[] { new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "Anonymous") }) }));
        private AuthenticationState? authenticationState = default;

        public AphroditeAuthenticationStateProvider(
            ILogger<AphroditeAuthenticationStateProvider> logger,
            JwtSecurityTokenHandler jwtSecurityTokenHandler,
            AphroditeAuthenticationTokenStore authenticationTokenStore,
            AphroditeAuthenticationClaimsIdentityValidator claimsIdentityValidator)
        {
            this.logger = logger;
            this.jwtSecurityTokenHandler = jwtSecurityTokenHandler;
            this.authenticationTokenStore = authenticationTokenStore;
            this.claimsIdentityValidator = claimsIdentityValidator;
            this.authenticationTokenStore.TokenUpdated += AuthenticationTokenStoreTokenUpdated;
        }

        private async void AuthenticationTokenStoreTokenUpdated(object? sender, string token)
        {
            logger.LogInformation($"Authentication token updated...");
            await this.ApplyTokenAsync(token);
            this.NotifyAuthenticationStateChanged(this.GetAuthenticationStateAsync());
        }

        private async Task ApplyTokenAsync(string token)
        {
            logger.LogInformation($"Apply token...");
            if (string.IsNullOrWhiteSpace(token))
            {
                authenticationState = AnonymousState;
            }
            else
            {
                var jwtSecurityToken = this.jwtSecurityTokenHandler.ReadJwtToken(token);
                if (!(jwtSecurityToken?.Claims?.Any() ?? false))
                {
                    authenticationState = AnonymousState;
                }

                authenticationState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(jwtSecurityToken!.Claims, IdentityContract.JwtAuthenticationType)));
            }
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            logger.LogInformation($"Get authentication state...");

            if (this.authenticationState is null)
            {
                var existedToken = await this.authenticationTokenStore.GetTokenAsync();
                await this.ApplyTokenAsync(existedToken);
            }

            var validated = await this.claimsIdentityValidator.ValidateAsync(authenticationState!.User.Identity as ClaimsIdentity);
            if (!validated)
            {
                return AnonymousState;
            }
            return authenticationState;
        }
    }
}
