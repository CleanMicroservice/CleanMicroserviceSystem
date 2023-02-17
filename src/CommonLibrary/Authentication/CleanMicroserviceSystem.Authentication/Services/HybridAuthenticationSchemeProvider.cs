using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace CleanMicroserviceSystem.Authentication.Services
{
    public class HybridAuthenticationSchemeProvider : AuthenticationSchemeProvider
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public HybridAuthenticationSchemeProvider(
            IOptions<AuthenticationOptions> options,
            IHttpContextAccessor httpContextAccessor) :
            base(options)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public override Task<AuthenticationScheme?> GetDefaultAuthenticateSchemeAsync()
        {
            /* TODO: Refactor to make configurable
            var headers = this.httpContextAccessor.HttpContext?.Request?.Headers;
            if (headers?.TryGetValue(grantTypeKey, out var value) ?? false)
            {
                if (value.Contains(clientCredentials))
                {
                    return this.GetSchemeAsync("Bearer_Api");
                }
            }
             */
            return base.GetDefaultAuthenticateSchemeAsync();
        }
    }
}
