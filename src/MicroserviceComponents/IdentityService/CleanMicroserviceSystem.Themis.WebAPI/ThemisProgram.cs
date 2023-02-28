using CleanMicroserviceSystem.Oceanus.Application.Abstraction.Configurations;
using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.DataSeed;
using CleanMicroserviceSystem.Oceanus.WebAPI.Abstraction;
using CleanMicroserviceSystem.Themis.Infrastructure;
using CleanMicroserviceSystem.Themis.Infrastructure.Persistence;

namespace CleanMicroserviceSystem.Themis.WebAPI;

public class ThemisProgram : OceanusProgram
{
    public ThemisProgram(NLog.ILogger logger) : base(logger) { }

    public override void ConfigureServices()
    {
        this.webAppBuilder.Services.AddInfrastructure(new OceanusDbConfiguration()
        {
            ConnectionString = this.configManager.GetConnectionString("ServiceDB")!
        });
        base.ConfigureServices();
    }

    public override void ConfigureWebApp()
    {
        base.ConfigureWebApp();

        var servicesProvider = this.webApp.Services;
        servicesProvider.InitializeDatabaseAsync<IdentityDbContext>().ConfigureAwait(false);
        servicesProvider.InitializeDatabaseAsync<ConfigurationDbContext>().ConfigureAwait(false);
    }
}
