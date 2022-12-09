﻿using CleanMicroserviceSystem.Oceanus.Application.Abstraction.Configurations;
using CleanMicroserviceSystem.Tethys.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CleanMicroserviceSystem.Tethys.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        OceanusDBConfiguration dbConfiguration)
    {
        return services
            .AddDbContext<DbContext, TethysDBContext>(
                options => options
                    .UseSqlite(dbConfiguration.ConnectionString)
                    .UseLazyLoadingProxies());
    }
}