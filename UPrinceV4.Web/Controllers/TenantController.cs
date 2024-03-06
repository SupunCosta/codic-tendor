using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UPrinceV4.Shared;
using UPrinceV4.Web.Models;

namespace UPrinceV4.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TenantController : ControllerBase
{
    private readonly ITenantProvider _TenantProvider;

    public TenantController(ITenantProvider tenantProvider)
    {
        _TenantProvider = tenantProvider;
    }

    [HttpGet("tenant")]
    public async Task<ActionResult> tenant()
    {
        try
        {
            return Ok(new ApiOkResponse(_TenantProvider.GetTenantDto(), "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}