using Microsoft.AspNetCore.Authorization;

namespace CleanMicroserviceSystem.Authentication.Domain;

public static class IdentityContract
{
    #region SchemeType

    public const string JwtAuthenticationType = $"Jwt";
    public const string BearerScheme = $"Bearer";
    public const string UserJwtBearerScheme = $"Jwt{BearerScheme}_User";
    public const string ClientJwtBearerScheme = $"Jwt{BearerScheme}_Client";
    #endregion

    #region Users

    public const string DefaultUserPassword = "Passw0rd";
    public const int AdminUserId = 1;
    public const string AdminUser = "Leon";
    public const int OperatorUserId = 2;
    public const string OperatorUser = "Mathilda";
    #endregion

    #region Roles

    public const int AdministratorRoleId = 1;
    public const string AdministratorRole = "Administrator";
    public const int OperatorRoleId = 2;
    public const string OperatorRole = "Operator";
    public const int CommonRoleId = 3;
    public const string CommonRole = "Common";
    #endregion

    #region Headers

    public const string AuthenticationSchemeHeaderName = "authentication_scheme";
    public const string ClientAuthenticationSchemeHeaderValue = "client";
    #endregion

    #region Clients

    public const string TethysClient = "Tethys";
    public const string ThemisNTLMClient = "ThemisNTLM";
    #endregion

    #region Accesses

    public const string Read = "Read";
    public const string Write = "Write";
    public const string Delete = "Delete";
    #endregion

    #region ApiResources

    public const string ThemisAPIResource = "ThemisAPI";
    public const string AstraAPIResource = "AstraAPI";
    #endregion

    #region PolicyNames

    public const string ThemisAPIReadPolicyName = $"{ThemisAPIResource}_{Read}";
    public const string ThemisAPIWritePolicyName = $"{ThemisAPIResource}_{Write}";
    public const string AstraAPIReadPolicyName = $"{AstraAPIResource}_{Read}";
    public const string AstraAPIWritePolicyName = $"{AstraAPIResource}_{Write}";
    public const string AstraAPIDeletePolicyName = $"{AstraAPIResource}_{Delete}";
    #endregion

    #region PolicyBuilders

    public static AuthorizationPolicy ThemisAPIReadPolicy =
        new AuthorizationPolicyBuilder().RequireClaim(ThemisAPIResource, Read).Build();
    public static AuthorizationPolicy ThemisAPIWritePolicy =
        new AuthorizationPolicyBuilder().RequireClaim(ThemisAPIResource, Write).Build();
    public static AuthorizationPolicy AstraAPIReadPolicy =
        new AuthorizationPolicyBuilder().RequireClaim(AstraAPIResource, Read).Build();
    public static AuthorizationPolicy AstraAPIWritePolicy =
        new AuthorizationPolicyBuilder().RequireClaim(AstraAPIResource, Write).Build();
    public static AuthorizationPolicy AstraAPIDeletePolicy =
        new AuthorizationPolicyBuilder().RequireClaim(AstraAPIResource, Delete).Build();
    #endregion
}
