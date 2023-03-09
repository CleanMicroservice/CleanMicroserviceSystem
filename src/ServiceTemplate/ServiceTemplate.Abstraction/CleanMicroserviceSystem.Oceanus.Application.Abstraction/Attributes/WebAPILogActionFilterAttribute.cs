using Microsoft.AspNetCore.Mvc.Filters;

namespace CleanMicroserviceSystem.Oceanus.Application.Abstraction.Attributes;
public class WebAPILogActionFilterAttribute : ActionFilterAttribute
{
    private readonly bool logRequestBody;
    private readonly bool logResponseBody;
    public const string LogRequestBodyKey = "LogRequestBody";
    public const string LogResponseBodyKey = "LogResponseBody";

    public WebAPILogActionFilterAttribute(bool logRequestBody = true, bool logResponseBody = true)
    {
        this.logRequestBody = logRequestBody;
        this.logResponseBody = logResponseBody;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        context.HttpContext.Items[LogRequestBodyKey] = this.logRequestBody;
        context.HttpContext.Items[LogResponseBodyKey] = this.logResponseBody;
        await base.OnActionExecutionAsync(context, next);
    }
}
