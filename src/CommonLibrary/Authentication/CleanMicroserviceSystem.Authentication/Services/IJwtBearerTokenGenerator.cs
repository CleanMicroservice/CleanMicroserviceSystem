using System.Security.Claims;

namespace CleanMicroserviceSystem.Authentication.Services;

public interface IJwtBearerTokenGenerator
{
    string GenerateClientSecurityToken(IEnumerable<Claim> claims);
    string GenerateUserSecurityToken(IEnumerable<Claim> claims);
}
