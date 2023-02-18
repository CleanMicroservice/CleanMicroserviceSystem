using CleanMicroserviceSystem.Authentication.Configurations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace CleanMicroserviceSystem.Authentication.Services
{
    public class HybridAuthenticationSchemeProvider : AuthenticationSchemeProvider
    {
        private readonly AuthenticationSchemeConfigurations schemeConfigurations;
        private readonly IHttpContextAccessor httpContextAccessor;

        public HybridAuthenticationSchemeProvider(
            IOptions<AuthenticationOptions> options,
            IConfigureOptions<AuthenticationSchemeConfigurations> schemeConfigurations,
            IHttpContextAccessor httpContextAccessor) :
            base(options)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.schemeConfigurations = new AuthenticationSchemeConfigurations();
            schemeConfigurations.Configure(this.schemeConfigurations);
        }

        public override async Task<AuthenticationScheme?> GetDefaultAuthenticateSchemeAsync()
        {
            var httpContext = this.httpContextAccessor.HttpContext;
            if (httpContext is null)
                return await base.GetDefaultAuthenticateSchemeAsync();

            var primarySchemeConfig = this.schemeConfigurations.SchemeConfigurations.FirstOrDefault(scheme => scheme.Predicate.Invoke(httpContext));
            if (primarySchemeConfig is null)
                return await base.GetDefaultAuthenticateSchemeAsync();

            return await this.GetSchemeAsync(primarySchemeConfig.SchemeName);
        }
    }
}
