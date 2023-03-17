using CleanMicroserviceSystem.Aphrodite;
using CleanMicroserviceSystem.Aphrodite.Application.Configurations;
using CleanMicroserviceSystem.Aphrodite.Infrastructure;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
var env = builder.HostEnvironment;
var config = builder.Configuration;

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var gatewayConfiguration = config.GetRequiredSection("GatewayAPIConfiguration").Get<GatewayAPIConfiguration>();
var nuGetServerConfiguration = config.GetRequiredSection("NuGetServerConfiguration")!.Get<NuGetServerConfiguration>()!;
var aphroditeConfiguration = new AphroditeConfiguration()
{
    WebUIBaseAddress = builder.HostEnvironment.BaseAddress,
    GatewayBaseAddress = gatewayConfiguration!.GatewayBaseAddress,
};
builder.Services.ConfigureServices(
    aphroditeConfiguration,
    nuGetServerConfiguration);

await builder.Build().RunAsync();
