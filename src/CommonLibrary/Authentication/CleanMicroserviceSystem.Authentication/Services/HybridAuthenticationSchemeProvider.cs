using CleanMicroserviceSystem.Authentication.Configurations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace CleanMicroserviceSystem.Authentication.Services
{
    public class HybridAuthenticationSchemeProvider : AuthenticationSchemeProvider
    {
        private readonly IEnumerable<AuthenticationSchemeConfiguration> schemeConfigurations;
        private readonly IHttpContextAccessor httpContextAccessor;

        public HybridAuthenticationSchemeProvider(
            IOptions<AuthenticationOptions> options,
            IEnumerable<AuthenticationSchemeConfiguration> schemeConfigurations,
            IHttpContextAccessor httpContextAccessor) :
            base(options)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.schemeConfigurations = schemeConfigurations;
        }

        public override async Task<AuthenticationScheme?> GetDefaultAuthenticateSchemeAsync()
        {
            var httpContext = this.httpContextAccessor.HttpContext;
            if (httpContext is null)
                return await base.GetDefaultAuthenticateSchemeAsync();

            var primarySchemeConfig = this.schemeConfigurations.FirstOrDefault(scheme => scheme.Predicate.Invoke(httpContext));
            if (primarySchemeConfig is null)
                return await base.GetDefaultAuthenticateSchemeAsync();

            var result = await this.GetSchemeAsync(primarySchemeConfig.SchemeName);
            return result;
        }
    }
}
