using CleanMicroserviceSystem.Themis.WebAPI;
using NLog;
using NLog.Web;

var assemblyName = typeof(Program).Assembly.GetName();
var setupInformation = AppDomain.CurrentDomain.SetupInformation;
var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Info($"{assemblyName.Name} launches, TargetFrameworkName={setupInformation.TargetFrameworkName}, Version={assemblyName.Version}");

try
{
    var program = new ThemisProgram(logger);
    program.ConfigureHostBuilder(args);
    program.ConfigureServices();
    program.ConfigurePipelines();
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
