using CleanMicroserviceSystem.Common.Contracts;
using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CleanMicroserviceSystem.Themis.WebAPI.Controllers;

[Authorize(Policy = IdentityContract.AccessRolesPolicy)]
[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly ILogger<RoleController> logger;
    private readonly UserManager<OceanusUser> userManager;
    private readonly RoleManager<OceanusRole> roleManager;

    public RoleController(
        ILogger<RoleController> logger,
        UserManager<OceanusUser> userManager,
        RoleManager<OceanusRole> roleManager)
    {
        this.logger = logger;
        this.userManager = userManager;
        this.roleManager = roleManager;
    }

    #region Roles

    #endregion

    #region RoleClaims

    #endregion

    #region RoleUsers

    #endregion
}
