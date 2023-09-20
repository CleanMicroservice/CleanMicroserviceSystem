using System.Diagnostics;
using CleanMicroserviceSystem.Oceanus.Application.Abstraction.Attributes;
using CleanMicroserviceSystem.Oceanus.Application.Abstraction.Repository;
using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IO;

namespace CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Middlewares;
public class WebAPILoggingMiddleware
{
    private readonly ILogger<WebAPILoggingMiddleware> logger;
    private readonly IWebAPILogRepository webAPILogRepository;
    private readonly RecyclableMemoryStreamManager recyclableMemoryStreamManager;
    private readonly RequestDelegate next;

    public WebAPILoggingMiddleware(
        ILogger<WebAPILoggingMiddleware> logger,
        IServiceScopeFactory serviceScopeFactory,
        RequestDelegate next)
    {
        this.logger = logger;
        var serviceScope = serviceScopeFactory.CreateScope();
        this.webAPILogRepository = serviceScope.ServiceProvider.GetRequiredService<IWebAPILogRepository>();
        this.recyclableMemoryStreamManager = serviceScope.ServiceProvider.GetRequiredService<RecyclableMemoryStreamManager>();
        this.next = next ?? throw new ArgumentNullException(nameof(next));
    }

    public async Task Invoke(HttpContext context)
    {
        var watcher = new Stopwatch();
        watcher.Start();

        var webAPILog = new WebAPILog
        {
            RequestURI = context.Request.Path.ToUriComponent(),
            QueryString = context.Request.QueryString.ToUriComponent(),
            Method = context.Request.Method,
            SourceHost = $"{context.Connection.RemoteIpAddress}:{context.Connection.RemotePort}",
            UserAgent = context.Request.Headers.TryGetValue("User-Agent", out var userAgent) ? userAgent : string.Empty,
            TraceIdentifier = context.TraceIdentifier,
            CreatedOn = DateTime.Now
        };

        try
        {
            var originalResponseStream = context.Response.Body;
            await using var responseBodyStream = this.recyclableMemoryStreamManager.GetStream();
            context.Response.Body = responseBodyStream;

            context.Request.EnableBuffering();
            await this.next(context);

            try
            {
                if (!context.Items.TryGetValue(WebAPILogActionFilterAttribute.LogRequestBodyKey, out var logRequestBody) ||
                    logRequestBody is true)
                {
                    context.Request.Body.Seek(0, SeekOrigin.Begin);
                    await using var requestStream = this.recyclableMemoryStreamManager.GetStream();
                    await context.Request.Body.CopyToAsync(requestStream);
                    using var requestStreamReader = new StreamReader(requestStream);
                    requestStream.Seek(0, SeekOrigin.Begin);
                    var requestBody = await requestStreamReader.ReadToEndAsync();
                    webAPILog.RequestBody = string.IsNullOrEmpty(requestBody) ? default : requestBody;
                }
            }
            catch (Exception ex)
            {
                this.logger.LogWarning(ex, $"Failed to read request body of {context.TraceIdentifier}.");
                webAPILog.RequestBody = ex.Message;
            }

            try
            {
                if (!context.Items.TryGetValue(WebAPILogActionFilterAttribute.LogResponseBodyKey, out var logResponseBody) ||
                    logResponseBody is true)
                {
                    // Stream will be disposed together with Reader automatically.
                    using var responseStreamReader = new StreamReader(responseBodyStream);
                    responseBodyStream.Seek(0, SeekOrigin.Begin);
                    var responseBody = await responseStreamReader.ReadToEndAsync();
                    responseBodyStream.Seek(0, SeekOrigin.Begin);
                    webAPILog.ResponseBody = string.IsNullOrEmpty(responseBody) ? default : responseBody;
                    await responseBodyStream.CopyToAsync(originalResponseStream);
                }
                else
                {
                    responseBodyStream.Seek(0, SeekOrigin.Begin);
                    await responseBodyStream.CopyToAsync(originalResponseStream);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogWarning(ex, $"Failed to read response body of {context.TraceIdentifier}.");
                webAPILog.ResponseBody = ex.Message;
            }

            webAPILog.IsAuthenticated = context.User.Identity?.IsAuthenticated ?? false;
            webAPILog.IdentityName = context.User.Identity?.Name ?? string.Empty;
            webAPILog.StatusCode = context.Response.StatusCode;
        }
        catch (Exception ex)
        {
            webAPILog.Exception = ex.ToString();
            throw;
        }
        finally
        {
            watcher.Stop();
            webAPILog.LastModifiedOn = DateTime.Now;
            webAPILog.ElapsedTime = watcher.ElapsedMilliseconds;

            try
            {
                lock (this.webAPILogRepository)
                {
                    this.webAPILogRepository.Add(webAPILog);
                }
            }
            catch (Exception logEx)
            {
                this.logger.LogError(logEx, $"Failed to record Web API Log of {webAPILog.IdentityName} from {webAPILog.SourceHost} in {webAPILog.ElapsedTime} ms: [{webAPILog.Method}]=>{webAPILog.RequestURI} [{webAPILog.StatusCode}]");
            }
            this.logger.LogInformation($"Web API of {webAPILog.IdentityName} from {webAPILog.SourceHost} in {webAPILog.ElapsedTime} ms: [{webAPILog.Method}]=>{webAPILog.RequestURI} [{webAPILog.StatusCode}]");
        }
    }
}
