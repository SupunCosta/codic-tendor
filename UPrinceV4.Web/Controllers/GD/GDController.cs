using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.GD.Vehicle;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces.GD;

namespace UPrinceV4.Web.Controllers.GD;

[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class GDController : CommonConfigurationController
{
    private readonly IGDRepository _GDRepository;

    public GDController(ApplicationDbContext uPrinceCustomerContext,
        IHttpContextAccessor contextAccessor, ApiResponse apiResponse, ApiBadRequestResponse apiBadRequestResponse,
        ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider, IGDRepository _GDRepositoryX
    ) : base(uPrinceCustomerContext, contextAccessor, apiResponse,
        apiBadRequestResponse, apiOkResponse, iTenantProvider)
    {
        _iTenantProvider = iTenantProvider;
        _GDRepository = _GDRepositoryX;
    }

    public ITenantProvider _iTenantProvider { get; }


    [AllowAnonymous]
    [HttpGet("GeoDynmicLogin")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GeoDynmicLogin()
    {
        return Ok(new ApiOkResponse(_GDRepository.CreateToken(new GDParameter())));
    }

    [AllowAnonymous]
    [HttpGet("GetVehicles")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetVehicles()
    {
        return Ok(new ApiOkResponse(_GDRepository.GetVehicles(new GDParameter())));
    }

    [AllowAnonymous]
    [HttpPost("GetVehiclePosition")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetVehiclePosition([FromBody] VehiclePositionDto vehiclePositionDto)
    {
        var gdParameter = new GDParameter
        {
            VehiclePositionDto = vehiclePositionDto
        };
        return Ok(new ApiOkResponse(_GDRepository.GetVehiclePosition(gdParameter)));
    }
}