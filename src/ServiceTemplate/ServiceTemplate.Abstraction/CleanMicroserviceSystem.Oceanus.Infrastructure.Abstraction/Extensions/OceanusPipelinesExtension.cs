using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Extensions;

public static class OceanusPipelinesExtension
{
    public static IApplicationBuilder UseOceanusPipelines(this IApplicationBuilder app)
    {
        app.UseMiddleware<WebAPILoggingMiddleware>();
        return app;
    }
}
