using CleanMicroserviceSystem.Oceanus.Application.Abstraction.Configurations;
using CleanMicroserviceSystem.Oceanus.WebAPI.Abstraction;
using CleanMicroserviceSystem.Astra.Infrastructure;

namespace CleanMicroserviceSystem.Astra.WebAPI;

public class AstraProgram : OceanusProgram
{
    public AstraProgram(NLog.ILogger logger) : base(logger) { }

    public override void ConfigureServices()
    {
        this.webAppBuilder.Services.AddInfrastructure(new OceanusDbConfiguration()
        {
            ConnectionString = this.configManager.GetConnectionString("ServiceDB")!
        });
        base.ConfigureServices();
    }
}
