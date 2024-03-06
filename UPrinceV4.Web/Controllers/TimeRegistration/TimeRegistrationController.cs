using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.CPC;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Repositories.Interfaces.GD;
using UPrinceV4.Web.Repositories.Interfaces.HR;
using UPrinceV4.Web.Repositories.Interfaces.PMOL;

namespace UPrinceV4.Web.Controllers.TimeRegistration;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class TimeRegistrationController : CommonConfigurationController
{
    private readonly IGDRepository _iGDRepository;
    private readonly IHRRepository _iHRRepository;
    private readonly IPmolRepository _iPmolRepository;

    private readonly ITimeRegistrationRepository _iTimeRegistrationRepository;
    private readonly IVPRepository _iVPRepository;
    private readonly ITenantProvider _TenantProvider;


    public TimeRegistrationController(ITenantProvider tenantProvider,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider
        , IHRRepository iHRRepository, IVPRepository iVPRepository, IPmolRepository iPmolRepository,
        ITimeRegistrationRepository iTimeRegistrationRepository, IGDRepository iGDRepository)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)

    {
        _iTimeRegistrationRepository = iTimeRegistrationRepository;
        _iHRRepository = iHRRepository;
        _TenantProvider = tenantProvider;
        _iVPRepository = iVPRepository;
        _iPmolRepository = iPmolRepository;
        _iGDRepository = iGDRepository;
    }

    [HttpPost("GetLabourPmolVehicalesPositions")]
    public async Task<ActionResult> GetLabourPmolVehicalesPositions([FromBody] GetLabourPmolVehicalesPositionsDto dto,
        [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var TimeRegistrationParameter = new TimeRegistrationParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                GetLabourPmolVehicalesPositionsDto = dto,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                IVPRepository = _iVPRepository,
                IPmolRepository = _iPmolRepository,
                IGDRepository = _iGDRepository
            };

            return Ok(new ApiOkResponse(
                await _iTimeRegistrationRepository.GetLabourPmolVehicalesPositions(TimeRegistrationParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("GetVtsDataByPerson")]
    public async Task<ActionResult> GetVtsDataByPerson([FromBody] GetLabourPmolVehicalesPositionsDto dto,
        [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var TimeRegistrationParameter = new TimeRegistrationParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                GetLabourPmolVehicalesPositionsDto = dto,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                IVPRepository = _iVPRepository,
                IPmolRepository = _iPmolRepository,
                IGDRepository = _iGDRepository
            };

            return Ok(new ApiOkResponse(
                await _iTimeRegistrationRepository.GetVtsDataByPerson(TimeRegistrationParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetVehicles")]
    public async Task<ActionResult> GetVehicles([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var TimeRegistrationParameter = new TimeRegistrationParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                IVPRepository = _iVPRepository,
                IPmolRepository = _iPmolRepository,
                IGDRepository = _iGDRepository
            };

            return Ok(new ApiOkResponse(await _iTimeRegistrationRepository.GetVehicles(TimeRegistrationParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}