using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleanMicroserviceSystem.Themis.Infrastructure.Persistence.Configurations;

public static class IdentityConfiguration
{
    public static ModelBuilder ConfigureUserClaim(this ModelBuilder modelBuilder)
    {
        var userClaimBuilder = modelBuilder.Entity<IdentityUserClaim<int>>();
        userClaimBuilder.HasIndex(nameof(IdentityUserClaim<int>.ClaimType), nameof(IdentityUserClaim<int>.ClaimValue));
        return modelBuilder;
    }
}
