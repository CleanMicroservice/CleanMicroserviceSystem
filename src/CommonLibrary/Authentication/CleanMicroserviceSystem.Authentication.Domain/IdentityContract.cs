using Microsoft.AspNetCore.Authorization;

namespace CleanMicroserviceSystem.Authentication.Domain;

public static class IdentityContract
{
    #region Roles

    public const string AdministratorRole = "Administrator";

    public const string OperatorRole = "Operator";
    #endregion

    #region Policies

    public const string AccessUsersPolicy = "AccessUsersPolicy";

    public const string AccessRolesPolicy = "AccessRolesPolicy";
    #endregion

    #region PolicyBuilders

    public static AuthorizationPolicyBuilder IsAdministratorRolePolicyBuilder = new AuthorizationPolicyBuilder()
        .RequireRole(AdministratorRole);
    #endregion
}
