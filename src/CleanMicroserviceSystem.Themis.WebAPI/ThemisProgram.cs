using CleanMicroserviceSystem.Oceanus.Application.Abstraction.Configurations;
using CleanMicroserviceSystem.Oceanus.WebAPI.Abstraction;
using CleanMicroserviceSystem.Themis.Infrastructure;

namespace CleanMicroserviceSystem.Themis.WebAPI;

public class ThemisProgram : OceanusProgram
{
    public ThemisProgram(NLog.ILogger logger) : base(logger) { }

    public override void ConfigureServices()
    {
        webAppBuilder.Services.AddInfrastructure(new OceanusDBConfiguration()
        {
            ConnectionString = configManager.GetConnectionString("ServiceDB")!
        });
        base.ConfigureServices();
    }
}
