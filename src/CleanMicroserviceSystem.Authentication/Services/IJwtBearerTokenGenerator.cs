using System.Security.Claims;

namespace CleanMicroserviceSystem.Authentication.Services;
public interface IJwtBearerTokenGenerator
{
    string GenerateSecurityToken(IEnumerable<Claim> claims);
}
