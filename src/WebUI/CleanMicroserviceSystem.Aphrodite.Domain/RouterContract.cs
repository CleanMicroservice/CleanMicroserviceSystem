namespace CleanMicroserviceSystem.Aphrodite.Domain;

public class RouterContract
{
    public const string DefaultUri = "/";
    public const string NotFoundUri = "/system/notfound";
    public const string UnauthorizedUri = "/system/unauthorized";

    public const string LoginUri = "/account/login";
    public const string RegisterUri = "/account/register";
    public const string ProfileUri = "/account/profile";
    public const string SettingsUri = "/system/settings";
    public const string UsersUri = "/permission/users";
    public const string UsersEditUri = "/permission/user/edit";
    public const string UsersEditUriPattern = $"{UsersEditUri}/{{id}}";
    public const string RolesUri = "/permission/roles";
    public const string RolesEditUri = "/permission/role/edit";
    public const string RolesEditUriPattern = $"{RolesEditUri}/{{id}}";
    public const string ClientsUri = "/permission/clients";
    public const string ClientsEditUri = "/permission/client/edit";
    public const string ClientsEditUriPattern = $"{ClientsEditUri}/{{id}}";

    public const string PackagesUri = "/nugetserver/packages";
}
