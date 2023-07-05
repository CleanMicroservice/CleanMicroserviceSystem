using CleanMicroserviceSystem.Oceanus.WebAPI.Abstraction;
using CleanMicroserviceSystem.Hermes.Infrastructure;

namespace CleanMicroserviceSystem.Hermes.WebAPI;

public class HermesProgram : OceanusProgram
{
    public HermesProgram(NLog.ILogger logger) : base(logger) { }

    public override void ConfigureServices()
    {
        webAppBuilder.Services.AddInfrastructure(this.configManager);
        base.ConfigureServices();
    }
}
