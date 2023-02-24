using Microsoft.AspNetCore.Components;

namespace CleanMicroserviceSystem.Aphrodite.Pages.SystemPages
{
    public class AphroditePageBase : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IHttpClientFactory HttpClientFactory { get; set; }
    }
}
