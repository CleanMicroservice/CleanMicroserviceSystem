using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace CleanMicroserviceSystem.Oceanus.Application.Abstraction.Attributes;
public class WebAPILogActionFilterAttribute : ActionFilterAttribute
{
    public const string NoLogResponseBodyFlag = "NoLogResponseBody";
    public const string NoLogRequestBodyFlag = "NoLogRequestBody";

    private readonly StringValues TureHeaderValue = new(bool.TrueString);
    private readonly bool noLogRequestBody;
    private readonly bool noLogResponseBody;

    public WebAPILogActionFilterAttribute(bool noLogRequestBody = false, bool noLogResponseBody = false)
    {
        this.noLogRequestBody = noLogRequestBody;
        this.noLogResponseBody = noLogResponseBody;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (this.noLogRequestBody) context.HttpContext.Request.Headers.Add(NoLogRequestBodyFlag, this.TureHeaderValue);
        if (this.noLogResponseBody) context.HttpContext.Response.Headers.Add(NoLogResponseBodyFlag, this.TureHeaderValue);
        await base.OnActionExecutionAsync(context, next);
    }
}
