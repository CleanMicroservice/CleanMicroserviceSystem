using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Aphrodite.Infrastructure.Services.Authentication
{
    public class AphroditeAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILogger<AphroditeAuthenticationStateProvider> logger;
        private readonly AphroditeJsonWebTokenParser tokenParser;
        private readonly AphroditeAuthenticationTokenStore authenticationTokenStore;
        private readonly AphroditeAuthenticationClaimsIdentityValidator claimsIdentityValidator;
        public readonly static AuthenticationState AnonymousState = new(
            new ClaimsPrincipal(new[] { new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "Anonymous") }) }));
        private AuthenticationState? authenticationState = default;

        public AphroditeAuthenticationStateProvider(
            ILogger<AphroditeAuthenticationStateProvider> logger,
            AphroditeJsonWebTokenParser tokenParser,
            AphroditeAuthenticationTokenStore authenticationTokenStore,
            AphroditeAuthenticationClaimsIdentityValidator claimsIdentityValidator)
        {
            this.logger = logger;
            this.tokenParser = tokenParser;
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
                var updatedClaims = tokenParser.ParseJWTToken(token);
                if (updatedClaims is null || !updatedClaims.Any())
                {
                    authenticationState = AnonymousState;
                }
                authenticationState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(updatedClaims)));
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

            var validated = await claimsIdentityValidator.ValidateAsync(authenticationState!.User.Identity as ClaimsIdentity);
            if (!validated)
            {
                return AnonymousState;
            }
            return authenticationState;
        }
    }
}
