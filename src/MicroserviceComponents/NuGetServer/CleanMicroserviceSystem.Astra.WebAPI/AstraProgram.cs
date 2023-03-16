using CleanMicroserviceSystem.Astra.Application.Configurations;
using CleanMicroserviceSystem.Astra.Infrastructure;
using CleanMicroserviceSystem.Astra.Infrastructure.Persistence;
using CleanMicroserviceSystem.Oceanus.Application.Abstraction.Configurations;
using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.DataSeed;
using CleanMicroserviceSystem.Oceanus.WebAPI.Abstraction;

namespace CleanMicroserviceSystem.Astra.WebAPI;

public class AstraProgram : OceanusProgram
{
    public AstraProgram(NLog.ILogger logger) : base(logger) { }

    public override void ConfigureServices()
    {
        var nuGetServerConfiguration = this.configManager.GetRequiredSection("NuGetServerConfiguration")!.Get<NuGetServerConfiguration>()!;
        this.webAppBuilder.Services.AddInfrastructure(
            new OceanusDbConfiguration()
            {
                ConnectionString = this.configManager.GetConnectionString("ServiceDB")!
            },
            nuGetServerConfiguration);
        base.ConfigureServices();
    }

    public override void ConfigureWebApp()
    {
        base.ConfigureWebApp();

        var servicesProvider = this.webApp.Services;
        servicesProvider.InitializeDatabaseAsync<BaGetDBContext>().ConfigureAwait(false);
    }
}
