using CleanMicroserviceSystem.Aphrodite;
using CleanMicroserviceSystem.Aphrodite.Infrastructure;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
var env = builder.HostEnvironment;
var config = builder.Configuration;

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.ConfigureServices();

await builder.Build().RunAsync();
