using Microsoft.AspNetCore.Components;

namespace CleanMicroserviceSystem.Aphrodite.Pages.SystemPages;

public class AphroditePageBase : ComponentBase
{
    [Inject]
    protected NavigationManager NavigationManager { get; set; }

    [Inject]
    protected IHttpClientFactory HttpClientFactory { get; set; }
}
