using System.Security.Claims;

namespace CleanMicroserviceSystem.Aphrodite.Application.Extensions;

public static class ClaimsExtension
{
    public static bool IsExpired(this ClaimsIdentity identity, string expiryClaimType)
    {
        var expiryClaim = identity.Claims?.FirstOrDefault(claim => string.Equals(claim.Type, expiryClaimType, StringComparison.OrdinalIgnoreCase)) ?? default;
        if (expiryClaim == null)
        {
            return false;
        }

        if (!long.TryParse(expiryClaim.Value, out var expiryTimeStamp))
        {
            return true;
        }

        var localExpiredTime = TimeZoneInfo.ConvertTimeFromUtc(
            new DateTime(1970, 1, 1) + TimeSpan.FromSeconds(expiryTimeStamp),
            TimeZoneInfo.Local);
        return DateTime.Now > localExpiredTime;
    }

    public static int? GetNameIdentifier(this ClaimsIdentity identity)
    {
        var value = identity?.Claims.FirstOrDefault(claim => string.Equals(claim.Type, ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase))?.Value;
        if (string.IsNullOrEmpty(value)) return default;
        return int.TryParse(value, out int identifier) ? identifier : default;
    }

    public static string? GetPhone(this ClaimsIdentity identity)
    {
        return identity?.Claims.FirstOrDefault(claim => string.Equals(claim.Type, ClaimTypes.HomePhone, StringComparison.OrdinalIgnoreCase))?.Value;
    }

    public static string? GetEmail(this ClaimsIdentity identity)
    {
        return identity?.Claims.FirstOrDefault(claim => string.Equals(claim.Type, ClaimTypes.Email, StringComparison.OrdinalIgnoreCase))?.Value;
    }
}
