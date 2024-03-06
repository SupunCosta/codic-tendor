using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.VisualPlan;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Controllers.VP;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class AbsenceController : CommonConfigurationController
{
    private readonly AbsenceParameter _absenceParameter;
    private readonly IAbsenceRepository _iAbsenceRepository;


    public AbsenceController(
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse,
        ITenantProvider iTenantProvider
        , IAbsenceRepository iAbsenceRepository)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)

    {
        _iAbsenceRepository = iAbsenceRepository;
        _absenceParameter = new AbsenceParameter();
    }

    [HttpPost("CreateAbsence")]
    public async Task<ActionResult> CreateAbsence(
        [FromBody] AbsenceHeaderCreateDto AbsenceDto, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(langX);

            _absenceParameter.ContractingUnitSequenceId = CU;
            _absenceParameter.ProjectSequenceId = Project;
            _absenceParameter.Lang = lang;
            _absenceParameter.AbsenceDto = AbsenceDto;
            _absenceParameter.ContextAccessor = ContextAccessor;
            _absenceParameter.TenantProvider = ItenantProvider;
            _absenceParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var s = await _iAbsenceRepository.Create(_absenceParameter);
            return Ok(new ApiOkResponse(s));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    [HttpGet("GetAbsenceById/{AbsenceId}")]
    public async Task<ActionResult> GetAbsenceById(string AbsenceId, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _absenceParameter.ContractingUnitSequenceId = CU;
            _absenceParameter.ProjectSequenceId = Project;
            _absenceParameter.Lang = lang;
            _absenceParameter.Id = AbsenceId;
            _absenceParameter.ContextAccessor = ContextAccessor;
            _absenceParameter.TenantProvider = ItenantProvider;
            return Ok(new ApiOkResponse(await _iAbsenceRepository.AbsenceGetById(_absenceParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetAbsenceListByPersonId/{PersonId}")]
    public async Task<ActionResult> GetAbsenceListByPersonId(string PersonId, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _absenceParameter.ContractingUnitSequenceId = CU;
            _absenceParameter.ProjectSequenceId = Project;
            _absenceParameter.Lang = lang;
            _absenceParameter.Id = PersonId;
            _absenceParameter.ContextAccessor = ContextAccessor;
            _absenceParameter.TenantProvider = ItenantProvider;
            return Ok(new ApiOkResponse(await _iAbsenceRepository.GetAbsenceListByPersonId(_absenceParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpDelete("DeleteAbsence")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAbsence([FromBody] List<string> idList)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            _absenceParameter.ContextAccessor = ContextAccessor;
            _absenceParameter.TenantProvider = ItenantProvider;
            _absenceParameter.TenantProvider = ItenantProvider;
            _absenceParameter.IdList = idList;
            _absenceParameter.Lang = lang;
            var companyList = await _iAbsenceRepository.DeleteAbsence(_absenceParameter);
            ApiOkResponse.Message = "ok";
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetAbsenceLeaveType")]
    public async Task<ActionResult> GetAbsenceLeaveType([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _absenceParameter.ContractingUnitSequenceId = CU;
            _absenceParameter.ProjectSequenceId = Project;
            _absenceParameter.Lang = lang;
            _absenceParameter.ContextAccessor = ContextAccessor;
            _absenceParameter.TenantProvider = ItenantProvider;
            return Ok(new ApiOkResponse(await _iAbsenceRepository.GetAbsenceLeaveType(_absenceParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}