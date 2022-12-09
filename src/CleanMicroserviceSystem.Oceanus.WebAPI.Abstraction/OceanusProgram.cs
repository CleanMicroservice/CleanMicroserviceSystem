using System.Reflection;
using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.DataSeed;
using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Extensions;
using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;

namespace CleanMicroserviceSystem.Oceanus.WebAPI.Abstraction;

public class OceanusProgram
{
    protected ILogger _logger;
    protected WebApplicationBuilder _webApplicationBuilder;
    protected WebApplication _webApplication;
    protected ConfigurationManager _configurationManager;
    protected AssemblyName _assemblyName;

    public OceanusProgram(ILogger logger)
    {
        _logger = logger;
    }

    public virtual void ConfigureHostBuilder(string[] args)
    {
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        Console.CancelKeyPress += Console_CancelKeyPress;
        _webApplicationBuilder = WebApplication.CreateBuilder(args);
        _configurationManager = _webApplicationBuilder.Configuration;
    }

    public virtual void ConfigureServices()
    {
        _webApplicationBuilder.Services.AddHttpContextAccessor();
        _webApplicationBuilder.Services.AddControllers();
        _webApplicationBuilder.Services.AddEndpointsApiExplorer();
        _webApplicationBuilder.Services.AddSwaggerGen();
        _webApplicationBuilder.Services.AddOceanusRepositoryServices();

        _webApplication = _webApplicationBuilder.Build();
        var lifetime = _webApplication.Services.GetRequiredService<IHostApplicationLifetime>();
        lifetime.ApplicationStarted.Register(ApplicationLifetime_ApplicationStarted);
        lifetime.ApplicationStopping.Register(ApplicationLifetime_ApplicationStopping);
        lifetime.ApplicationStopped.Register(ApplicationLifetime_ApplicationStopped);
    }

    public virtual void ConfigurePipelines()
    {
        if (_webApplication.Environment.IsDevelopment())
        {
            _webApplication.UseSwagger();
            _webApplication.UseSwaggerUI();
        }

        _webApplication.UseWebAPILogging();

        _webApplication.UseHttpsRedirection();

        _webApplication.UseAuthorization();

        _webApplication.MapControllers();
    }

    public virtual void Run()
    {
        _webApplication
            .InitializeDatabase()
            .Run();
    }

    public virtual void Console_CancelKeyPress(object? sender, ConsoleCancelEventArgs e)
        => _logger.Info($"{nameof(Console_CancelKeyPress)} => {(e.Cancel ? "Cancel" : "Not Cancel")}, {e.SpecialKey}");

    public virtual void CurrentDomain_ProcessExit(object? sender, EventArgs e)
        => _logger.Info($"{nameof(CurrentDomain_ProcessExit)}");

    public virtual void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        => _logger.Error($"{nameof(CurrentDomain_UnhandledException)} => {(e.IsTerminating ? "Terminating" : "Not Terminating")}, {e.ExceptionObject}");

    public virtual void ApplicationLifetime_ApplicationStarted()
        => _logger.Info($"{nameof(ApplicationLifetime_ApplicationStarted)}");

    public virtual void ApplicationLifetime_ApplicationStopping()
        => _logger.Info($"{nameof(ApplicationLifetime_ApplicationStopping)}");

    public virtual void ApplicationLifetime_ApplicationStopped()
        => _logger.Info($"{nameof(ApplicationLifetime_ApplicationStopped)}");
}
