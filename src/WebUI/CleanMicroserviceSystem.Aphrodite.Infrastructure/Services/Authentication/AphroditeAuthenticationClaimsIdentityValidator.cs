using System.Security.Claims;
using CleanMicroserviceSystem.Aphrodite.Application.Extensions;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Aphrodite.Infrastructure.Services.Authentication
{
    public class AphroditeAuthenticationClaimsIdentityValidator
    {
        private readonly ILogger<AphroditeAuthenticationClaimsIdentityValidator> logger;

        public AphroditeAuthenticationClaimsIdentityValidator(
            ILogger<AphroditeAuthenticationClaimsIdentityValidator> logger)
        {
            this.logger = logger;
        }

        public async Task<bool> ValidateAsync(ClaimsIdentity? identity)
        {
            logger.LogInformation($"Validate claims identity [{identity?.Name ?? string.Empty}]...");
            if (identity is null) return false;
            return !identity.IsExpired("exp");
        }
    }
}
