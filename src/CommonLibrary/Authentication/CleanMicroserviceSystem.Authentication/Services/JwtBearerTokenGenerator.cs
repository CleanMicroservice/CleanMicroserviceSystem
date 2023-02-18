using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CleanMicroserviceSystem.Authentication.Configurations;
using Microsoft.IdentityModel.Tokens;

namespace CleanMicroserviceSystem.Authentication.Services;

public class JwtBearerTokenGenerator : IJwtBearerTokenGenerator
{
    private readonly IOptionsSnapshot<JwtBearerConfiguration> options;
    private readonly SymmetricSecurityKey securityKey;
    private readonly SigningCredentials signingCredentials;
    private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler = new();

    public JwtBearerTokenGenerator(
        IOptionsSnapshot<JwtBearerConfiguration> options)
    {
        this.options = options;
        this.securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.options.Value.JwtSecurityKey));
        this.signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
    }

    public string GenerateUserSecurityToken(IEnumerable<Claim> claims)
    {
        var expiry = DateTime.Now.AddMinutes(this.options.Value.JwtExpiryInMinutes);

        var token = new JwtSecurityToken(
            this.options.Value.JwtIssuer,
            this.options.Value.JwtAudience,
            claims,
            expires: expiry,
            signingCredentials: this.signingCredentials);

        return this.GenerateSecurityToken(token);
    }

    public string GenerateClientSecurityToken(IEnumerable<Claim> claims)
    {
        var token = new JwtSecurityToken(
            this.options.Value.JwtIssuer,
            this.options.Value.JwtAudience,
            claims,
            expires: null,
            signingCredentials: this.signingCredentials);

        return this.GenerateSecurityToken(token);
    }

    protected string GenerateSecurityToken(JwtSecurityToken token)
    {
        return this.jwtSecurityTokenHandler.WriteToken(token);
    }
}
