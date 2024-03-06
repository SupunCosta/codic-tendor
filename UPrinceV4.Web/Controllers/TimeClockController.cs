using System;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.PMOL;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Repositories.Interfaces.BOR;
using UPrinceV4.Web.Repositories.Interfaces.PBS;
using UPrinceV4.Web.Repositories.Interfaces.PMOL;
using UPrinceV4.Web.Util;
using TimeZone = UPrinceV4.Web.Data.TimeZone;

namespace UPrinceV4.Web.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class TimeClockController : CommonConfigurationController
{
    private readonly IBorRepository _borRepository;
    private readonly IPbsResourceRepository _iPbsResourceRepository;
    private readonly IPmolRepository _iPmolRepository;
    private readonly IPmolResourceRepository _iPmolResourceRepository;
    private readonly ITimeClockRepository _iTimeClockRepository;
    private readonly IShiftRepository _iTShiftRepository;
    //private readonly PmolParameter _pmolParameter;


    public TimeClockController(ITimeClockRepository iTimeClockRepository,
        IShiftRepository iShiftRepository,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider,
        IPmolRepository iPmolRepository, 
        IBorRepository borRepository, IPmolResourceRepository iPmolResourceRepository,
        IPbsResourceRepository iPbsResourceRepository)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)
    {
        _iTimeClockRepository = iTimeClockRepository;
        _iTShiftRepository = iShiftRepository;
        _iPmolRepository = iPmolRepository;
       
        _borRepository = borRepository;
        _iPmolResourceRepository = iPmolResourceRepository;
        _iPbsResourceRepository = iPbsResourceRepository;
    }


    [HttpGet("GetTimeClock")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetTimeClock()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            var ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            var timeClock = await _iTimeClockRepository.GetTimeClock(UPrinceCustomerContext,
                ContractingUnitSequenceId, ProjectSequenceId, ItenantProvider);

            return Ok(!timeClock.Any()
                ? new ApiResponse(200, false, "noAvailableTimeClock")
                : new ApiOkResponse(timeClock));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }


    [HttpPost("ReadByDate")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetTimeClockByDate([FromBody] TimeZone timeZone)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            var ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            return Ok(new ApiOkResponse(await _iTimeClockRepository.GetTimeClockByDate(UPrinceCustomerContext,
                ContextAccessor, _iTShiftRepository, timeZone, lang, ContractingUnitSequenceId, ProjectSequenceId,
                ItenantProvider)));
        }
        catch (Exception ex)
        {
            if (ex.Message == "PleaseStartAShift")
            {
                var mApiOkResponse = new ApiOkResponse(null, "PleaseStartAShift");
                mApiOkResponse.Status = false;
                return Ok(mApiOkResponse);
            }

            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpGet("GetTimeClockById/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetTimeClockById(string id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            var ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            //string message = ApiErrorMessages.GetErrorMessage(ItenantProvider, ErrorMessageKey.Ok, lang).Message;
            var timeClock = await _iTimeClockRepository.GetTimeClockById(UPrinceCustomerContext, id,
                ContractingUnitSequenceId, ProjectSequenceId, ItenantProvider);

            return Ok(timeClock == null
                ? new ApiResponse(200, false,
                    ApiErrorMessages
                        .GetErrorMessage(ItenantProvider, ErrorMessageKey.NoAvailableTimeClockForTheId, lang)
                        .Message)
                : new ApiOkResponse(timeClock));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }


    [HttpGet("ReadByShiftId")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetTimeClockByShiftId(string shiftId)
    {
        try
        {
            var ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            var ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            var timeClock = await _iTimeClockRepository.GetTimeClockByShiftId(UPrinceCustomerContext, shiftId,
                ContractingUnitSequenceId, ProjectSequenceId, ItenantProvider);

            return Ok(!timeClock.Any()
                ? new ApiResponse(200, false, "noAvailableTimeClockForTheShift")
                : new ApiOkResponse(timeClock));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.Message));
        }
    }


    [HttpGet("Page/{pageNo}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetTimeClockPage(int pageNo)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            var ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();


            return Ok(new ApiOkResponse(await _iTimeClockRepository.GetTimeClockPagedResult<TimeClock>(
                UPrinceCustomerContext, pageNo, lang, ContractingUnitSequenceId, ProjectSequenceId,
                ItenantProvider)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("Create")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreateTimeClock(
        [FromBody] CreateTimeClockDto timeClockDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(new ApiBadRequestResponse(ModelState));

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            var ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();

            return Ok(new ApiOkResponse(await _iTimeClockRepository.CreateTimeClock(UPrinceCustomerContext,
                timeClockDto,
                ContextAccessor, lang, ContractingUnitSequenceId, ProjectSequenceId, ItenantProvider)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.Message));
        }
    }

    //[Microsoft.AspNetCore.Mvc.HttpPut("Update")]
    [HttpPut("Update")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> UpdateTimeClock(
        [FromBody] UpdateTimeClockDto timeClockDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            var ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            return Ok(new ApiOkResponse(await _iTimeClockRepository.UpdateTimeClock(UPrinceCustomerContext,
                timeClockDto, ContextAccessor, lang, ContractingUnitSequenceId, ProjectSequenceId,
                ItenantProvider)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("Delete")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult DeleteTimeClock(string id)
    {
        try
        {
            var ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            var ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _iTimeClockRepository.DeleteTimeClock(UPrinceCustomerContext, id, ContractingUnitSequenceId,
                ProjectSequenceId, ItenantProvider);
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = false;
            ApiResponse.Message = "Time Clock deleted successfully";
            return Ok(ApiResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }


    [HttpPost("ReadLaborCalculation")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> ReadLaborCalculation(
        [FromBody] CreateTimeClockDto timeClockDto)
    {
        var lang = langCode(Request.Headers["lang"].FirstOrDefault());
        var ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
        var ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
        var _pmolParameter = new PmolParameter();
        _pmolParameter.ContractingUnitSequenceId = ContractingUnitSequenceId;
        _pmolParameter.ProjectSequenceId = ProjectSequenceId;
        _pmolParameter.Lang = lang;
        _pmolParameter.Id = timeClockDto.PmolId;
        _pmolParameter.ContextAccessor = ContextAccessor;
        _pmolParameter.TenantProvider = ItenantProvider;
        await _iPmolRepository.ReadLaborCalculation(_pmolParameter);
        return Ok(new ApiOkResponse("s"));
    }

    [HttpPost("CreatePmolTimeClock")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreatePmolTimeClock(
        [FromBody] CreateTimeClockDto timeClockDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(new ApiBadRequestResponse(ModelState));

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            var ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            var _pmolParameter = new PmolParameter();
            //string message = ApiErrorMessages.GetErrorMessage(ItenantProvider, ErrorMessageKey.Ok, lang).Message;
            var id = await _iTimeClockRepository.CreateTimeClock(UPrinceCustomerContext, timeClockDto,
                ContextAccessor, lang, ContractingUnitSequenceId, ProjectSequenceId, ItenantProvider);


            if (timeClockDto.Type == 0 || timeClockDto.Type == 1 || timeClockDto.Type == 2)
            {
                if (timeClockDto.IsBreak == false)
                {
                    _pmolParameter.ContractingUnitSequenceId = ContractingUnitSequenceId;
                    _pmolParameter.ProjectSequenceId = ProjectSequenceId;
                    _pmolParameter.Lang = lang;
                    _pmolParameter.Id = timeClockDto.PmolId;
                    _pmolParameter.ContextAccessor = ContextAccessor;
                    _pmolParameter.TenantProvider = ItenantProvider;
                    await _iPmolRepository.UpdateStartTime(_pmolParameter);

                    var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims
                        .First(claim =>
                            claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")
                        .Value;
                    //var user = UPrinceCustomerContext.ApplicationUser.Where(u => u.OId == objectIdentifier).FirstOrDefault();
                    _pmolParameter.UserId = objectIdentifier;
                    var pmolList =
                        await _iPmolRepository.GetPmolByUserId(_pmolParameter);
                    var toBeStoppedPmol = pmolList
                        .Where(p => p.StartDateTime != null && p.EndDateTime == null).FirstOrDefault();
                    if (toBeStoppedPmol != null && toBeStoppedPmol.Id != timeClockDto.PmolId)
                    {
                        _pmolParameter.Id = toBeStoppedPmol.Id;
                        _pmolParameter.ProjectSequenceId = toBeStoppedPmol.ProjectDefinition.SequenceCode;
                        await _iPmolRepository.UpdateEndTime(_pmolParameter);
                    }
                }
                else //if is break
                {
                    _pmolParameter.Id = timeClockDto.PmolId;

                    await _iPmolRepository.BreakPmolStop(_pmolParameter);
                }
            }
            else if (timeClockDto.Type == 5)
            {
                _pmolParameter.ContractingUnitSequenceId = ContractingUnitSequenceId;
                // _pmolParameter.ProjectSequenceId = ProjectSequenceId;
                _pmolParameter.Lang = lang;
                //_pmolParameter.Id = timeClockDto.PmolId;
                var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

                _pmolParameter.UserId = objectIdentifier;
                _pmolParameter.ContextAccessor = ContextAccessor;
                _pmolParameter.TenantProvider = ItenantProvider;
                var pmolList =
                    await _iPmolRepository.GetPmolByUserId(_pmolParameter);
                var toBeStoppedPmol =
                    pmolList.Where(p => p.StartDateTime != null && p.EndDateTime == null);
                foreach (var pmol in toBeStoppedPmol)
                {
                    _pmolParameter.Id = pmol.Id;
                    _pmolParameter.ContractingUnitSequenceId = ContractingUnitSequenceId;
                    _pmolParameter.ProjectSequenceId = pmol.ProjectDefinition.SequenceCode;
                    _pmolParameter.Lang = lang;
                    //_pmolParameter.Id = timeClockDto.PmolId;
                    _pmolParameter.ContextAccessor = ContextAccessor;
                    _pmolParameter.TenantProvider = ItenantProvider;
                    await _iPmolRepository.UpdateEndTime(_pmolParameter);
                    _pmolParameter.Id = pmol.ProjectMoleculeId;
                    _pmolParameter.borRepository = _borRepository;
                    _pmolParameter.IPmolResourceRepository = _iPmolResourceRepository;
                    var newID = await _iPmolRepository.ClonePmol(_pmolParameter);
                    return Ok(new ApiOkResponse(newID));
                }
            }
            else if (timeClockDto.Type == 6)
            {
                // IEnumerable<PmolListDtoForMobile> pmolList = await _iPmolRepository.GetPmolByUserId(_pmolParameter);
                //IEnumerable<PmolListDtoForMobile> toBeStoppedPmol = pmolList.Where(p => p.StartDateTime != null && p.EndDateTime == null).FirstOrDefault();
                _pmolParameter.Id = timeClockDto.PmolId;

                await _iPmolRepository.BreakPmol(_pmolParameter);
            }

            _pmolParameter.Id = timeClockDto.PmolId;
            _pmolParameter.ContractingUnitSequenceId = ContractingUnitSequenceId;
            _pmolParameter.ProjectSequenceId = ProjectSequenceId;
            _pmolParameter.Lang = lang;
            var userId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            _pmolParameter.UserId = userId;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = ItenantProvider;

            var isForeman = await _iPmolRepository.IsForeman(_pmolParameter);
            timeClockDto.IsForeman = isForeman;

            return Ok(new ApiOkResponse(timeClockDto));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("CreatePmolTimeClockForMobile")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreatePmolTimeClockForMobile(
        [FromBody] CreateTimeClockDto timeClockDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(new ApiBadRequestResponse(ModelState));

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            var ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            var _pmolParameter = new PmolParameter();
            //string message = ApiErrorMessages.GetErrorMessage(ItenantProvider, ErrorMessageKey.Ok, lang).Message;
            var id = await _iTimeClockRepository.CreateTimeClockForAll(UPrinceCustomerContext, timeClockDto,
                ContextAccessor, lang, ContractingUnitSequenceId, ProjectSequenceId, ItenantProvider);

            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            var isForeman = timeClockDto.IsForeman;

            if (isForeman)
            {
                if (timeClockDto.Type == 0 || timeClockDto.Type == 1 || timeClockDto.Type == 2)
                {
                    if (timeClockDto.IsBreak == false)
                    {
                        _pmolParameter.ContractingUnitSequenceId = ContractingUnitSequenceId;
                        _pmolParameter.ProjectSequenceId = ProjectSequenceId;
                        _pmolParameter.Lang = lang;
                        _pmolParameter.Id = timeClockDto.PmolId;
                        _pmolParameter.ContextAccessor = ContextAccessor;
                        _pmolParameter.TenantProvider = ItenantProvider;
                        await _iPmolRepository.UpdateStartTime(_pmolParameter);
                        _pmolParameter.UserId = objectIdentifier;
                        var pmolList =
                            await _iPmolRepository.GetPmolByUserId(_pmolParameter);
                        var toBeStoppedPmol = pmolList
                            .Where(p => p.StartDateTime != null && p.EndDateTime == null).FirstOrDefault();
                        if (toBeStoppedPmol != null && toBeStoppedPmol.Id != timeClockDto.PmolId)
                        {
                            _pmolParameter.Id = toBeStoppedPmol.Id;
                            _pmolParameter.ProjectSequenceId = toBeStoppedPmol.ProjectDefinition.SequenceCode;
                            await _iPmolRepository.UpdateEndTimeByForeman(_pmolParameter);
                        }
                    }
                    else //if is break
                    {
                        _pmolParameter.Id = timeClockDto.PmolId;

                        await _iPmolRepository.BreakPmolStop(_pmolParameter);
                    }
                }
                else if (timeClockDto.Type == 5)
                {
                    _pmolParameter.ContractingUnitSequenceId = ContractingUnitSequenceId;
                    _pmolParameter.Lang = lang;
                    _pmolParameter.UserId = objectIdentifier;
                    _pmolParameter.ContextAccessor = ContextAccessor;
                    _pmolParameter.TenantProvider = ItenantProvider;
                    var pmolList =
                        await _iPmolRepository.GetPmolByUserId(_pmolParameter);
                    var toBeStoppedPmol =
                        pmolList.Where(p => p.StartDateTime != null && p.EndDateTime == null);
                    foreach (var pmol in toBeStoppedPmol)
                    {
                        _pmolParameter.Id = pmol.Id;
                        _pmolParameter.ContractingUnitSequenceId = ContractingUnitSequenceId;
                        _pmolParameter.ProjectSequenceId = pmol.ProjectDefinition.SequenceCode;
                        _pmolParameter.Lang = lang;
                        _pmolParameter.ContextAccessor = ContextAccessor;
                        _pmolParameter.TenantProvider = ItenantProvider;
                        await _iPmolRepository.UpdateEndTimeByForeman(_pmolParameter);
                        _pmolParameter.Id = pmol.ProjectMoleculeId;
                        _pmolParameter.borRepository = _borRepository;
                        _pmolParameter.IPmolResourceRepository = _iPmolResourceRepository;
                        var newID = await _iPmolRepository.ClonePmol(_pmolParameter);
                        return Ok(new ApiOkResponse(newID));
                    }
                }
                else if (timeClockDto.Type == 6)
                {
                    _pmolParameter.Id = timeClockDto.PmolId;

                    await _iPmolRepository.BreakPmol(_pmolParameter);
                }


                return Ok(new ApiOkResponse(timeClockDto));
            }

            if (timeClockDto.Type == 0 || timeClockDto.Type == 1 || timeClockDto.Type == 2)
            {
                if (timeClockDto.IsBreak == false)
                {
                    _pmolParameter.ContractingUnitSequenceId = ContractingUnitSequenceId;
                    _pmolParameter.ProjectSequenceId = ProjectSequenceId;
                    _pmolParameter.Lang = lang;
                    _pmolParameter.Id = timeClockDto.PmolId;
                    _pmolParameter.ContextAccessor = ContextAccessor;
                    _pmolParameter.TenantProvider = ItenantProvider;
                    _pmolParameter.UserId = objectIdentifier;
                    await _iPmolRepository.UpdateLabourStartTime(_pmolParameter);

                    var pmolList =
                        await _iPmolRepository.GetPmolByUserIdOfLabour(_pmolParameter);
                    var toBeStoppedPmol = pmolList
                        .Where(p => p.StartDateTime != null && p.EndDateTime == null).FirstOrDefault();
                    if (toBeStoppedPmol != null && toBeStoppedPmol.Id != timeClockDto.PmolId)
                    {
                        _pmolParameter.Id = toBeStoppedPmol.Id;
                        _pmolParameter.ProjectSequenceId = toBeStoppedPmol.ProjectDefinition.SequenceCode;
                        await _iPmolRepository.UpdateLabourpmolEndTime(_pmolParameter);

                        return Ok(new ApiOkResponse(timeClockDto));
                    }
                }
                else //if is break
                {
                    _pmolParameter.Id = timeClockDto.PmolId;

                    await _iPmolRepository.BreakLabourStop(_pmolParameter);
                }
            }
            else if (timeClockDto.Type == 5)
            {
                _pmolParameter.ContractingUnitSequenceId = ContractingUnitSequenceId;
                // _pmolParameter.ProjectSequenceId = ProjectSequenceId;
                _pmolParameter.Lang = lang;
                //_pmolParameter.Id = timeClockDto.PmolId;
                //var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                //    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
                //var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                //claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

                _pmolParameter.UserId = objectIdentifier;
                _pmolParameter.ContextAccessor = ContextAccessor;
                _pmolParameter.TenantProvider = ItenantProvider;
                var pmolList =
                    await _iPmolRepository.GetPmolByUserIdOfLabour(_pmolParameter);
                var toBeStoppedPmol =
                    pmolList.Where(p => p.StartDateTime != null && p.EndDateTime == null);
                foreach (var pmol in toBeStoppedPmol)
                {
                    _pmolParameter.Id = pmol.Id;
                    _pmolParameter.ContractingUnitSequenceId = ContractingUnitSequenceId;
                    _pmolParameter.ProjectSequenceId = pmol.ProjectDefinition.SequenceCode;
                    _pmolParameter.Lang = lang;
                    //_pmolParameter.Id = timeClockDto.PmolId;
                    _pmolParameter.ContextAccessor = ContextAccessor;
                    _pmolParameter.TenantProvider = ItenantProvider;
                    var LabourStoppedPmol = await _iPmolRepository.UpdateLabourpmolEndTime(_pmolParameter);
                    return Ok(new ApiOkResponse(LabourStoppedPmol));
                }
            }
            else if (timeClockDto.Type == 6)
            {
                // IEnumerable<PmolListDtoForMobile> pmolList = await _iPmolRepository.GetPmolByUserId(_pmolParameter);
                //IEnumerable<PmolListDtoForMobile> toBeStoppedPmol = pmolList.Where(p => p.StartDateTime != null && p.EndDateTime == null).FirstOrDefault();
                _pmolParameter.ContractingUnitSequenceId = ContractingUnitSequenceId;
                _pmolParameter.ProjectSequenceId = ProjectSequenceId;
                _pmolParameter.TenantProvider = ItenantProvider;
                _pmolParameter.UserId = objectIdentifier;
                _pmolParameter.Id = timeClockDto.PmolId;

                await _iPmolRepository.BreakLabour(_pmolParameter);
            }


            return Ok(new ApiOkResponse(timeClockDto));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("UpdatePmolStart")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> UpdatePmolStart(
        [FromBody] PmolLabours PmolLabours)
    {
        var lang = langCode(Request.Headers["lang"].FirstOrDefault());
        var ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
        var ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
        var _pmolParameter = new PmolParameter();

        var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
            claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

        _pmolParameter.UserId = objectIdentifier;
        _pmolParameter.ContractingUnitSequenceId = ContractingUnitSequenceId;
        _pmolParameter.ProjectSequenceId = ProjectSequenceId;
        _pmolParameter.Lang = lang;
        _pmolParameter.PmolLabours = PmolLabours;
        _pmolParameter.ContextAccessor = ContextAccessor;
        _pmolParameter.TenantProvider = ItenantProvider;
        await _iPmolRepository.UpdatePmolStart(_pmolParameter);
        return Ok(new ApiOkResponse("s"));
    }

    [HttpPost("UpdatePmolStop")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> UpdatePmolStop(
        [FromBody] PmolLabours PmolLabours)
    {
        var lang = langCode(Request.Headers["lang"].FirstOrDefault());
        var ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
        var ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
        var _pmolParameter = new PmolParameter();

        var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
            claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

        _pmolParameter.UserId = objectIdentifier;
        _pmolParameter.ContractingUnitSequenceId = ContractingUnitSequenceId;
        _pmolParameter.ProjectSequenceId = ProjectSequenceId;
        _pmolParameter.Lang = lang;
        _pmolParameter.PmolLabours = PmolLabours;
        _pmolParameter.ContextAccessor = ContextAccessor;
        _pmolParameter.TenantProvider = ItenantProvider;
        await _iPmolRepository.UpdatePmolStop(_pmolParameter);
        return Ok(new ApiOkResponse("s"));
    }


    [HttpPost("CreatePmolTimeClockNew")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreatePmolTimeClockNew(
        [FromBody] CreateTimeClockDto timeClockDto)
    {
        var _log4net = LogManager.GetLogger(typeof(TimeClockController));
        var jsonString = JsonConvert.SerializeObject(timeClockDto, Formatting.Indented);

        try
        {
            if (!ModelState.IsValid) return BadRequest(new ApiBadRequestResponse(ModelState));

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            var ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            var _pmolParameter = new PmolParameter();
            timeClockDto.PbsResourceRepository = _iPbsResourceRepository;
            //string message = ApiErrorMessages.GetErrorMessage(ItenantProvider, ErrorMessageKey.Ok, lang).Message;
            await _iTimeClockRepository.CreateTimeClock(UPrinceCustomerContext, timeClockDto,
                ContextAccessor, lang, ContractingUnitSequenceId, ProjectSequenceId, ItenantProvider);


            if (timeClockDto.Type == 0 || timeClockDto.Type == 1 || timeClockDto.Type == 2)
            {
                if (timeClockDto.IsBreak == false)
                {
                    _pmolParameter.ContractingUnitSequenceId = ContractingUnitSequenceId;
                    _pmolParameter.ProjectSequenceId = ProjectSequenceId;
                    _pmolParameter.Lang = lang;
                    _pmolParameter.Id = timeClockDto.PmolId;
                    _pmolParameter.ContextAccessor = ContextAccessor;
                    _pmolParameter.TenantProvider = ItenantProvider;
                    await _iPmolRepository.UpdateStartTime(_pmolParameter);

                    var dto = new PmolLabours
                    {
                        PmolId = timeClockDto.PmolId,
                        Type = timeClockDto.Type.ToString(),
                        IsForeman = timeClockDto.IsForeman,
                        IsBreak = timeClockDto.IsBreak
                    };

                    _pmolParameter.PmolLabours = dto;

                    var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims
                        .First(claim =>
                            claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")
                        .Value;
                    //var user = UPrinceCustomerContext.ApplicationUser.Where(u => u.OId == objectIdentifier).FirstOrDefault();
                    _pmolParameter.UserId = objectIdentifier;

                    await _iPmolRepository.UpdatePmolStart(_pmolParameter);

                    var pmolList =
                        await _iPmolRepository.GetPmolByUserId(_pmolParameter);
                    // var toBeStoppedPmol = pmolList
                    //     .Where(p => p.StartDateTime != null && p.EndDateTime == null).FirstOrDefault();
                    var toBeStoppedPmol = pmolList
                        .Where(p => p.StartDateTime != null && p.IsEnded == false).FirstOrDefault();
                    if (toBeStoppedPmol != null && toBeStoppedPmol.Id != timeClockDto.PmolId)
                    {
                        _pmolParameter.Id = toBeStoppedPmol.Id;
                        _pmolParameter.ProjectSequenceId = toBeStoppedPmol.ProjectDefinition.SequenceCode;
                        await _iPmolRepository.UpdateEndTime(_pmolParameter);

                        await _iPmolRepository.UpdatePmolStop(_pmolParameter);
                    }
                }
                else //if is break
                {
                    _pmolParameter.Id = timeClockDto.PmolId;

                    await _iPmolRepository.BreakPmolStop(_pmolParameter);

                    _pmolParameter.ContractingUnitSequenceId = ContractingUnitSequenceId;
                    _pmolParameter.ProjectSequenceId = ProjectSequenceId;
                    _pmolParameter.Lang = lang;
                    _pmolParameter.ContextAccessor = ContextAccessor;
                    _pmolParameter.TenantProvider = ItenantProvider;

                    var dto = new PmolLabours
                    {
                        PmolId = timeClockDto.PmolId,
                        Type = timeClockDto.Type.ToString(),
                        IsForeman = timeClockDto.IsForeman,
                        IsBreak = timeClockDto.IsBreak
                    };

                    _pmolParameter.PmolLabours = dto;

                    var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims
                        .First(claim =>
                            claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")
                        .Value;
                    _pmolParameter.UserId = objectIdentifier;

                    await _iPmolRepository.UpdatePmolStart(_pmolParameter);
                }
            }
            else if (timeClockDto.Type == 5) //stop
            {
                _pmolParameter.ContractingUnitSequenceId = ContractingUnitSequenceId;
                // _pmolParameter.ProjectSequenceId = ProjectSequenceId;
                _pmolParameter.Lang = lang;
                //_pmolParameter.Id = timeClockDto.PmolId;
                var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

                _pmolParameter.UserId = objectIdentifier;
                _pmolParameter.ContextAccessor = ContextAccessor;
                _pmolParameter.TenantProvider = ItenantProvider;
                var pmolList =
                    await _iPmolRepository.GetPmolByUserId(_pmolParameter);
                // var toBeStoppedPmol =
                //     pmolList.Where(p => p.StartDateTime != null && p.EndDateTime == null);
                var toBeStoppedPmol =
                    pmolList.Where(p => p.StartDateTime != null && p.IsEnded == false);
                foreach (var pmol in toBeStoppedPmol)
                {
                    _pmolParameter.Id = pmol.Id;
                    _pmolParameter.ContractingUnitSequenceId = ContractingUnitSequenceId;
                    _pmolParameter.ProjectSequenceId = pmol.ProjectDefinition.SequenceCode;
                    _pmolParameter.Lang = lang;
                    //_pmolParameter.Id = timeClockDto.PmolId;
                    _pmolParameter.ContextAccessor = ContextAccessor;
                    _pmolParameter.TenantProvider = ItenantProvider;
                    await _iPmolRepository.UpdateEndTime(_pmolParameter);

                    var dto = new PmolLabours
                    {
                        PmolId = pmol.Id,
                        Type = timeClockDto.Type.ToString(),
                        IsForeman = timeClockDto.IsForeman,
                        IsBreak = timeClockDto.IsBreak
                    };

                    _pmolParameter.PmolLabours = dto;

                    await _iPmolRepository.UpdatePmolStop(_pmolParameter);

                    _pmolParameter.Id = pmol.ProjectMoleculeId;
                    _pmolParameter.borRepository = _borRepository;
                    _pmolParameter.IPmolResourceRepository = _iPmolResourceRepository;
                    //var newID = await _iPmolRepository.ClonePmol(_pmolParameter);
                    return Ok(new ApiOkResponse(pmol.ProjectMoleculeId));
                }
            }
            else if (timeClockDto.Type == 6) //break
            {
                // IEnumerable<PmolListDtoForMobile> pmolList = await _iPmolRepository.GetPmolByUserId(_pmolParameter);
                //IEnumerable<PmolListDtoForMobile> toBeStoppedPmol = pmolList.Where(p => p.StartDateTime != null && p.EndDateTime == null).FirstOrDefault();
                _pmolParameter.Id = timeClockDto.PmolId;


                _pmolParameter.ContractingUnitSequenceId = ContractingUnitSequenceId;
                _pmolParameter.ProjectSequenceId = ProjectSequenceId;
                _pmolParameter.Lang = lang;
                _pmolParameter.ContextAccessor = ContextAccessor;
                _pmolParameter.TenantProvider = ItenantProvider;

                var dto = new PmolLabours
                {
                    PmolId = timeClockDto.PmolId,
                    Type = timeClockDto.Type.ToString(),
                    IsForeman = timeClockDto.IsForeman,
                    IsBreak = timeClockDto.IsBreak
                };

                _pmolParameter.PmolLabours = dto;

                var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims
                    .First(claim =>
                        claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")
                    .Value;
                _pmolParameter.UserId = objectIdentifier;

                await _iPmolRepository.BreakPmol(_pmolParameter);


                await _iPmolRepository.UpdatePmolStart(_pmolParameter);
            }

            _pmolParameter.Id = timeClockDto.PmolId;
            _pmolParameter.ContractingUnitSequenceId = ContractingUnitSequenceId;
            _pmolParameter.ProjectSequenceId = ProjectSequenceId;
            _pmolParameter.Lang = lang;
            var userId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            _pmolParameter.UserId = userId;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = ItenantProvider;

            var isForeman = await _iPmolRepository.IsForeman(_pmolParameter);
            timeClockDto.IsForeman = isForeman;

            await _iPmolRepository.UpdateUserCurrentPmol(_pmolParameter);

            return Ok(new ApiOkResponse(timeClockDto));
        }
        catch (Exception ex)
        {
            _log4net.Info("CreatePmolTimeClockNew error");
            _log4net.Info(jsonString);
            _log4net.Info(ex.ToString());
            return BadRequest(new ApiResponse(400, false, ex.Message));
        }
    }

    [HttpPost("CreatePmolTimeClockChanged")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreatePmolTimeClockChanged(
        [FromBody] CreateTimeClockDto timeClockDto)
    {
        var _log4net = LogManager.GetLogger(typeof(TimeClockController));
        var jsonString = JsonConvert.SerializeObject(timeClockDto, Formatting.Indented);

        try
        {
            if (!ModelState.IsValid) return BadRequest(new ApiBadRequestResponse(ModelState));

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            var ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            var _pmolParameter = new PmolParameter();
            timeClockDto.PbsResourceRepository = _iPbsResourceRepository;
            await _iTimeClockRepository.CreateTimeClockChanged(UPrinceCustomerContext, timeClockDto,
                ContextAccessor, lang, ContractingUnitSequenceId, ProjectSequenceId, ItenantProvider);
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims
                .First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")
                .Value;

            if (timeClockDto.Type == 0 || timeClockDto.Type == 1 || timeClockDto.Type == 2)
            {
                if (timeClockDto.IsBreak == false)
                {
                    _pmolParameter.ContractingUnitSequenceId = ContractingUnitSequenceId;
                    _pmolParameter.ProjectSequenceId = ProjectSequenceId;
                    _pmolParameter.Lang = lang;
                    _pmolParameter.Id = timeClockDto.PmolId;
                    _pmolParameter.ContextAccessor = ContextAccessor;
                    _pmolParameter.TenantProvider = ItenantProvider;
                    await _iPmolRepository.UpdateStartTime(_pmolParameter);

                    var dto = new PmolLabours
                    {
                        PmolId = timeClockDto.PmolId,
                        Type = timeClockDto.Type.ToString(),
                        IsForeman = timeClockDto.IsForeman,
                        IsBreak = timeClockDto.IsBreak
                    };

                    _pmolParameter.PmolLabours = dto;
                    
                    //var user = UPrinceCustomerContext.ApplicationUser.Where(u => u.OId == objectIdentifier).FirstOrDefault();
                    _pmolParameter.UserId = objectIdentifier;

                    await _iPmolRepository.UpdatePmolStart(_pmolParameter);

                    var pmolList =
                        await _iPmolRepository.GetPmolByUserId(_pmolParameter);
                    // var toBeStoppedPmol = pmolList
                    //     .Where(p => p.StartDateTime != null && p.EndDateTime == null).FirstOrDefault();
                    var toBeStoppedPmol = pmolList
                        .Where(p => p.StartDateTime != null && p.IsEnded == false).FirstOrDefault();
                    if (toBeStoppedPmol != null && toBeStoppedPmol.Id != timeClockDto.PmolId)
                    {
                        _pmolParameter.Id = toBeStoppedPmol.Id;
                        _pmolParameter.ProjectSequenceId = toBeStoppedPmol.ProjectDefinition.SequenceCode;
                        await _iPmolRepository.UpdateEndTime(_pmolParameter);

                        await _iPmolRepository.UpdatePmolStop(_pmolParameter);
                    }
                }
                else //if is break
                {
                    _pmolParameter.Id = timeClockDto.PmolId;
                    _pmolParameter.ContractingUnitSequenceId = ContractingUnitSequenceId;
                    _pmolParameter.ProjectSequenceId = ProjectSequenceId;
                    _pmolParameter.Lang = lang;
                    _pmolParameter.ContextAccessor = ContextAccessor;
                    _pmolParameter.TenantProvider = ItenantProvider;

                    await _iPmolRepository.BreakPmolStop(_pmolParameter);


                    var dto = new PmolLabours
                    {
                        PmolId = timeClockDto.PmolId,
                        Type = timeClockDto.Type.ToString(),
                        IsForeman = timeClockDto.IsForeman,
                        IsBreak = timeClockDto.IsBreak
                    };

                    _pmolParameter.PmolLabours = dto;
                    _pmolParameter.UserId = objectIdentifier;

                    await _iPmolRepository.UpdatePmolStart(_pmolParameter);
                }
            }
            else if (timeClockDto.Type == 5) //stop
            {
                _pmolParameter.ContractingUnitSequenceId = ContractingUnitSequenceId;
                // _pmolParameter.ProjectSequenceId = ProjectSequenceId;
                _pmolParameter.Lang = lang;
                //_pmolParameter.Id = timeClockDto.PmolId;
                _pmolParameter.UserId = objectIdentifier;
                _pmolParameter.ContextAccessor = ContextAccessor;
                _pmolParameter.TenantProvider = ItenantProvider;
                var pmolList =
                    await _iPmolRepository.GetPmolByUserId(_pmolParameter);
                // var toBeStoppedPmol =
                //     pmolList.Where(p => p.StartDateTime != null && p.EndDateTime == null);
                var toBeStoppedPmol =
                    pmolList.Where(p => p.StartDateTime != null && p.IsEnded == false);
                foreach (var pmol in toBeStoppedPmol)
                {
                    _pmolParameter.Id = pmol.Id;
                    _pmolParameter.ContractingUnitSequenceId = ContractingUnitSequenceId;
                    _pmolParameter.ProjectSequenceId = pmol.ProjectDefinition.SequenceCode;
                    _pmolParameter.Lang = lang;
                    //_pmolParameter.Id = timeClockDto.PmolId;
                    _pmolParameter.ContextAccessor = ContextAccessor;
                    _pmolParameter.TenantProvider = ItenantProvider;
                    await _iPmolRepository.UpdateEndTime(_pmolParameter);

                    var dto = new PmolLabours
                    {
                        PmolId = pmol.Id,
                        Type = timeClockDto.Type.ToString(),
                        IsForeman = timeClockDto.IsForeman,
                        IsBreak = timeClockDto.IsBreak
                    };

                    _pmolParameter.PmolLabours = dto;

                    await _iPmolRepository.UpdatePmolStop(_pmolParameter);

                    _pmolParameter.Id = pmol.ProjectMoleculeId;
                    _pmolParameter.borRepository = _borRepository;
                    _pmolParameter.IPmolResourceRepository = _iPmolResourceRepository;
                    //var newID = await _iPmolRepository.ClonePmol(_pmolParameter);
                    return Ok(new ApiOkResponse(pmol.ProjectMoleculeId));
                }
            }
            else if (timeClockDto.Type == 6) //break
            {
                // IEnumerable<PmolListDtoForMobile> pmolList = await _iPmolRepository.GetPmolByUserId(_pmolParameter);
                //IEnumerable<PmolListDtoForMobile> toBeStoppedPmol = pmolList.Where(p => p.StartDateTime != null && p.EndDateTime == null).FirstOrDefault();
                _pmolParameter.Id = timeClockDto.PmolId;


                _pmolParameter.ContractingUnitSequenceId = ContractingUnitSequenceId;
                _pmolParameter.ProjectSequenceId = ProjectSequenceId;
                _pmolParameter.Lang = lang;
                _pmolParameter.ContextAccessor = ContextAccessor;
                _pmolParameter.TenantProvider = ItenantProvider;

                var dto = new PmolLabours
                {
                    PmolId = timeClockDto.PmolId,
                    Type = timeClockDto.Type.ToString(),
                    IsForeman = timeClockDto.IsForeman,
                    IsBreak = timeClockDto.IsBreak
                };

                _pmolParameter.PmolLabours = dto;
                _pmolParameter.UserId = objectIdentifier;

                await _iPmolRepository.BreakPmol(_pmolParameter);


                await _iPmolRepository.UpdatePmolStart(_pmolParameter);
            }

            _pmolParameter.Id = timeClockDto.PmolId;
            _pmolParameter.ContractingUnitSequenceId = ContractingUnitSequenceId;
            _pmolParameter.ProjectSequenceId = ProjectSequenceId;
            _pmolParameter.Lang = lang;
            _pmolParameter.UserId = objectIdentifier;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = ItenantProvider;

            var isForeman = await _iPmolRepository.IsForeman(_pmolParameter);
            timeClockDto.IsForeman = isForeman;

            await _iPmolRepository.UpdateUserCurrentPmol(_pmolParameter);

            return Ok(new ApiOkResponse(timeClockDto));
        }
        catch (Exception ex)
        {
            _log4net.Info("CreatePmolTimeClockNew error");
            _log4net.Info(jsonString);
            _log4net.Info(ex.ToString());
            return BadRequest(new ApiResponse(400, false, ex.Message));
        }
    }

    [HttpPost("UpdatePmolLabourJobDone")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> UpdatePmolLabourJobDone(
        [FromBody] PmolJobDone PmolJobDone)
    {
        var lang = langCode(Request.Headers["lang"].FirstOrDefault());
        var ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
        var ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();

        var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
            claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;


        var results = await _iTimeClockRepository.UpdatePmolLabourJobDone(PmolJobDone, ContextAccessor, lang,
            ContractingUnitSequenceId, ProjectSequenceId, ItenantProvider, objectIdentifier);
        return Ok(new ApiOkResponse(results));
    }

    [HttpGet("PmolFinishCheck")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> PmolFinishCheck()
    {
        var lang = langCode(Request.Headers["lang"].FirstOrDefault());
        var ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
        //  var ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
        var _pmolParameter = new PmolParameter
        {
            ContractingUnitSequenceId = ContractingUnitSequenceId,
            Lang = lang
        };

        var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
            claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

        _pmolParameter.UserId = objectIdentifier;
        _pmolParameter.ContextAccessor = ContextAccessor;
        _pmolParameter.TenantProvider = ItenantProvider;
        var pmolList =
            await _iPmolRepository.GetPmolByUserId(_pmolParameter);

        var result = false;
        var toBeStoppedPmolIsForeman =
            pmolList.Where(p => p.IsForeman);

        if (toBeStoppedPmolIsForeman.Any())
        {
            var toBeStoppedPmol =
                pmolList.Where(p => p.StartDateTime != null && p.IsEnded == false && p.IsFinished == false);

            result = toBeStoppedPmol.Any();
        }


        return Ok(new ApiOkResponse(result));
    }
}