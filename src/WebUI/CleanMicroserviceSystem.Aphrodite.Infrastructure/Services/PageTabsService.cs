using CleanMicroserviceSystem.Aphrodite.Domain;
using Masa.Blazor.Presets;

namespace CleanMicroserviceSystem.Aphrodite.Infrastructure.Services;

public class PageTabsService
{
    public PPageTabs? PageTabsComponent { get; set; }

    public string[] SelfPatterns = new string[]
    {
        RouterContract.UsersEditUriPattern,
        RouterContract.RolesEditUriPattern,
        RouterContract.ClientsEditUriPattern,
    };
}
