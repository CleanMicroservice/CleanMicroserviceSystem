using Masa.Blazor.Presets;
using Microsoft.AspNetCore.Components;

namespace CleanMicroserviceSystem.Aphrodite.Pages.SystemPages;

public class AphroditePageBase : ComponentBase
{
    [CascadingParameter]
    public IPageTabsProvider PageTabsProvider { get; set; } = default!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;
}
