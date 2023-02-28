namespace CleanMicroserviceSystem.Aphrodite.Domain;

public class RouterContract
{
    public const string DefaultUri = "/";
    public const string NotFoundUri = "/system/notfound";
    public const string UnauthorizedUri = "/system/unauthorized";

    public const string LoginUri = "/account/login";
    public const string ProfileUri = "/account/profile";
    public const string SettingsUri = "/system/settings";
}
