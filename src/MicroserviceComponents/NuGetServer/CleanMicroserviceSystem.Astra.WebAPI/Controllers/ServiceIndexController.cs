using CleanMicroserviceSystem.Astra.Contract.NuGetPackages;
using CleanMicroserviceSystem.Astra.Domain;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.ServiceIndex;
using Microsoft.AspNetCore.Mvc;

namespace CleanMicroserviceSystem.Astra.WebAPI.Controllers;

[ApiController]
[Route("api/v3")]
public class ServiceIndexController : ControllerBase
{
    private readonly IServiceIndexService _serviceIndex;

    public ServiceIndexController(IServiceIndexService serviceIndex)
    {
        this._serviceIndex = serviceIndex ?? throw new ArgumentNullException(nameof(serviceIndex));
    }

    [HttpGet]
    [Route("index.json", Name = NuGetRouteContract.IndexRouteName)]
    public async Task<ServiceIndexResponse> GetAsync(CancellationToken cancellationToken)
    {
        return await this._serviceIndex.GetAsync(cancellationToken);
    }
}
