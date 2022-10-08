using CleanMicroserviceSystem.Oceanus.Application.Abstraction.Configurations;
using CleanMicroserviceSystem.Tethys.Infrastructure;
using NLog;
using NLog.Web;

var assemblyName = typeof(Program).Assembly.GetName();
var setupInformation = AppDomain.CurrentDomain.SetupInformation;
var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Info($"{assemblyName.Name} launches, TargetFrameworkName={setupInformation.TargetFrameworkName}, Version={assemblyName.Version}");

try
{
    AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
    AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
    Console.CancelKeyPress += Console_CancelKeyPress;
    var builder = WebApplication.CreateBuilder(args);
    var config = builder.Configuration;

    builder.Services.AddInfrastructure(new OceanusDBConfiguration()
    {
        ConnectionString = config.GetConnectionString("ServiceDB")!
    });
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();
    var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
    lifetime.ApplicationStarted.Register(ApplicationLifetime_ApplicationStarted);
    lifetime.ApplicationStopping.Register(ApplicationLifetime_ApplicationStopping);
    lifetime.ApplicationStopped.Register(ApplicationLifetime_ApplicationStopped);

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

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

void Console_CancelKeyPress(object? sender, ConsoleCancelEventArgs e)
    => logger.Info($"{nameof(Console_CancelKeyPress)} => {(e.Cancel ? "Cancel" : "Not Cancel")}, {e.SpecialKey.ToString()}");

void CurrentDomain_ProcessExit(object? sender, EventArgs e)
    => logger.Info($"{nameof(CurrentDomain_ProcessExit)}");

void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    => logger.Error($"{nameof(CurrentDomain_UnhandledException)} => {(e.IsTerminating ? "Terminating" : "Not Terminating")}, {e.ExceptionObject.ToString()}");

void ApplicationLifetime_ApplicationStarted()
    => logger.Info($"{nameof(ApplicationLifetime_ApplicationStarted)}");

void ApplicationLifetime_ApplicationStopping()
    => logger.Info($"{nameof(ApplicationLifetime_ApplicationStopping)}");

void ApplicationLifetime_ApplicationStopped()
    => logger.Info($"{nameof(ApplicationLifetime_ApplicationStopped)}");
