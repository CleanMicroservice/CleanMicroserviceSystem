using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Aphrodite.Infrastructure.Services.Authentication
{
    public class AphroditeAuthenticationTokenStore
    {
        private const string AuthenticationTokenCookieName = "AphroditeAuthenticationToken";
        private readonly ILogger<AphroditeAuthenticationTokenStore> logger;
        private readonly CookieStorage cookieStorage;
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
            return await cookieStorage.GetCookieAsync(AuthenticationTokenCookieName);
        }

        public async Task UpdateTokenAsync(string token)
        {
            logger.LogInformation("Update Token ...");
            await cookieStorage.SetItemAsync(AuthenticationTokenCookieName, token);
            TokenUpdated?.Invoke(this, token);
        }
    }
}
