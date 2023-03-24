using CleanMicroserviceSystem.Oceanus.WebAPI.Abstraction;
using CleanMicroserviceSystem.Uranus.Infrastructure;
using Ocelot.Middleware;

namespace CleanMicroserviceSystem.Uranus.WebAPI;

public class UranusProgram : OceanusProgram
{
    public UranusProgram(NLog.ILogger logger) : base(logger) { }

    public override void ConfigurePipelinesBeforeAuth()
    {
        base.ConfigurePipelinesBeforeAuth();
        this.webApp.UseOcelot().Wait();
    }

    public override void ConfigureServices()
    {
        this.webAppBuilder.Services.AddInfrastructure(this.configManager);
        base.ConfigureServices();
    }
}
