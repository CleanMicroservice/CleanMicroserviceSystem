using Microsoft.AspNetCore.Http;

namespace CleanMicroserviceSystem.Authentication.Configurations;

public class AuthenticationSchemeConfigurations
{
    public IEnumerable<AuthenticationSchemeConfiguration> SchemeConfigurations { get; set; }
}

public class AuthenticationSchemeConfiguration
{
    public AuthenticationSchemeConfiguration(
        string schemeName,
        Func<HttpContext, bool> predicate)
    {
        this.Predicate = predicate;
        this.SchemeName = schemeName;
    }

    public Func<HttpContext, bool> Predicate { get; set; }

    public string SchemeName { get; set; }
}
