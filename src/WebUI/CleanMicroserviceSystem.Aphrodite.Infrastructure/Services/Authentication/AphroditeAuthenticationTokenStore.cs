using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Aphrodite.Infrastructure.Services.Authentication
{
    public class AphroditeAuthenticationTokenStore
    {
        private const string AuthenticationTokenCookieName = "AphroditeAuthenticationToken";
        private readonly ILogger<AphroditeAuthenticationTokenStore> logger;
        private readonly CookieStorage cookieStorage;
        private string token;
        public event EventHandler<string> TokenUpdated;

        public AphroditeAuthenticationTokenStore(
            ILogger<AphroditeAuthenticationTokenStore> logger,
            CookieStorage cookieStorage)
        {
            this.logger = logger;
            this.cookieStorage = cookieStorage;
        }

        public async ValueTask<string> GetTokenAsync()
        {
            logger.LogInformation("Get Token ...");
            if (string.IsNullOrWhiteSpace(this.token))
            {
                this.token = await cookieStorage.GetCookieAsync(AuthenticationTokenCookieName);
            }
            return this.token;
        }

        public async Task UpdateTokenAsync(string token)
        {
            logger.LogInformation("Update Token ...");
            this.token = token;
            await cookieStorage.SetItemAsync(AuthenticationTokenCookieName, token);
            TokenUpdated?.Invoke(this, token);
        }
    }
}
