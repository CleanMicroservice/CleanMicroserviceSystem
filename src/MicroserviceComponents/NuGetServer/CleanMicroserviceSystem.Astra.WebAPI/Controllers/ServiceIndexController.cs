using BaGet.Core;
using BaGet.Protocol.Models;
using Microsoft.AspNetCore.Mvc;

namespace CleanMicroserviceSystem.Astra.WebAPI.Controllers;

[ApiController]
public class ServiceIndexController : ControllerBase
{
    private readonly IServiceIndexService _serviceIndex;

    public ServiceIndexController(IServiceIndexService serviceIndex)
    {
        this._serviceIndex = serviceIndex ?? throw new ArgumentNullException(nameof(serviceIndex));
    }

    [HttpGet]
    [Route("v3/index.json")]
    public async Task<ServiceIndexResponse> GetAsync(CancellationToken cancellationToken)
    {
        return await this._serviceIndex.GetAsync(cancellationToken);
    }
}
