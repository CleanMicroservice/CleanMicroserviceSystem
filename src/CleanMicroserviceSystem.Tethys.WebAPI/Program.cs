using CleanMicroserviceSystem.Tethys.WebAPI;
using NLog;
using NLog.Web;

var assemblyName = typeof(Program).Assembly.GetName();
var setupInformation = AppDomain.CurrentDomain.SetupInformation;
var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Info($"{assemblyName.Name} launches, TargetFrameworkName={setupInformation.TargetFrameworkName}, Version={assemblyName.Version}");

try
{
    var program = new TethysProgram(logger);
    program.ConfigureHostBuilder(args);
    program.ConfigureServices();
    program.ConfigurePipelines();
    program.ConfigureWebApp();
    program.Run();
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
