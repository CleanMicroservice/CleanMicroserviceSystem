using CleanMicroserviceSystem.Astra.Infrastructure;
using CleanMicroserviceSystem.Astra.Infrastructure.Persistence;
using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.DataSeed;
using CleanMicroserviceSystem.Oceanus.WebAPI.Abstraction;

namespace CleanMicroserviceSystem.Astra.WebAPI;

public class AstraProgram : OceanusProgram
{
    public AstraProgram(NLog.ILogger logger) : base(logger) { }

    public override void ConfigureServices()
    {
        this.webAppBuilder.Services.AddInfrastructure(this.configManager);
        base.ConfigureServices();
    }

    public override void ConfigureWebApp()
    {
        base.ConfigureWebApp();

        var servicesProvider = this.webApp.Services;
        servicesProvider.InitializeDatabaseAsync<BaGetDBContext>().ConfigureAwait(false);
    }
}
