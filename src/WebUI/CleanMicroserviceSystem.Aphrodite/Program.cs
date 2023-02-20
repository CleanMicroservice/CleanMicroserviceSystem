using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using CleanMicroserviceSystem.Aphrodite;
using CleanMicroserviceSystem.Aphrodite.Domain;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
var env = builder.HostEnvironment;
var config = builder.Configuration;

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMasaBlazor();
builder.Services.AddHttpClient(ApiContract.AphroditeHTTPServiceName, client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

// Supply HttpClient instances that include access tokens when making requests to the server project
// builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("CleanMicroserviceSystem.Aphrodite.ServerAPI"));

await builder.Build().RunAsync();
