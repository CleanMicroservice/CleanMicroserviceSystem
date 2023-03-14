using CleanMicroserviceSystem.Oceanus.Application.Abstraction.Configurations;
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
        var dbConfig = new OceanusDbConfiguration()
        {
            ConnectionString = this.configManager.GetConnectionString("ServiceDB")!
        };
        _ = this.webAppBuilder.Services.AddInfrastructure(dbConfig);
        base.ConfigureServices();
    }
}
