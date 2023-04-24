using CleanMicroserviceSystem.Authentication.Domain;
using CleanMicroserviceSystem.Themis.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleanMicroserviceSystem.Themis.Infrastructure.DataSeeds;

public static class IdentityDatabaseInitializer
{
    public static ModelBuilder InitializeIdentityData(this ModelBuilder builder)
    {
        builder.Entity<OceanusRole>().HasData(new[]
        {
            new OceanusRole() {
                Id = IdentityContract.AdministratorRoleId,
                Name = IdentityContract.AdministratorRole,
                NormalizedName = IdentityContract.AdministratorRole.ToUpper(),
                ConcurrencyStamp = "8ef3768d-cdd3-43a4-9338-c549cec56942"
            },
            new OceanusRole() {
                Id = IdentityContract.OperatorRoleId,
                Name = IdentityContract.OperatorRole,
                NormalizedName = IdentityContract.OperatorRole.ToUpper(),
                ConcurrencyStamp = "43daf209-df6b-499c-83e5-94ea05cf8997"
            },
            new OceanusRole()
            {
                Id = IdentityContract.CommonRoleId,
                Name = IdentityContract.CommonRole,
                NormalizedName=IdentityContract.CommonRole.ToUpper(),
                ConcurrencyStamp = "ae075bb1-39d2-4c3a-a054-526e0ad7512c"
            }
        });

        builder.Entity<OceanusUser>().HasData(new[]
        {
            new OceanusUser() {
                Id = IdentityContract.AdminUserId,
                UserName = IdentityContract.AdminUser,
                NormalizedUserName = IdentityContract.AdminUser.ToUpper(),
                Email = "leon@icann.com",
                NormalizedEmail = "LEON@ICANN.COM",
                EmailConfirmed = true,
                PhoneNumber = "100001",
                PhoneNumberConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEBpsyxgzjSNJvSIm6y3I1jqvKN4iV/IkvwmMrrYR5X8a6pEXza2RwA9xxSXidOiGkQ==",
                SecurityStamp = "SU6NODNYTSGYJ5NXXYIA7I2M542MLV2V",
                ConcurrencyStamp = "baeb86b5-116c-43ae-ade7-489dabd07012",
                TwoFactorEnabled = false,
                LockoutEnd = null,
                LockoutEnabled = true,
                AccessFailedCount = 0
            },
            new OceanusUser() {
                Id = IdentityContract.OperatorUserId,
                UserName = IdentityContract.OperatorUser,
                NormalizedUserName = IdentityContract.OperatorUser.ToUpper(),
                Email = "mathilda@icann.com",
                NormalizedEmail = "MATHILDA@ICANN.COM",
                EmailConfirmed = true,
                PhoneNumber = "100002",
                PhoneNumberConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEDjIsjVamUxv4OQ06Ur/7YnsqddYfO2eQP7UK/Adjs38RIkmBpgTldrfCXZ5QHP1vQ==",
                SecurityStamp = "2NGFUDFGMLPCBN5U67CHXJEYIDBWQPO3",
                ConcurrencyStamp = "93cdc1b8-0c84-4f52-9245-d6ae4bbe5f59",
                TwoFactorEnabled = false,
                LockoutEnd = null,
                LockoutEnabled = true,
                AccessFailedCount = 0
            }
        });

        builder.Entity<IdentityUserRole<int>>().HasData(new[]
        {
            new IdentityUserRole<int>() { UserId = IdentityContract.AdminUserId, RoleId = IdentityContract.AdministratorRoleId },
            new IdentityUserRole<int>() { UserId = IdentityContract.AdminUserId, RoleId = IdentityContract.OperatorRoleId },
            new IdentityUserRole<int>() { UserId = IdentityContract.AdminUserId, RoleId = IdentityContract.CommonRoleId },
            new IdentityUserRole<int>() { UserId = IdentityContract.OperatorUserId, RoleId = IdentityContract.OperatorRoleId },
            new IdentityUserRole<int>() { UserId = IdentityContract.OperatorUserId, RoleId = IdentityContract.CommonRoleId },
        });

        builder.Entity<IdentityUserClaim<int>>().HasData(new[]
        {
            new IdentityUserClaim<int> { Id = 1, UserId = IdentityContract.AdminUserId, ClaimType = nameof(IdentityContract.AdminUser), ClaimValue = IdentityContract.Read },
            new IdentityUserClaim<int> { Id = 2, UserId = IdentityContract.AdminUserId, ClaimType = nameof(IdentityContract.AdminUser), ClaimValue = IdentityContract.Write },
            new IdentityUserClaim<int> { Id = 3, UserId = IdentityContract.OperatorUserId, ClaimType = nameof(IdentityContract.OperatorUser), ClaimValue = IdentityContract.Read },
            new IdentityUserClaim<int> { Id = 4, UserId = IdentityContract.OperatorUserId, ClaimType = nameof(IdentityContract.OperatorUser), ClaimValue = IdentityContract.Write },
        });

        builder.Entity<IdentityRoleClaim<int>>().HasData(new[]
        {
            new IdentityRoleClaim<int> { Id=1, RoleId = IdentityContract.AdministratorRoleId, ClaimType = IdentityContract.ThemisAPIResource, ClaimValue = IdentityContract.Read },
            new IdentityRoleClaim<int> { Id=2, RoleId = IdentityContract.AdministratorRoleId, ClaimType = IdentityContract.ThemisAPIResource, ClaimValue = IdentityContract.Write },
            new IdentityRoleClaim<int> { Id=3, RoleId = IdentityContract.AdministratorRoleId, ClaimType = IdentityContract.AstraAPIResource, ClaimValue = IdentityContract.Delete },
            new IdentityRoleClaim<int> { Id=4, RoleId = IdentityContract.OperatorRoleId, ClaimType = IdentityContract.ThemisAPIResource, ClaimValue = IdentityContract.Read },
            new IdentityRoleClaim<int> { Id=5, RoleId = IdentityContract.CommonRoleId, ClaimType = IdentityContract.AstraAPIResource, ClaimValue = IdentityContract.Read },
            new IdentityRoleClaim<int> { Id=6, RoleId = IdentityContract.CommonRoleId, ClaimType = IdentityContract.AstraAPIResource, ClaimValue = IdentityContract.Write },
        });

        return builder;
    }
}

