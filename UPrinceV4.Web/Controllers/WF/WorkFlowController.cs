using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.WF;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Repositories.Interfaces.PMOL;

namespace UPrinceV4.Web.Controllers.WF;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class WorkFlowController : CommonConfigurationController
{
    private readonly IWorkFlowRepository _iWorkFlowRepository;
    private readonly ITenantProvider _TenantProvider;
    private readonly WFParameter _wfParameter;


    public WorkFlowController(ITenantProvider tenantProvider,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApplicationDbContext dbContext,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, PmolParameter PmolParameter,
        ILogger<WorkFlowController> logger, ITenantProvider iTenantProvider
        , IWorkFlowRepository iWorkFlowRepository)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)

    {
        _iWorkFlowRepository = iWorkFlowRepository;
        _wfParameter = new WFParameter();
        _TenantProvider = tenantProvider;
    }

    [HttpGet("ShortcutPaneData")]
    public async Task<ActionResult> ReadWHShortcutPaneData([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _wfParameter.ContractingUnitSequenceId = CU;
            _wfParameter.ProjectSequenceId = Project;
            _wfParameter.Lang = lang;
            _wfParameter.ContextAccessor = ContextAccessor;
            _wfParameter.TenantProvider = _TenantProvider;
            var shortcutPaneData = await _iWorkFlowRepository.GetShortcutpaneData(_wfParameter);

            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("FilterWF")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetWFList([FromBody] WFFilter filter,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _wfParameter.ContractingUnitSequenceId = CU;
            _wfParameter.ProjectSequenceId = Project;
            _wfParameter.Lang = lang;
            _wfParameter.Filter = filter;
            _wfParameter.ContextAccessor = ContextAccessor;
            _wfParameter.TenantProvider = _TenantProvider;
            var result = await _iWorkFlowRepository.GetWFList(_wfParameter);
            var mApiOkResponse = new ApiOkResponse(result);


            if (!result.Any())
            {
                var mApiResponse = new ApiOkResponse(null, "noAvailableWorkFlow");
                mApiResponse.Status = false;
                return Ok(mApiResponse);
            }

            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CreateHeader")]
    public async Task<ActionResult> CreateHeader([FromBody] WFCreateDto WFDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
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

            _wfParameter.ContractingUnitSequenceId = CU;
            _wfParameter.ProjectSequenceId = Project;
            _wfParameter.Lang = lang;
            _wfParameter.WFDto = WFDto;
            _wfParameter.ContextAccessor = ContextAccessor;
            _wfParameter.TenantProvider = ItenantProvider;
            _wfParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var s = await _iWorkFlowRepository.CreateHeader(_wfParameter);
            return Ok(new ApiOkResponse(s));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetWFDropdown")]
    
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetWFDropdown([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _wfParameter.ContractingUnitSequenceId = CU;
            _wfParameter.ProjectSequenceId = Project;
            _wfParameter.Lang = lang;

            _wfParameter.TenantProvider = ItenantProvider;
            return Ok(new ApiOkResponse(await _iWorkFlowRepository.GetWFDropdown(_wfParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetById/{SequenceId}")]
    public async Task<ActionResult> GetWFById(string SequenceId, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            _wfParameter.ContractingUnitSequenceId = CU;
            _wfParameter.ProjectSequenceId = Project;
            _wfParameter.Lang = lang;
            _wfParameter.Id = SequenceId;
            _wfParameter.ContextAccessor = ContextAccessor;
            _wfParameter.TenantProvider = ItenantProvider;
            return Ok(new ApiOkResponse(await _iWorkFlowRepository.GetWFById(_wfParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPut("CUApprove/{SequenceId}")]
    public async Task<ActionResult> CUApprove([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, string SequenceId)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(langX);

            _wfParameter.ContractingUnitSequenceId = CU;
            _wfParameter.ProjectSequenceId = Project;
            _wfParameter.Lang = lang;
            _wfParameter.Id = SequenceId;
            _wfParameter.ContextAccessor = ContextAccessor;
            _wfParameter.TenantProvider = ItenantProvider;
            _wfParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var s = await _iWorkFlowRepository.CUApprove(_wfParameter);
            return Ok(new ApiOkResponse(s));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPut("ProjectApprove/{SequenceId}")]
    public async Task<ActionResult> ProjectApprove([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, string SequenceId)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(langX);

            _wfParameter.ContractingUnitSequenceId = CU;
            _wfParameter.ProjectSequenceId = Project;
            _wfParameter.Lang = lang;
            _wfParameter.Id = SequenceId;
            _wfParameter.ContextAccessor = ContextAccessor;
            _wfParameter.TenantProvider = ItenantProvider;
            _wfParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var s = await _iWorkFlowRepository.ProjectApprove(_wfParameter);
            return Ok(new ApiOkResponse(s));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}