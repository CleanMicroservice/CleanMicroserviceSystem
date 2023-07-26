using CleanMicroserviceSystem.Themis.NTLMWeb.Configurations;
using CleanMicroserviceSystem.Themis.Client;
using Microsoft.AspNetCore.Authentication.Negotiate;
using NLog;
using NLog.Web;
using MSLoggingLevel = Microsoft.Extensions.Logging.LogLevel;
using CleanMicroserviceSystem.Authentication.Application;

const string GatewayAPIConfigurationKey = "GatewayAPIConfiguration";

var assemblyName = typeof(Program).Assembly.GetName();
var setupInformation = AppDomain.CurrentDomain.SetupInformation;
var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Info($"{assemblyName.Name} launches, TargetFrameworkName={setupInformation.TargetFrameworkName}, Version={assemblyName.Version}");

var builder = WebApplication.CreateBuilder(args);
builder.Logging
    .ClearProviders()
    .AddConsole()
    .AddDebug()
    .SetMinimumLevel(MSLoggingLevel.Debug)
    .AddNLogWeb();
logger.Info($"{nameof(builder.Environment.EnvironmentName)}={builder.Environment.EnvironmentName}");

try
{
    builder.Services
        .Configure<GatewayAPIConfiguration>(options => builder.Configuration.GetRequiredSection(GatewayAPIConfigurationKey).Bind(options))
        .AddSingleton<IAuthenticationTokenStore, DefaultAuthenticationTokenStore>()
        .AddTransient<DefaultAuthenticationDelegatingHandler>()
        .AddHttpClient()
        .AddThemisClients(options =>
        {
            options.GatewayClientName = ApiContract.GatewayHttpClientName;
        })
        .AddHttpClient<HttpClient>(
            ApiContract.GatewayHttpClientName,
            client => client.BaseAddress = new Uri(builder.Configuration.GetRequiredSection(GatewayAPIConfigurationKey).Get<GatewayAPIConfiguration>()!.GatewayBaseAddress))
        .AddHttpMessageHandler<DefaultAuthenticationDelegatingHandler>();
    builder.Services
        .AddAuthorization(options =>
        {
            options.FallbackPolicy = options.DefaultPolicy;
        })
        .AddAuthentication(NegotiateDefaults.AuthenticationScheme)
        .AddNegotiate();
    builder.Services
        .AddRazorPages();

    var app = builder.Build();
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthorization();
    app.MapRazorPages();
    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, $"{assemblyName.Name} launches failed.");
}
finally
{
    logger.Info($"{assemblyName.Name} shutdown, Version={assemblyName.Version}");
    LogManager.Shutdown();
}
