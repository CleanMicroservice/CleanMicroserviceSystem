using Microsoft.AspNetCore.Components;

namespace CleanMicroserviceSystem.Aphrodite.Shared
{
    public partial class NavigationItemBase : ComponentBase
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }
    }
}
