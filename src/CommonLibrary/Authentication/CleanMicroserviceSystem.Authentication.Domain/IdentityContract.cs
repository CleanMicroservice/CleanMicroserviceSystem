using Microsoft.AspNetCore.Authorization;

namespace CleanMicroserviceSystem.Authentication.Domain;

public static class IdentityContract
{
    #region Users

    public const string SuperUser = "Leon";
    public const int SuperUserId = 1;
    public const string CommonUser = "Mathilda";
    public const int CommonUserId = 2;
    #endregion

    #region Roles

    public const string AdministratorRole = "Administrator";
    public const int AdministratorRoleId = 1;
    public const string OperatorRole = "Operator";
    public const int OperatorRoleId = 2;
    #endregion

    #region Policies

    public const string AccessUsersPolicy = "AccessUsersPolicy";
    public const string AccessRolesPolicy = "AccessRolesPolicy";
    public const string AccessClientsPolicy = "AccessClientsPolicy";
    #endregion

    #region PolicyBuilders

    public static AuthorizationPolicyBuilder IsAdministratorRolePolicyBuilder = new AuthorizationPolicyBuilder()
        .RequireRole(AdministratorRole);
    #endregion
}
