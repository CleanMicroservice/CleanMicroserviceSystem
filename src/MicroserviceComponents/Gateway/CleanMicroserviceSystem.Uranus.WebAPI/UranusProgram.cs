﻿using CleanMicroserviceSystem.Gateway.Contract;
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
        // TODO: Ocelot
        // Ocelot pipeline has no logging;
        // WebAPILogMiddleware cause 415 error;
        this.webApp.UseWhen(
            context => context.Request.Path.StartsWithSegments(GatewayContract.GatewayUriPrefix),
            builder => builder.UseOcelot().Wait());
        base.ConfigurePipelines();
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
