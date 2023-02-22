using Microsoft.AspNetCore.Components;

namespace CleanMicroserviceSystem.Aphrodite.Shared
{
    public partial class NavigationItemBase : ComponentBase
    {
        protected readonly NavigationManager navigationManager;

        public NavigationItemBase(
            NavigationManager navigationManager) : base()
        {
            this.navigationManager = navigationManager;
        }
    }
}
