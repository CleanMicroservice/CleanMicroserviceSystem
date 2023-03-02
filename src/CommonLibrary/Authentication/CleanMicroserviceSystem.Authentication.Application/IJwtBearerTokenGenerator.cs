using System.Security.Claims;

namespace CleanMicroserviceSystem.Authentication.Application;

public interface IJwtBearerTokenGenerator
{
    string GenerateClientSecurityToken(IEnumerable<Claim> claims);
    string GenerateUserSecurityToken(IEnumerable<Claim> claims);
}
