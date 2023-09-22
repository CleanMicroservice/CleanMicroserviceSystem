using System.Security.Claims;

namespace CleanMicroserviceSystem.Authentication.Domain.Extensions;

public static class ClaimsExtension
{
    public static int? GetNameIdentifier(this ClaimsIdentity identity)
    {
        var value = identity?.Claims?.FirstOrDefault(claim => string.Equals(claim.Type, ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase))?.Value;
        if (string.IsNullOrEmpty(value)) return default;
        return int.TryParse(value, out var identifier) ? identifier : default;
    }

    public static string? GetPhone(this ClaimsIdentity identity)
    {
        return identity?.Claims?.FirstOrDefault(claim => string.Equals(claim.Type, ClaimTypes.HomePhone, StringComparison.OrdinalIgnoreCase))?.Value;
    }

    public static string? GetEmail(this ClaimsIdentity identity)
    {
        return identity?.Claims?.FirstOrDefault(claim => string.Equals(claim.Type, ClaimTypes.Email, StringComparison.OrdinalIgnoreCase))?.Value;
    }
}
