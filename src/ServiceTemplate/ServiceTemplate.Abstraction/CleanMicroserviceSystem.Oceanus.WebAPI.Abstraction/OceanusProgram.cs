using System.Reflection;
using CleanMicroserviceSystem.Authentication.Configurations;
using CleanMicroserviceSystem.Authentication.Extensions;
using CleanMicroserviceSystem.Gateway.Configurations;
using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.DataSeed;
using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using MSLoggingLevel = Microsoft.Extensions.Logging.LogLevel;

namespace CleanMicroserviceSystem.Oceanus.WebAPI.Abstraction;

public class OceanusProgram
{
    protected NLog.ILogger logger;
    protected WebApplicationBuilder webAppBuilder;
    protected WebApplication webApp;
    protected ConfigurationManager configManager;
    protected AssemblyName assemblyName;

    public OceanusProgram(NLog.ILogger logger)
    {
        this.logger = logger;
    }

    public virtual void ConfigureHostBuilder(string[] args)
    {
        AppDomain.CurrentDomain.UnhandledException += this.CurrentDomain_UnhandledException;
        AppDomain.CurrentDomain.ProcessExit += this.CurrentDomain_ProcessExit;
        Console.CancelKeyPress += this.Console_CancelKeyPress;
        this.webAppBuilder = WebApplication.CreateBuilder(args);
        this.configManager = this.webAppBuilder.Configuration;
        this.webAppBuilder.Logging
            .ClearProviders()
            .AddConsole()
            .AddDebug()
            .SetMinimumLevel(MSLoggingLevel.Debug)
            .AddNLogWeb();
    }

    public virtual void ConfigureServices()
    {
        /* JwtBearerConfiguration was stored in user secrets file, to make multiple applications reuse this part of section by same Secret Id
         * Right click on WebAPI project in Solution Explorer window
         * Click Manage User Secrets (User secrets id was specified in <UserSecretsId> of project file)
         * Configurations in user secrets file will be imported into _configurationManager automatically when startup
         */
        var jwtBearerConfiguration = this.configManager.GetRequiredSection("JwtBearerConfiguration")!.Get<JwtBearerConfiguration>()!;
        var agentServiceRegistrationConfiguration = this.configManager.GetRequiredSection("AgentServiceRegistrationConfiguration")!.Get<AgentServiceRegistrationConfiguration>()!;
        this.webAppBuilder.Services.AddJwtBearerAuthentication(jwtBearerConfiguration!);
        this.webAppBuilder.Services.AddHttpContextAccessor();
        this.webAppBuilder.Services.AddControllers();
        this.webAppBuilder.Services.AddEndpointsApiExplorer();
        this.webAppBuilder.Services.AddOceanusSwaggerGen();
        this.webAppBuilder.Services.AddOceanusServices(agentServiceRegistrationConfiguration);
        this.webApp = this.webAppBuilder.Build();
    }

    public virtual void ConfigurePipelines()
    {
        if (this.webApp.Environment.IsDevelopment())
        {
            _ = this.webApp.UseSwagger();
            _ = this.webApp.UseSwaggerUI(options => options.EnablePersistAuthorization());
        }
        this.webApp.UseCors();
        this.webApp.UseOceanusPipelines();
        this.webApp.UseHttpsRedirection();
        this.ConfigurePipelinesBeforeAuth();
        this.webApp.UseAuthentication();
        this.webApp.UseAuthorization();
        this.ConfigurePipelinesAfterAuth();
        this.webApp.MapControllers();
    }

    public virtual void ConfigurePipelinesBeforeAuth()
    {
    }

    public virtual void ConfigurePipelinesAfterAuth()
    {
    }

    public virtual void ConfigureWebApp()
    {
        var servicesProvider = this.webApp.Services;
        servicesProvider.InitializeDatabaseAsync().ConfigureAwait(false);
        var lifetime = servicesProvider.GetRequiredService<IHostApplicationLifetime>();
        lifetime.ApplicationStarted.Register(this.ApplicationLifetime_ApplicationStarted);
        lifetime.ApplicationStopping.Register(this.ApplicationLifetime_ApplicationStopping);
        lifetime.ApplicationStopped.Register(this.ApplicationLifetime_ApplicationStopped);
    }

    public virtual void Run() => this.webApp.Run();

    public virtual void Console_CancelKeyPress(object? sender, ConsoleCancelEventArgs e)
        => this.logger?.Info($"{nameof(Console_CancelKeyPress)} => {(e.Cancel ? "Cancel" : "Not Cancel")}, {e.SpecialKey}");

    public virtual void CurrentDomain_ProcessExit(object? sender, EventArgs e)
        => this.logger?.Info($"{nameof(CurrentDomain_ProcessExit)}");

    public virtual void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        => this.logger?.Error($"{nameof(CurrentDomain_UnhandledException)} => {(e.IsTerminating ? "Terminating" : "Not Terminating")}, {e.ExceptionObject}");

    public virtual void ApplicationLifetime_ApplicationStarted()
        => this.logger?.Info($"{nameof(ApplicationLifetime_ApplicationStarted)}");

    public virtual void ApplicationLifetime_ApplicationStopping()
        => this.logger?.Info($"{nameof(ApplicationLifetime_ApplicationStopping)}");

    public virtual void ApplicationLifetime_ApplicationStopped()
        => this.logger?.Info($"{nameof(ApplicationLifetime_ApplicationStopped)}");
}
