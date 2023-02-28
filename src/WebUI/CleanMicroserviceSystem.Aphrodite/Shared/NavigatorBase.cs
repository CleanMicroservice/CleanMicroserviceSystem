using Microsoft.AspNetCore.Components;

namespace CleanMicroserviceSystem.Aphrodite.Shared;

public partial class NavigatorBase : ComponentBase
{
    [Inject]
    protected NavigationManager NavigationManager { get; set; }
}
