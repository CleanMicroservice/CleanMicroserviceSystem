using Microsoft.AspNetCore.HttpLogging;
using NLog.Web;

namespace CleanMicroserviceSystem.Aphrodite.Host.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder ConfigurateLog(this WebApplicationBuilder builder)
    {
        builder.Logging
            .ClearProviders()
            .AddConsole()
            .AddDebug()
            .SetMinimumLevel(LogLevel.Trace)
            .AddNLogWeb();
        builder.Services
            .AddLogging()
            .AddHttpLogging(options =>
            {
                options.LoggingFields = HttpLoggingFields.All;
            });
        return builder;
    }
}
