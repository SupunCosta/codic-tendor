using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.PMOL;
using UPrinceV4.Web.Data.VisualPlan;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Repositories.Interfaces.BOR;
using UPrinceV4.Web.Repositories.Interfaces.CPC;
using UPrinceV4.Web.Repositories.Interfaces.PMOL;

namespace UPrinceV4.Web.Controllers.VP;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class MyCalenderController : CommonConfigurationController
{
    private readonly IMyCalenderRepository _iMyCalenderRepository;
    private readonly ITenantProvider _iTenantProvider;
    private readonly IPmolRepository _iPmolRepository;
    private readonly IPmolResourceRepository _iPmolResourceRepository;
    private readonly IBorRepository _iBorRepository;
    private readonly IBorResourceRepository _iBorResourceRepository;
    private readonly ICoporateProductCatalogRepository _iCoporateProductCatalogRepository;
    private readonly IVPRepository _iVPRepository;
    
    
    public MyCalenderController(ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor,
        ApiResponse apiResponse, ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse,
        ITenantProvider iTenantProvider,MyCalenderParameter myCalenderParameter,IMyCalenderRepository iMyCalenderRepository,
        IPmolRepository iPmolRepository,IPmolResourceRepository iPmolResourceRepository,IBorRepository iBorRepository,
        IBorResourceRepository iBorResourceRepository,ICoporateProductCatalogRepository iCoporateProductCatalogRepository,IVPRepository iVPRepository, IConfiguration iConfiguration
    ) 
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse, iTenantProvider)
    {
        _iMyCalenderRepository = iMyCalenderRepository;
        _iTenantProvider = iTenantProvider;
        _iPmolRepository = iPmolRepository;
        _iPmolResourceRepository = iPmolResourceRepository;
        _iBorRepository = iBorRepository;
        _iBorResourceRepository = iBorResourceRepository;
        _iCoporateProductCatalogRepository = iCoporateProductCatalogRepository;
        _iVPRepository = iVPRepository;
        _iConfiguration = iConfiguration;

    }
    private IConfiguration _iConfiguration { get; }
    
    [HttpPost("MyCalenderListData")]
    public async Task<ActionResult> MyCalenderListData([FromBody] CalenderGetTeamDto calenderGetTeamDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromHeader(Name = "isMyEnv")] bool myEnv)
    {
        try
        {
            var lang = langCode(langX);
            var _myCalenderParameter = new MyCalenderParameter();
            _myCalenderParameter.ContractingUnitSequenceId = CU;
            _myCalenderParameter.ProjectSequenceId = Project;
            _myCalenderParameter.Lang = lang;
            _myCalenderParameter.CalenderGetTeamDto = calenderGetTeamDto;
            _myCalenderParameter.ContextAccessor = ContextAccessor;
            _myCalenderParameter.TenantProvider = _iTenantProvider;
            _myCalenderParameter.Configuration = _iConfiguration;
            _myCalenderParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            if (myEnv)
            {
                var shortcutPaneData = await _iMyCalenderRepository.MyCalenderListData(_myCalenderParameter);
                return Ok(new ApiOkResponse(shortcutPaneData));
            }
            else
            {
                var shortcutPaneData = await _iMyCalenderRepository.Teams(_myCalenderParameter);
                return Ok(new ApiOkResponse(shortcutPaneData));
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("MyCalenderListDataForCu")]
    public async Task<ActionResult> MyCalenderListDataForCu([FromBody] MyCalenderGetTeamDto getTeamDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromHeader(Name = "isMyEnv")] bool myEnv)
    {
        try
        {
            var lang = langCode(langX);
            var _myCalenderParameter = new MyCalenderParameter();
            _myCalenderParameter.ContractingUnitSequenceId = CU;
            _myCalenderParameter.ProjectSequenceId = Project;
            _myCalenderParameter.Lang = lang;
            _myCalenderParameter.MyCalenderGetTeamDto = getTeamDto;
            _myCalenderParameter.ContextAccessor = ContextAccessor;
            _myCalenderParameter.TenantProvider = _iTenantProvider;
            _myCalenderParameter.Configuration = _iConfiguration;
            _myCalenderParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            var shortcutPaneData = await _iMyCalenderRepository.MyCalenderListDataForCu(_myCalenderParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
          
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("MyCalenderProjectFlter")]
    public async Task<ActionResult> MyCalenderProjectFlter([FromBody] ProjectSearchMyCalender ProjectSearchMyCalender,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromHeader(Name = "isMyEnv")] bool myEnv)
    {
        try
        {
            var lang = langCode(langX);
            var _myCalenderParameter = new MyCalenderParameter();
            _myCalenderParameter.Lang = lang;
            _myCalenderParameter.ProjectSearchMyCalender = ProjectSearchMyCalender;
            _myCalenderParameter.IsMyEnv = myEnv;
            _myCalenderParameter.ContextAccessor = ContextAccessor;
            _myCalenderParameter.TenantProvider = _iTenantProvider;
            _myCalenderParameter.Configuration = _iConfiguration;
            _myCalenderParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            var data = await _iMyCalenderRepository.MyCalenderProjectFlter(_myCalenderParameter);
            return Ok(new ApiOkResponse(data));
            
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpGet("GetMyCalenderPbsTaxonomy/{Id}")]
    public async Task<ActionResult> GetMyCalenderPbsTaxonomy(string Id,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromHeader(Name = "isMyEnv")] bool myEnv)
    {
        try
        {
            var lang = langCode(langX);
            var _myCalenderParameter = new MyCalenderParameter();
            _myCalenderParameter.Lang = lang;
            _myCalenderParameter.Id = Id;
            _myCalenderParameter.ContextAccessor = ContextAccessor;
            _myCalenderParameter.TenantProvider = _iTenantProvider;
            _myCalenderParameter.Configuration = _iConfiguration;
            _myCalenderParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                 claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            var data = await _iMyCalenderRepository.GetMyCalenderPbsTaxonomy(_myCalenderParameter);
            return Ok(new ApiOkResponse(data));
            
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("MyCalenderCreatePmol")]
    public async Task<ActionResult> MyCalenderCreatePmol([FromBody] ProjectCreateMycal dto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromHeader(Name = "isMyEnv")] bool myEnv)
    {
        try
        {
            var lang = langCode(langX);
            var _myCalenderParameter = new MyCalenderParameter();
            _myCalenderParameter.Lang = lang;
            _myCalenderParameter.ProjectCreateMycal = dto;
            _myCalenderParameter.ContextAccessor = ContextAccessor;
            _myCalenderParameter.TenantProvider = _iTenantProvider;
            _myCalenderParameter.Configuration = _iConfiguration;
            _myCalenderParameter.iPmolRepository = _iPmolRepository;
            _myCalenderParameter.iPmolResourceRepository = _iPmolResourceRepository;
            _myCalenderParameter._iBorRepository = _iBorRepository;
            _myCalenderParameter._iBorResourceRepository = _iBorResourceRepository;
            _myCalenderParameter._iCoporateProductCatalogRepository = _iCoporateProductCatalogRepository;
            _myCalenderParameter.VpRepository = _iVPRepository;
            _myCalenderParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var data = await _iMyCalenderRepository.MyCalenderCreatePmol(_myCalenderParameter);
            return Ok(new ApiOkResponse(data));
            
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}