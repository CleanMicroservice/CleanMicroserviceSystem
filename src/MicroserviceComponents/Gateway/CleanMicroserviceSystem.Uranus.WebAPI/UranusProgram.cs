using CleanMicroserviceSystem.Oceanus.Application.Abstraction.Configurations;
using CleanMicroserviceSystem.Oceanus.WebAPI.Abstraction;
using CleanMicroserviceSystem.Uranus.Infrastructure;
using Ocelot.Middleware;

namespace CleanMicroserviceSystem.Uranus.WebAPI;

public class UranusProgram : OceanusProgram
{
    public UranusProgram(NLog.ILogger logger) : base(logger) { }

    public override void ConfigurePipelines()
    {
        base.ConfigurePipelines();
        webApp.UseOcelot().Wait();
    }

    public override void ConfigureServices()
    {
        var dbConfig = new OceanusDbConfiguration()
        {
            ConnectionString = configManager.GetConnectionString("ServiceDB")!
        };
        webAppBuilder.Services.AddInfrastructure(dbConfig);
        base.ConfigureServices();
    }
}
