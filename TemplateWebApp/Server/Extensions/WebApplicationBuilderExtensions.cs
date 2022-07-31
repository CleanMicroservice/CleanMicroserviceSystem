using NLog.Web;

namespace TemplateWebApp.Server.Extensions;

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
        return builder;
    }
}
