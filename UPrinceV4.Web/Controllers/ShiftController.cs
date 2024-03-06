using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Util;
using TimeZone = UPrinceV4.Web.Data.TimeZone;

namespace UPrinceV4.Web.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class ShiftController : CommonConfigurationController
{
    private readonly IQrCodeRepository _iQrCodeRepository;
    private readonly IShiftRepository _iShiftRepository;
    private readonly ITimeClockActivityTypeRepository _iTimeClockActivityTypeRepository;
    private readonly IWorkflowStateRepository _iWorkflowStateRepository;

    public ShiftController(IShiftRepository iShiftRepository, IWorkflowStateRepository iworkRepository,
        ITimeClockActivityTypeRepository iTimeClockActivityTypeRepository, IQrCodeRepository iQrCodeRepository,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)
    {
        _iShiftRepository = iShiftRepository;
        _iWorkflowStateRepository = iworkRepository;
        _iTimeClockActivityTypeRepository = iTimeClockActivityTypeRepository;
        _iQrCodeRepository = iQrCodeRepository;
    }

    //[Microsoft.AspNetCore.Mvc.HttpPost("Filter")]
    [HttpPost("Filter")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetShift()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var user = UPrinceCustomerContext.ApplicationUser.Where(u => u.OId == objectIdentifier)
                .FirstOrDefault();
            var ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            var ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            //  string message = ApiErrorMessages.GetErrorMessage(ItenantProvider, ErrorMessageKey.Ok, lang).Message;
            var shifts = await _iShiftRepository.GetShift(UPrinceCustomerContext, ContractingUnitSequenceId,
                ProjectSequenceId, ItenantProvider);
            foreach (var shift in shifts)
            {
                var workflowId = shift.WorkflowStateId;
                var workflow =
                    await _iWorkflowStateRepository.GetWorkflowStateById(UPrinceCustomerContext, workflowId, lang);
                shift.WorkflowState = workflow;
            }

            return Ok(!shifts.Any()
                ? new ApiResponse(400, false, "noShiftsAvailable")
                : new ApiOkResponse(shifts));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("FilterV2")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetShiftV2([FromBody] ShiftFilter ShiftFilter,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var ShiftParameter = new ShiftParameter();
            ShiftParameter.ContractingUnitSequenceId = CU;
            ShiftParameter.ProjectSequenceId = Project;
            ShiftParameter.Lang = lang;
            ShiftParameter.ContextAccessor = ContextAccessor;
            ShiftParameter.TenantProvider = ItenantProvider;
            ShiftParameter.Filter = ShiftFilter;

            ShiftParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var shifts = await _iShiftRepository.GetShiftV2(ShiftParameter);

            return Ok(!shifts.Any() ? new ApiResponse(400, false, "noShiftsAvailable") : new ApiOkResponse(shifts));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    //[Microsoft.AspNetCore.Mvc.HttpGet("ReadByUser")]
    [HttpGet("ReadByUser")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetShiftByUser()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            var ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            //string message = ApiErrorMessages.GetErrorMessage(ItenantProvider, ErrorMessageKey.Ok, lang).Message;
            var shifts = await _iShiftRepository.GetShiftByUser(UPrinceCustomerContext, ContextAccessor,
                ContractingUnitSequenceId, ProjectSequenceId, ItenantProvider);
            foreach (var shift in shifts)
            {
                var workflow =
                    await _iWorkflowStateRepository.GetWorkflowStateById(UPrinceCustomerContext,
                        shift.WorkflowStateId, lang);
                shift.WorkflowState = workflow;
            }

            return Ok(new ApiOkResponse(shifts));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

   
    [HttpGet("ReadShiftDetails/{shiftId}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetShiftDetails(string ShiftId)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            var ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            var shiftDetails = await _iShiftRepository.GetShiftDetails(UPrinceCustomerContext, ShiftId,
                _iTimeClockActivityTypeRepository, lang, ContractingUnitSequenceId, ProjectSequenceId,
                ItenantProvider);
            return Ok(new ApiOkResponse(shiftDetails));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    
    [HttpPost("GetShiftByDate")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetShiftByDate(TimeZone timeZone)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            var ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            var shifts = await _iShiftRepository.GetShiftByDate(UPrinceCustomerContext, timeZone,
                ContractingUnitSequenceId, ProjectSequenceId, ItenantProvider);
            foreach (var shift in shifts)
            {
                var workflow =
                    await _iWorkflowStateRepository.GetWorkflowStateById(UPrinceCustomerContext,
                        shift.WorkflowStateId, lang);
                shift.WorkflowState = workflow;
            }

            if (!shifts.Any())
            {
                ApiOkResponse.Status = false;
                ApiOkResponse.StatusCode = 400;
                ApiOkResponse.Message = ApiErrorMessages
                    .GetErrorMessage(ItenantProvider, ErrorMessageKey.NoAvailableShiftsForTheDay, lang).Message;
            }
            else
            {
                ApiOkResponse.Status = true;
                ApiOkResponse.StatusCode = 200;
                ApiOkResponse.Message = "ok";
            }

            // ApiResponse.StatusCode = 200;
            // ApiResponse.Status = false;
            //ApiResponse.Message = ApiErrorMessages.GetErrorMessage(ItenantProvider, ErrorMessageKey.NoAvailableShiftsForTheDay, lang).Message;
            ApiOkResponse.Result = shifts;


            // ApiOkResponse.Message = message;
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    
    [HttpPut("Approve/{shiftId}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ApproveShift(string shiftId)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            var ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            var shift = await _iShiftRepository.ChangeState(UPrinceCustomerContext, shiftId, "Approved", lang,
                ContractingUnitSequenceId, ProjectSequenceId, ItenantProvider);
            if (shift != null) return Ok(new ApiOkResponse(shift));
            
            return BadRequest( new ApiResponse(400, false, null));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
            
        }
    }

    //[Microsoft.AspNetCore.Mvc.HttpPut("Pending/{shiftId}")]
    [HttpPut("Pending/{shiftId}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> PendingShift(string shiftId)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            var ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            var shift = await _iShiftRepository.ChangeState(UPrinceCustomerContext, shiftId, "Pending", lang,
                ContractingUnitSequenceId, ProjectSequenceId, ItenantProvider);
            if (shift != null) return Ok(new ApiOkResponse(shift));
            
            return BadRequest(new ApiResponse(400, false, null));  
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    //[Microsoft.AspNetCore.Mvc.HttpPut("Reject/{shiftId}")]
    [HttpPut("Reject/{shiftId}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> RejectShift(string shiftId)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            var ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            var shift = await _iShiftRepository.ChangeState(UPrinceCustomerContext, shiftId, "Rejected", lang,
                ContractingUnitSequenceId, ProjectSequenceId, ItenantProvider);
            if (shift != null) return Ok(new ApiOkResponse(shift));
            
            return BadRequest(new ApiResponse(400, false, null));  
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("ShiftFilter")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> FilterShifts([FromBody] ShiftFilter filter)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            var ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            var shifts = await _iShiftRepository.FilterShifts(UPrinceCustomerContext, filter,
                ContractingUnitSequenceId, ProjectSequenceId, ItenantProvider);
            foreach (var shift in shifts)
            {
                var workflow =
                    await _iWorkflowStateRepository.GetWorkflowStateById(UPrinceCustomerContext,
                        shift.WorkflowStateId, lang);
                shift.WorkflowState = workflow;
            }


            var mApiOkResponse = new ApiOkResponse(shifts);


            if (!shifts.Any())
            {
                var mApiResponse = new ApiOkResponse(null, "noshiftsavailable");
                mApiResponse.Status = false;
                return Ok(mApiResponse);
            }

            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

   
    [HttpPost("ReadShiftsForExcel")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetShiftsForExel([FromBody] ShiftFilter filter)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            //var user = UPrinceCustomerContext.ApplicationUser.Where(u => u.OId == objectIdentifier).Include(u => u.UserRole).ThenInclude(u => u.Role).FirstOrDefault();
            var ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            // string ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            var shiftDetails = await _iShiftRepository.ReadShiftsForExcel(UPrinceCustomerContext,
                ContractingUnitSequenceId, null, ItenantProvider, filter, _iQrCodeRepository, lang,
                objectIdentifier);
            return Ok(new ApiOkResponse(shiftDetails));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetAllShiftsForExcel")]
    public async Task<ActionResult> GetAllShiftsForExcel()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var user = UPrinceCustomerContext.ApplicationUser.Where(u => u.OId == objectIdentifier)
                .FirstOrDefault();
            var ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            var ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();

            var shiftDetails = await _iShiftRepository.ReadAllShiftsForExcel(UPrinceCustomerContext,
                ContractingUnitSequenceId, ProjectSequenceId, ItenantProvider, _iQrCodeRepository, lang,
                objectIdentifier);
            return Ok(new ApiOkResponse(shiftDetails));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpDelete("OptimizeDatabase")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> OptimizeDatabase([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
           // var lang = langCode(langX);
            var ContractingUnitSequenceId = CU;
            var ProjectSequenceId = Project;
            var UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            await _iShiftRepository.OptimizeDatabase(ContractingUnitSequenceId, ProjectSequenceId, ItenantProvider,
                UserId);
            return Ok(new ApiOkResponse("Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}