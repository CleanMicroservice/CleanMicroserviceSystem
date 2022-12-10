using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleanMicroserviceSystem.Themis.Infrastructure.DataSeeds;

public static class IdentityDatabaseInitializer
{
    public static ModelBuilder InitializeIdentityData(this ModelBuilder builder)
    {
        builder.Entity<OceanusRole>().HasData(new[]
        {
            new OceanusRole() { Id = 1, Name = "Administrator", NormalizedName = "ADMINISTRATOR", ConcurrencyStamp = "8ef3768d-cdd3-43a4-9338-c549cec56942" },
            new OceanusRole() { Id = 2, Name = "Operator", NormalizedName = "OPERATOR", ConcurrencyStamp = "43daf209-df6b-499c-83e5-94ea05cf8997" }
        });

        builder.Entity<OceanusUser>().HasData(new[]
        {
            new OceanusUser() { Id = 1, UserName = "Leon", NormalizedUserName = "LEON", Email = "leon@icann.com", NormalizedEmail = "LEON@ICANN.COM", EmailConfirmed = true, PasswordHash = "AQAAAAEAACcQAAAAEBpsyxgzjSNJvSIm6y3I1jqvKN4iV/IkvwmMrrYR5X8a6pEXza2RwA9xxSXidOiGkQ==", SecurityStamp = "SU6NODNYTSGYJ5NXXYIA7I2M542MLV2V", ConcurrencyStamp = "baeb86b5-116c-43ae-ade7-489dabd07012", PhoneNumber = null, PhoneNumberConfirmed = false, TwoFactorEnabled = false, LockoutEnd = null, LockoutEnabled = true, AccessFailedCount = 0 },
            new OceanusUser() { Id = 2, UserName = "Mathilda", NormalizedUserName = "MATHILDA", Email = "mathilda@icann.com", NormalizedEmail = "MATHILDA@ICANN.COM", EmailConfirmed = true, PasswordHash = "AQAAAAEAACcQAAAAEDjIsjVamUxv4OQ06Ur/7YnsqddYfO2eQP7UK/Adjs38RIkmBpgTldrfCXZ5QHP1vQ==", SecurityStamp = "2NGFUDFGMLPCBN5U67CHXJEYIDBWQPO3", ConcurrencyStamp = "93cdc1b8-0c84-4f52-9245-d6ae4bbe5f59", PhoneNumber = null, PhoneNumberConfirmed = false, TwoFactorEnabled = false, LockoutEnd = null, LockoutEnabled = true, AccessFailedCount = 0 }
        });

        builder.Entity<IdentityUserRole<int>>().HasData(new[]
        {
            new IdentityUserRole<int>() { UserId = 1, RoleId = 1 },
            new IdentityUserRole<int>() { UserId = 2, RoleId = 2 }
        });

        builder.Entity<IdentityUserClaim<int>>().HasData(new[]
        {
            new IdentityUserClaim<int> { Id = 1, UserId = 1, ClaimType = "Access", ClaimValue = "ReadWrite" }
        });

        return builder;
    }
}

