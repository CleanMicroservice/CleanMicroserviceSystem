﻿using CleanMicroserviceSystem.Oceanus.WebAPI.Abstraction;
using CleanMicroserviceSystem.Tethys.Infrastructure;

namespace CleanMicroserviceSystem.Tethys.WebAPI;

public class TethysProgram : OceanusProgram
{
    public TethysProgram(NLog.ILogger logger) : base(logger) { }

    public override void ConfigureServices()
    {
        webAppBuilder.Services.AddInfrastructure(this.configManager);
        base.ConfigureServices();
    }
}
