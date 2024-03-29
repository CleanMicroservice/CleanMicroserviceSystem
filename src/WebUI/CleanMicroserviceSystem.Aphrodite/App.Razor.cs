﻿using Microsoft.AspNetCore.Components.Routing;

namespace CleanMicroserviceSystem.Aphrodite;

public partial class App
{
    protected override async Task OnInitializedAsync()
    {
        this.logger.LogInformation($"Initialize App...");
        await base.OnInitializedAsync();
    }

    protected async Task OnNavigateAsync(NavigationContext navigationContext)
    {
        this.logger.LogInformation($"Navigating to {navigationContext.Path}");
        await Task.CompletedTask;
    }
}
