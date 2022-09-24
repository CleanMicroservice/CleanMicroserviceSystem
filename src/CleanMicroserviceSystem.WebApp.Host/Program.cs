using NLog;
using NLog.Web;
using CleanMicroserviceSystem.WebAppHost.Extensions;

var assemblyName = typeof(Program).Assembly.GetName();
var setupInformation = AppDomain.CurrentDomain.SetupInformation;
var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Info($"{assemblyName.Name} launches, TargetFrameworkName={setupInformation.TargetFrameworkName}, Version={assemblyName.Version}");

try
{
    var builder = WebApplication.CreateBuilder(args);
    var environment = builder.Environment;
    var configuration = builder.Configuration;

    builder.ConfigurateLog();

    builder.Services.AddControllersWithViews();
    builder.Services.AddRazorPages();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseWebAssemblyDebugging();
    }
    else
    {
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseBlazorFrameworkFiles();
    app.UseStaticFiles();

    app.UseRouting();

    app.MapRazorPages();
    app.MapControllers();
    app.MapFallbackToFile("index.html");

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, $"{assemblyName.Name} launches failed.");
}
finally
{
    logger.Info($"{assemblyName.Name} shutdown, Version={assemblyName.Version}");
    LogManager.Shutdown();
}
