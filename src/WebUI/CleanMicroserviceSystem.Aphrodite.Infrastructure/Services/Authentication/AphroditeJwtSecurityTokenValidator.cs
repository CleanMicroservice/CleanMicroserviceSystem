using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Aphrodite.Infrastructure.Services.Authentication
{
    public class AphroditeJwtSecurityTokenValidator
    {
        private readonly ILogger<AphroditeJwtSecurityTokenValidator> logger;

        public AphroditeJwtSecurityTokenValidator(
            ILogger<AphroditeJwtSecurityTokenValidator> logger)
        {
            this.logger = logger;
        }

        public async Task<bool> ValidateAsync(JwtSecurityToken? token)
        {
            this.logger.LogInformation($"Validate JwtSecurityToken [{token?.Id}]...");
            if (token is null) return false;

            DateTime utcNow = DateTime.UtcNow;
            if (token.Payload.Nbf.HasValue && utcNow < token.ValidFrom)
                return false;
            if (token.Payload.Exp.HasValue && token.ValidTo < utcNow)
                return false;

            return true;
        }
    }
}
