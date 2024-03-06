using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.BOR;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Data.PMOL;
using UPrinceV4.Web.Data.VisualPlaane;
using UPrinceV4.Web.Data.VisualPlan;
using UPrinceV4.Web.Data.WH;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Repositories.Interfaces.BOR;
using UPrinceV4.Web.Repositories.Interfaces.CPC;
using UPrinceV4.Web.Repositories.Interfaces.PBS;
using UPrinceV4.Web.Repositories.Interfaces.PMOL;

namespace UPrinceV4.Web.Controllers.VP;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class VPController : CommonConfigurationController
{
    private readonly IBorRepository _iBorRepository;
    private readonly IBorResourceRepository _iBorResourceRepositoryRepository;
    private readonly ICoporateProductCatalogRepository _iCoporateProductCatalogRepository;
    private readonly IPbsRepository _iPbsRepository;
    private readonly IPbsResourceRepository _iPbsResourceRepository;
    private readonly IPmolRepository _iPmolRepository;
    private readonly IProjectDefinitionRepository _iProjectDefinitionRepository;
    private readonly IProjectTeamRepository _iProjectTeamRepository;
    private readonly IShiftRepository _iShiftRepository;
    private readonly IVPRepository _iVPRepository;
    private readonly ITenantProvider _TenantProvider;
    //private readonly VPParameter _vpParameter;
    private readonly ICiawRepository _iCiawRepository;
    private readonly IProjectFinanceRepository _iProjectFinanceRepository;
    private readonly IProjectTimeRepository _iProjectTimeRepository;


    public VPController(ITenantProvider tenantProvider,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider
        , IVPRepository iVPRepository, IPmolRepository iPmolRepository, IConfiguration iConfiguration,
        IPbsResourceRepository iPbsResourceRepository, IBorRepository iBorRepository,
        IProjectDefinitionRepository iProjectDefinitionRepository,
        IShiftRepository iShiftRepository, IPbsRepository iPbsRepository,
        IBorResourceRepository iBorResourceRepositoryRepository,
        ICoporateProductCatalogRepository iCoporateProductCatalogRepository,
        IProjectTeamRepository iProjectTeamRepository,ICiawRepository iCiawRepository,IProjectFinanceRepository iProjectFinanceRepository,IProjectTimeRepository iProjectTimeRepository)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)

    {
        _iVPRepository = iVPRepository;
        //_vpParameter = new VPParameter();
        _TenantProvider = tenantProvider;
        _iPmolRepository = iPmolRepository;
        _iConfiguration = iConfiguration;
        _iPbsResourceRepository = iPbsResourceRepository;
        _iBorRepository = iBorRepository;
        _iProjectDefinitionRepository = iProjectDefinitionRepository;
        _iShiftRepository = iShiftRepository;
        _iPbsRepository = iPbsRepository;
        _iBorResourceRepositoryRepository = iBorResourceRepositoryRepository;
        _iCoporateProductCatalogRepository = iCoporateProductCatalogRepository;
        _iProjectTeamRepository = iProjectTeamRepository;
        _iCiawRepository = iCiawRepository;
        _iProjectFinanceRepository = iProjectFinanceRepository;
        _iProjectTimeRepository = iProjectTimeRepository;
    }


    private IConfiguration _iConfiguration { get; }

    [HttpGet("ShortcutPaneData")]
    public async Task<ActionResult> ReadVPShortcutPaneData([FromHeader(Name = "CU")] string cu,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = cu;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            var shortcutPaneData = await _iVPRepository.GetShortcutpaneData(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    [HttpPost("VpPo")]
    [ResponseCache(Duration = 300)]
    public async Task<ActionResult> VpPo([FromBody] VpPoFilter Filter,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromHeader(Name = "isMyEnv")] bool myEnv,
        CancellationToken cancellationToken)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                Filter = Filter,
                ContextAccessor = ContextAccessor,
                TenantProvider = _TenantProvider,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                IPmolRepository = _iPmolRepository
            };


            if (myEnv)
            {
                var shortcutPaneData = await _iVPRepository.AllVpPoMyEnv(_vpParameter);

                return Ok(new ApiOkResponse(shortcutPaneData));
            }
            else
            {
                var shortcutPaneData = await _iVPRepository.AllVpPo(_vpParameter);
                return Ok(new ApiOkResponse(shortcutPaneData));
            }
        }
        catch (OperationCanceledException e)
        {
            return BadRequest(new ApiResponse(400, false, e.Message));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    [HttpGet("GetVpPo")]
    public async Task<ActionResult> GetVoPo([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            var shortcutPaneData = await _iVPRepository.GetVpPo(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    [HttpGet("DeleteAllVpPo")]
    public async Task<ActionResult> DeleteAllVpPo([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            var shortcutPaneData = await _iVPRepository.DeleteAllVpPo(_vpParameter);

            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    [HttpPost("UpdateVpPo")]
    public async Task<ActionResult> UpdateVpPo([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project, [FromHeader(Name = "lang")] string langX,
        [FromBody] VpPo vpPo)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.VpPo = vpPo;
            var shortcutPaneData = await _iVPRepository.UpdateVpPo(_vpParameter);

            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("TreeGetAll")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> TreeGetAll([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.TenantProvider = ItenantProvider;
            return Ok(new ApiOkResponse(await _iVPRepository.GetAll(_vpParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    [HttpPost("BorList")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> BorList([FromBody] BorFilter filter,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.BorFilter = filter;
            var result = await _iVPRepository.BorList(_vpParameter);
            var mApiOkResponse = new ApiOkResponse(result);

            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    [HttpPost("GetPmolTeams")]
    public async Task<ActionResult> GetPmolTeams([FromBody] GetTeamDto getTeamDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromHeader(Name = "isMyEnv")] bool myEnv)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.GetTeamDto = getTeamDto;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            if (myEnv)
            {
                var shortcutPaneData = await _iVPRepository.TeamsMyEnv(_vpParameter);
                return Ok(new ApiOkResponse(shortcutPaneData));
            }
            else
            {
                var shortcutPaneData = await _iVPRepository.Teams(_vpParameter);
                return Ok(new ApiOkResponse(shortcutPaneData));
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("GetAvailableTeams")]
    public async Task<ActionResult> GetAvailableTeams([FromBody] GetTeamDto getTeamDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromHeader(Name = "isMyEnv")] bool myEnv)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.GetTeamDto = getTeamDto;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            if (myEnv)
            {
                var shortcutPaneData = await _iVPRepository.AvailableTeamsDayPlanningCuHr(_vpParameter);
                return Ok(new ApiOkResponse(shortcutPaneData));
            }
            else
            {
                //var shortcutPaneData = await _iVPRepository.AvailableTeams(_vpParameter);
                var shortcutPaneData = await _iVPRepository.AvailableTeamsDayPlanningProjectCuhr(_vpParameter);
                return Ok(new ApiOkResponse(shortcutPaneData));
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("UpdatePMOL")]
    public async Task<ActionResult> UpdatePMOL([FromBody] PomlUpdateDto pomlUpdateDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.PomlUpdateDto = pomlUpdateDto;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            var shortcutPaneData = await _iVPRepository.UpdatePMOL(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("GetVpWH")]
    public async Task<ActionResult> GetVpWH(
        [FromBody] WHTaxonomyFilterDto WHTaxonomyFilterDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromHeader(Name = "isMyEnv")] bool myEnv)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.WHTaxonomyFilter = WHTaxonomyFilterDto;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            if (myEnv)
            {
                var shortcutPaneData = await _iVPRepository.GetVpWHMyEnv(_vpParameter);
                return Ok(new ApiOkResponse(shortcutPaneData));
            }
            else
            {
                var shortcutPaneData = await _iVPRepository.GetVpWH(_vpParameter);
                return Ok(new ApiOkResponse(shortcutPaneData));
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("GetVpWHTool")]
    public async Task<ActionResult> GetVpWHTool(
        [FromBody] WHTaxonomyFilterDto WHTaxonomyFilterDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromHeader(Name = "isMyEnv")] bool myEnv)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.WHTaxonomyFilter = WHTaxonomyFilterDto;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            if (myEnv)
            {
                var shortcutPaneData = await _iVPRepository.GetVpWHToolMyEnv(_vpParameter);
                return Ok(new ApiOkResponse(shortcutPaneData));
            }
            else
            {
                var shortcutPaneData = await _iVPRepository.GetVpWHTool(_vpParameter);
                return Ok(new ApiOkResponse(shortcutPaneData));
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("GetPmolTeamsForCu")]
    public async Task<ActionResult> GetPmolTeamsForCu([FromBody] GetTeamDto getTeamDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.GetTeamDto = getTeamDto;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            var shortcutPaneData = await _iVPRepository.TeamsForCu(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    [HttpPost("UpdatePersonDate")]
    public async Task<ActionResult> UpdatePersonDate(
        [FromBody] UpdatePersonsDate UpdatePersonsDate,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;

            _vpParameter.UpdatePersonsDate = UpdatePersonsDate;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            var shortcutPaneData = await _iVPRepository.UpdatePersonDate(_vpParameter);


            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("UpdateWHTaxonomyDate")]
    public async Task<ActionResult> UpdateWHTaxonomyDate(
        [FromBody] UpdatePersonsDate UpdatePersonsDate,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;

            _vpParameter.UpdatePersonsDate = UpdatePersonsDate;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            var shortcutPaneData = await _iVPRepository.UpdateWHTaxonomyDate(_vpParameter);


            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("VpHRByPersonId/{PersonId}")]
    public async Task<ActionResult> VpHRByPersonId(string PersonId, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();

            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.Id = PersonId;
            var data = await _iVPRepository.VpHRByPersonId(_vpParameter);


            return Ok(new ApiOkResponse(data));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("GetPbsForVP")]
    public async Task<ActionResult> GetPbsForVP([FromBody] PbsForVPDtoFilter Filter,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromHeader(Name = "isMyEnv")] bool myEnv,[FromHeader(Name = "Iscu")] bool iscu)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.PbsForVPDtoFilter = Filter;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.IsCu = iscu;
            _vpParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            
            if (myEnv || iscu)
            {
                return Ok(new ApiOkResponse(await _iVPRepository.GetPbsForVPMyEnv(_vpParameter, true)));
            }
            else
            {
                return Ok(new ApiOkResponse(await _iVPRepository.GetPbsForVP(_vpParameter, false)));
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("GeTLabourTeamsAndToolsByDate")]
    public async Task<ActionResult> GeTLabourTeamsAndToolsByDate(
        [FromBody] DateFilter DateFilter, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromHeader(Name = "isMyEnv")] bool myEnv)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.DateFilter = DateFilter;

            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            if (myEnv)
            {
                var shortcutPaneData = await _iVPRepository.GeTLabourTeamsAndToolsByDateMyEnv(_vpParameter);
                return Ok(new ApiOkResponse(shortcutPaneData));
            }
            else
            {
                var shortcutPaneData = await _iVPRepository.GeTLabourTeamsAndToolsByDate(_vpParameter);
                return Ok(new ApiOkResponse(shortcutPaneData));
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CreateMilestone")]
    public async Task<ActionResult> CreateMilestone(
        [FromBody] MilestoneHeaderCreateDto MilestoneHeaderCreateDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.MilestoneHeaderCreateDto = MilestoneHeaderCreateDto;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var shortcutPaneData = await _iVPRepository.CreateMilestone(_vpParameter);


            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetMilestoneById/{SequenceId}")]
    public async Task<ActionResult> GetMilestoneById(string SequenceId, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.Id = SequenceId;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            var shortcutPaneData = await _iVPRepository.GetMilestoneById(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("GetVpOrganizationTaxonomyList")]
    public async Task<ActionResult> GetVpOrganizationTaxonomyList(
        [FromBody] OrganizationTaxonomyFilter OrganizationTaxonomyFilter,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;

            _vpParameter.OrganizationTaxonomyFilter = OrganizationTaxonomyFilter;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            var shortcutPaneData = await _iVPRepository.GetVpOrganizationTaxonomyList(_vpParameter);


            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("GetMTPOrganizationTaxonomyList")]
    public async Task<ActionResult> GetMTPOrganizationTaxonomyList(
        [FromBody] OrganizationTaxonomyFilter OrganizationTaxonomyFilter,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;

            _vpParameter.OrganizationTaxonomyFilter = OrganizationTaxonomyFilter;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            var shortcutPaneData = await _iVPRepository.GetMTPOrganizationTaxonomyList(_vpParameter);


            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetMilestoneDropdown")]
    public async Task<ActionResult> GetMilestoneDropdown([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            var shortcutPaneData = await _iVPRepository.GetMilestoneDropdown(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    [HttpPost("AddPmolPlannedLabour")]
    public async Task<ActionResult> AddPmolPlannedLabour(
        [FromBody] AddPmolPlannedWork AddPmolPlannedWork,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;

            _vpParameter.AddPmolPlannedWork = AddPmolPlannedWork;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            var shortcutPaneData = await _iVPRepository.AddPmolPlannedLabour(_vpParameter);


            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    [HttpPost("AddPmolPlannedTools")]
    public async Task<ActionResult> AddPmolPlannedTools(
        [FromBody] AddPmolPlannedWork AddPmolPlannedWork,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;

            _vpParameter.AddPmolPlannedWork = AddPmolPlannedWork;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            var shortcutPaneData = await _iVPRepository.AddPmolPlannedTools(_vpParameter);


            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("MilestoneList")]
    public async Task<ActionResult> MilestoneList([FromBody] MilestoneFilter MilestoneFilter,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.MSFilter = MilestoneFilter;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            var shortcutPaneData = await _iVPRepository.GetMilestoneList(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetMilestoneShortcutPaneData")]
    public async Task<ActionResult> GetMilestoneShortcutPaneData([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project, [FromHeader(Name = "lang")] string langX,
        [FromHeader(Name = "isMyEnv")] bool myEnv,[FromHeader(Name = "isCu")] bool isCu)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            if (myEnv && !isCu)
            {
                var shortcutPaneData = await _iVPRepository.MyEnvMilestoneShortcutPaneData(_vpParameter);
                return Ok(new ApiOkResponse(shortcutPaneData));
            }
            if (isCu)
            {
                var shortcutPaneData = await _iVPRepository.CuMilestoneShortcutPaneData(_vpParameter);
                return Ok(new ApiOkResponse(shortcutPaneData));
            }
            else
            {
                var shortcutPaneData = await _iVPRepository.MilestoneShortcutPaneData(_vpParameter);
                return Ok(new ApiOkResponse(shortcutPaneData));
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CreateMachineTaxonomy")]
    public async Task<ActionResult> CreateMachineTaxonomy(
        [FromBody] MachineTaxonmyDto MachineTaxonmyDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.MachineTaxonmyDto = MachineTaxonmyDto;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var shortcutPaneData = await _iVPRepository.MachineTaxonomyCreate(_vpParameter);


            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetMilestoneListForVp")]
    public async Task<ActionResult> GetMilestoneListForVp([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            var shortcutPaneData = await _iVPRepository.GetMilestoneListAsTaxonomy(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("UpdateBorPbsForVp")]
    public async Task<ActionResult> UpdateBorPbsForVp(
        [FromBody] UpdateBorPbsForVp UpdateBorPbsForVp,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.UpdateBorPbsForVp = UpdateBorPbsForVp;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.PbsResourceRepository = _iPbsResourceRepository;
            var shortcutPaneData = await _iVPRepository.UpdateBorPbsForVp(_vpParameter);


            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetVPFilterDropdownData")]
    public async Task<ActionResult> GetVPFilterDropdownData([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromHeader(Name = "ismyenv")] bool ismyenv)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.IsMyEnv = ismyenv;
            var shortcutPaneData = await _iVPRepository.GetVPFilterDropdownData(_vpParameter);


            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("GetPbsTreeStructureDataForVp")]
    public async Task<ActionResult> GetPbsTreeStructureDataForVp(
        [FromBody] PbsTreeStructureFilter PbsFilter, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromHeader(Name = "IsMyEnv")] bool myEnv,[FromHeader(Name = "Iscu")] bool iscu)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.PbsTreeStructureFilter = PbsFilter;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.PbsResourceRepository = _iPbsResourceRepository;
            _vpParameter.IsCu = iscu;
            _vpParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            if (myEnv || iscu)
            {
                var shortcutPaneData = await _iVPRepository.GetPbsTreeStructureDataForMyEnv(_vpParameter);
                return Ok(new ApiOkResponse(shortcutPaneData));
            }
            else
            {
                var shortcutPaneData = await _iVPRepository.GetPbsTreeStructureDataForVp(_vpParameter);
                return Ok(new ApiOkResponse(shortcutPaneData));
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
            
        }
    }
    
    [HttpPost("GetPbsTreeStructureDataForMTP")]
    public async Task<ActionResult> GetPbsTreeStructureDataForMTP(
        [FromBody] PbsTreeStructureFilter PbsFilter, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromHeader(Name = "IsMyEnv")] bool myEnv,[FromHeader(Name = "Iscu")] bool iscu)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.PbsTreeStructureFilter = PbsFilter;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.IsCu = iscu;
            _vpParameter.MTP = true;
            _vpParameter.PbsResourceRepository = _iPbsResourceRepository;
            _vpParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            if (myEnv || iscu)
            {
                var shortcutPaneData = await _iVPRepository.GetPbsTreeStructureDataForMyEnv(_vpParameter);
                return Ok(new ApiOkResponse(shortcutPaneData));
            }
            else
            {
                var shortcutPaneData = await _iVPRepository.GetPbsTreeStructureDataForVp(_vpParameter);
                return Ok(new ApiOkResponse(shortcutPaneData));
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
            
        }
    }

    [HttpGet("GetPbswithPmol")]
    public async Task<ActionResult> GetPbswithPmol([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromHeader(Name = "isMyEnv")] bool myEnv)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;

            var shortcutPaneData = await _iVPRepository.GetPbswithpmol(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("UpdatePbsProductTaxonomyDataForVp")]
    public async Task<ActionResult> UpdatePbsProductTaxonomyDataForVp(
        [FromBody] PbsProductTaxonomyTree PbsProductTaxonomy,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromHeader(Name = "ismyenv")] bool isMyEnv)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.PbsProductTaxonomyTree = PbsProductTaxonomy;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.PbsResourceRepository = _iPbsResourceRepository;

            // if (isMyEnv)
            // {
            //     _vpParameter.ContractingUnitSequenceId = PbsProductTaxonomy.ContractingUnit;
            //     _vpParameter.ProjectSequenceId = PbsProductTaxonomy.ProjectSequenceCode;
            // }

            var shortcutPaneData = await _iVPRepository.UpdatePbsProductTaxonomyDataForVp(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("PbsCloneForVp")]
    public async Task<ActionResult> PbsCloneForVp(
        [FromBody] PBSCloneForVpDto pbsCloneForVpDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameters = new VPParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                PBSCloneForVpDto = pbsCloneForVpDto,
                ContextAccessor = ContextAccessor,
                TenantProvider = _TenantProvider,
                _iPbsRepository = _iPbsRepository,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };
            var shortcutPaneData = await _iVPRepository.PBSCloneForVpDto(_vpParameters);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("TreeIndex")]
    public async Task<ActionResult> PbsTreeIndex(
        [FromBody] PbsTreeIndexDto PbsTreeIndexDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX,[FromHeader(Name = "lang")] bool IsFilter)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameters = new VPParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                PbsTreeIndexDto = PbsTreeIndexDto,
                ContextAccessor = ContextAccessor,
                TenantProvider = _TenantProvider
            };
            var shortcutPaneData = await _iVPRepository.PbsTreeIndex(_vpParameters);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("DayPlanningFilter")]
    public async Task<ActionResult> DayPlanningFilter(
        [FromBody] DayPlanningFilterDto DayPlanningFilterDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.DayPlanningFilterDto = DayPlanningFilterDto;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var shortcutPaneData = await _iVPRepository.DayPlanningFilter(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("DayPlanningListData")]
    public async Task<ActionResult> DayPlanningListData(
        [FromBody] GetTeamDto getTeamDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromHeader(Name = "isMyEnv")] bool myEnv)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                GetTeamDto = getTeamDto,
                ContextAccessor = ContextAccessor,
                TenantProvider = _TenantProvider,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                _iShiftRepository = _iShiftRepository,
                Configuration = _iConfiguration
            };

            if (myEnv)
            {
                var shortcutPaneData = await _iVPRepository.DayPlanningListData(_vpParameter);
                return Ok(new ApiOkResponse(shortcutPaneData));
            }
            else
            {
                var shortcutPaneData = await _iVPRepository.DayPlanningListDataProject(_vpParameter);
                return Ok(new ApiOkResponse(shortcutPaneData));
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("PmolAssignDayPanning")]
    public async Task<ActionResult> PmolAssignDayPanning(
        [FromBody] PmolAssignDayPanningDto PmolAssignDayPanningDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.PmolAssignDayPanningDto = PmolAssignDayPanningDto;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.Configuration = _iConfiguration;
            _vpParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var shortcutPaneData = await _iVPRepository.PmolAssignDayPanning(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("CopyTeamswithPmol")]
    public async Task<ActionResult> CopyTeamswithPmol()
    {
        try
        {
            var _vpParameter = new VPParameter();
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;

            var shortcutPaneData = await _iVPRepository.CopyTeamswithPmol(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("GetDayPlanningToolsByCu")]
    public async Task<ActionResult> GetDayPlanningToolsByCu(
        [FromBody] DayPlanningToolsFilter DayPlanningToolsFilter,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.DayPlanningToolsFilter = DayPlanningToolsFilter;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var shortcutPaneData = await _iVPRepository.GetDayPlanningToolsByCu(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("PersonAssignDayPlanning")]
    public async Task<ActionResult> PersonAssignDayPlanning(
        [FromBody] PmolAssignDayPanningDto PmolAssignDayPanningDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.PmolAssignDayPanningDto = PmolAssignDayPanningDto;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var shortcutPaneData = await _iVPRepository.PersonAssignDayPlanning(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CreateNewTeamDayPlanning")]
    public async Task<ActionResult> CreateNewTeamDayPlanning(
        [FromBody] PmolAssignDayPanningDto PmolAssignDayPanningDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.PmolAssignDayPanningDto = PmolAssignDayPanningDto;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var shortcutPaneData = await _iVPRepository.CreateNewTeamDayPlanning(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("VehicleAssignDayPlanning")]
    public async Task<ActionResult> VehicleAssignDayPlanning(
        [FromBody] PmolAssignDayPanningDto PmolAssignDayPanningDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.PmolAssignDayPanningDto = PmolAssignDayPanningDto;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var shortcutPaneData = await _iVPRepository.VehicleAssignDayPlanning(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("ToolAssignDayPlanning")]
    public async Task<ActionResult> ToolAssignDayPlanning(
        [FromBody] ToolAssignDayPanningDto ToolAssignDayPanningDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.ToolAssignDayPanningDto = ToolAssignDayPanningDto;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var shortcutPaneData = await _iVPRepository.ToolAssignDayPlanning(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("GetBorForVp")]
    public async Task<ActionResult> GetBorForVp([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromBody] BorVpFilter BorVpFilter)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.BorVpFilter = BorVpFilter;
            var shortcutPaneData = await _iVPRepository.GetBorForVp(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("GetBorForProjectPlanning")]
    public async Task<ActionResult> GetBorForProjectPlanning([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromBody] BorVpFilter BorVpFilter)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.BorVpFilter = BorVpFilter;
            var shortcutPaneData = await _iVPRepository.GetBorForProjectPlanning(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("GetResourceItemsForVp")]
    public async Task<ActionResult> GetResourceItemsForVp([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromBody] BorVpFilter BorVpFilter)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.BorVpFilter = BorVpFilter;
            var shortcutPaneData = await _iVPRepository.GetResourceItemsForVp(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("GetPoForProjectPlanning")]
    public async Task<ActionResult> GetPoForProjectPlanning([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromBody] BorVpFilter BorVpFilter)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.BorVpFilter = BorVpFilter;


            var shortcutPaneData = await _iVPRepository.GetPoForProjectPlanning(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("TeamAssignForDayPlanning")]
    public async Task<ActionResult> TeamAssignForDayPlanning([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromBody] AssignPmolTeam AssignPmolTeam)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.AssignPmolTeam = AssignPmolTeam;

            var shortcutPaneData = await _iVPRepository.TeamAssignForDayPlanning(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("VpShortcutPaneData")]
    public async Task<ActionResult> VpShortcutPaneData([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project, [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            var shortcutPaneData = await _iVPRepository.VpShortcutPaneData(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("ProjectsWithPmol")]
    public async Task<ActionResult> ProjectsWithPmol([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromHeader(Name = "IsMyEnv")] bool myEnv,
        [FromBody] ProjectsPmol ProjectsPmol)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.ProjectsPmol = ProjectsPmol;
            _vpParameter.Configuration = _iConfiguration;
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            _vpParameter.UserId = objectIdentifier;
            _vpParameter._iShiftRepository = _iShiftRepository;
            //_vpParameter.AssignPmolTeam = AssignPmolTeam;

            if (myEnv)
            {
                var shortcutPaneData = await _iVPRepository.ProjectsWithPmol(_vpParameter);
                return Ok(new ApiOkResponse(shortcutPaneData));
            }
            else
            {
                var shortcutPaneData = await _iVPRepository.ProjectsWithPmolProjectLevel(_vpParameter);
                return Ok(new ApiOkResponse(shortcutPaneData));
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("GetProjectsForProjectsDayPlanning")]
    public async Task<ActionResult> GetProjectsForProjectsDayPlanning([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;

            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            _vpParameter.UserId = objectIdentifier;
            //_vpParameter.AssignPmolTeam = AssignPmolTeam;

            var shortcutPaneData = await _iVPRepository.GetProjectsForProjectsDayPlanning(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("DayPlanningFilterForProjectDayPlanning")]
    public async Task<ActionResult> DayPlanningFilterForProjectDayPlanning(
        [FromBody] DayPlanningFilterDto DayPlanningFilterDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.DayPlanningFilterDto = DayPlanningFilterDto;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var shortcutPaneData = await _iVPRepository.DayPlanningFilterForProjectDayPlanning(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("PmolExecutionDateSet")]
    public async Task<ActionResult> PmolExecutionDateSet(
        [FromBody] PmolDrag PmolDrag,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.PmolDrag = PmolDrag;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var shortcutPaneData = await _iVPRepository.PmolExecutionDateSet(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("UpdatePmolStartEndDate")]
    public async Task<ActionResult> UpdatePmolStartEndDate(
        [FromBody] PmolDrag PmolDrag,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.PmolDrag = PmolDrag;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var shortcutPaneData = await _iVPRepository.UpdatePmolStartEndDate(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("AddTeamMemberToProjectPmols")]
    public async Task<ActionResult> AddTeamMemberToProjectPmols(
        [FromBody] AddTeamMember AddTeamMember,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.AddTeamMember = AddTeamMember;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            _vpParameter.Configuration = _iConfiguration;
            var shortcutPaneData = await _iVPRepository.AddTeamMemberToProjectPmols(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("AddTeamMemberToPmol")]
    public async Task<ActionResult> AddTeamMemberToPmol(
        [FromBody] AddTeamMember AddTeamMember,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.AddTeamMember = AddTeamMember;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            _vpParameter.Configuration = _iConfiguration;
            var shortcutPaneData = await _iVPRepository.AddTeamMemberToPmol(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("AddMultipleMembersToPmol")]
    public async Task<ActionResult> AddMultipleMembersToPmol(
        [FromBody] AddMutipleTeamMembers addMutipleTeamMembers,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.AddMutipleTeamMembers = addMutipleTeamMembers;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            _vpParameter.Configuration = _iConfiguration;
            var shortcutPaneData = await _iVPRepository.AddMultipleMembersToPmol(_vpParameter,false);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("AddToolsToProjectPmols")]
    public async Task<ActionResult> AddToolsToProjectPmols(
        [FromBody] AddTools AddTools,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.AddTools = AddTools;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            _vpParameter.Configuration = _iConfiguration;
            _vpParameter.PbsResourceRepository = _iPbsResourceRepository;
            var shortcutPaneData = await _iVPRepository.AddToolsToProjectPmols(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("AddToolsToPmol")]
    public async Task<ActionResult> AddToolsToPmol(
        [FromBody] AddTools AddTools,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.AddTools = AddTools;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            _vpParameter.Configuration = _iConfiguration;
            _vpParameter.PbsResourceRepository = _iPbsResourceRepository;
            var shortcutPaneData = await _iVPRepository.AddToolsToPmol(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    
    [Route("BuFilterForDayPlanning"), HttpPost]
    public async Task<ActionResult> BuFilterForDayPlanningX(
        [FromBody] FilterBu FilterBu,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.FilterBu = FilterBu;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            _vpParameter.Configuration = _iConfiguration;
            _vpParameter.PbsResourceRepository = _iPbsResourceRepository;
            var shortcutPaneData = await _iVPRepository.BuFilterForDayPlanning(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
            
        }
    }

    [HttpPost("CretePrFromBor")]
    public async Task<ActionResult> CretePrFromBor(
        [FromBody] CreatePr CreatePr,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.CreatePr = CreatePr;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            _vpParameter.Configuration = _iConfiguration;
            _vpParameter.PbsResourceRepository = _iPbsResourceRepository;
            _vpParameter.BorRepository = _iBorRepository;
            _vpParameter.ProjectDefinitionRepository = _iProjectDefinitionRepository;
            _vpParameter.uPrinceCustomerContext = UPrinceCustomerContext;
            var shortcutPaneData = await _iVPRepository.CretePrFromBor(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("GenerateRFQForProjectDayPlanning")]
    public async Task<ActionResult> GenerateRFQForProjectDayPlanning([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromHeader(Name = "IsMyEnv")] bool myEnv,
        [FromBody] ProjectsPmol ProjectsPmol)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.ProjectsPmol = ProjectsPmol;
            _vpParameter.Configuration = _iConfiguration;
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            _vpParameter.UserId = objectIdentifier;
            _vpParameter._iShiftRepository = _iShiftRepository;
            _vpParameter.ProjectDefinitionRepository = _iProjectDefinitionRepository;
            _vpParameter.uPrinceCustomerContext = UPrinceCustomerContext;
            //_vpParameter.AssignPmolTeam = AssignPmolTeam;


            // var shortcutPaneData = await _iVPRepository.GenerateRFQForProjectDayPlanning(_vpParameter);
            var shortcutPaneData = await _iVPRepository.GenerateRFQNew(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));

            // else
            // {
            //     var shortcutPaneData = await _iVPRepository.ProjectsWithPmolProjectLevel(_vpParameter);
            //     return Ok(new ApiOkResponse(shortcutPaneData));
            // }
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("DeleteLabourFromProjectDayPlanning")]
    public async Task<ActionResult> DeleteLabourFromProjectDayPlanning([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromBody] DeleteLabour DeleteLabour)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.DeleteLabour = DeleteLabour;
            _vpParameter.Configuration = _iConfiguration;
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            _vpParameter.UserId = objectIdentifier;
            _vpParameter._iShiftRepository = _iShiftRepository;
            _vpParameter.ProjectDefinitionRepository = _iProjectDefinitionRepository;
            _vpParameter.uPrinceCustomerContext = UPrinceCustomerContext;
            //_vpParameter.AssignPmolTeam = AssignPmolTeam;


            var shortcutPaneData = await _iVPRepository.DeleteLabourFromProjectDayPlanning(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));

            // else
            // {
            //     var shortcutPaneData = await _iVPRepository.ProjectsWithPmolProjectLevel(_vpParameter);
            //     return Ok(new ApiOkResponse(shortcutPaneData));
            // }
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpDelete("DeleteVpLabourAssign/{Id}")]
    public async Task<ActionResult> DeleteVpLabourAssign(string Id, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.Id = Id;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            var shortcutPaneData = await _iVPRepository.DeleteVpLabourAssign(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("RemovePerson")]
    public async Task<ActionResult> RemovePerson([FromBody] RemoveLabour RemoveLabour,
        [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.RemoveLabour = RemoveLabour;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            var shortcutPaneData = await _iVPRepository.RemoveLabourNew(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("ProjectSearchMyEnv")]
    public async Task<ActionResult> ProjectSearchMyEnv([FromBody] ProjectSearchMyEnv projectSearchMyEnv,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.ProjectSearchMyEnv = projectSearchMyEnv;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            _vpParameter.Configuration = _iConfiguration;
            var shortcutPaneData = await _iVPRepository.ProjectSearchMyEnv(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CreateNewProjectMyEnv")]
    public async Task<ActionResult> CreateNewProjectMyEnv(
        [FromBody] CreateNewProjectMyEnvDto createProject,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.CreateProject = createProject;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.uPrinceCustomerContext = UPrinceCustomerContext;
            _vpParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            _vpParameter.Configuration = _iConfiguration;
            _vpParameter._iPbsRepository = _iPbsRepository;
            _vpParameter.BorRepository = _iBorRepository;
            _vpParameter._iBorResourceRepositoryRepository = _iBorResourceRepositoryRepository;
            _vpParameter._iCoporateProductCatalogRepository = _iCoporateProductCatalogRepository;
            _vpParameter._iProjectTeamRepository = _iProjectTeamRepository;
            _vpParameter._iProjectDefinitionRepository = _iProjectDefinitionRepository;
            _vpParameter._iCiawRepository = _iCiawRepository;
            _vpParameter._iProjectDefinitionRepository = _iProjectDefinitionRepository;
            _vpParameter._iProjectTimeRepository = _iProjectTimeRepository;
           // _vpParameter._i

            var shortcutPaneData = await _iVPRepository.CreateNewProjectMyEnv(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("UpdatePmolTitleMyEnv")]
    public async Task<ActionResult> UpdatePmolTitleMyEnv(
        [FromBody] UpdatePmolTitle UpdatePmolTitle,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.UpdatePmolTitle = UpdatePmolTitle;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            _vpParameter.Configuration = _iConfiguration;

            var shortcutPaneData = await _iVPRepository.UpdatePmolTitleMyEnv(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("PbsDisplayOrder")]
    public async Task<ActionResult> PbsDisplayOrder(
        [FromBody] List<PbsDisplayOrder> PbsDisplayOrder,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromHeader(Name = "ismyenv")] bool isMyEnv)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.PbsDisplayOrder = PbsDisplayOrder;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            

            var shortcutPaneData = await _iVPRepository.PbsDisplayOrder(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("TreeIndexSiblingAdd")]
    public async Task<ActionResult> TreeIndexSiblingAdd(
        [FromBody] PbsTreeIndexDto PbsTreeIndexDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX,[FromHeader(Name = "lang")] bool IsFilter)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = CU;
            _vpParameter.ProjectSequenceId = Project;
            _vpParameter.Lang = lang;
            _vpParameter.PbsTreeIndexDto = PbsTreeIndexDto;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            var shortcutPaneData = await _iVPRepository.TreeIndexSiblingAdd(_vpParameter);
            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("PbsAssing")]
    public async Task<ActionResult> PbsAssign(
        [FromBody] PbsAssignDto pbsAssignDto,
        [FromHeader(Name = "CU")] string cu, [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX, [FromHeader(Name = "isMyEnv")] bool isMyEnv)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = cu;
            _vpParameter.ProjectSequenceId = project;
            _vpParameter.Lang = lang;
            _vpParameter.PbsAssignDto = pbsAssignDto;
            _vpParameter.IsMyEnv = isMyEnv;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.uPrinceCustomerContext = UPrinceCustomerContext;
            _vpParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            _vpParameter.Configuration = _iConfiguration;
            _vpParameter._iPbsRepository = _iPbsRepository;
            _vpParameter.BorRepository = _iBorRepository;
            _vpParameter._iBorResourceRepositoryRepository = _iBorResourceRepositoryRepository;
            _vpParameter._iCoporateProductCatalogRepository = _iCoporateProductCatalogRepository;
            _vpParameter._iProjectTeamRepository = _iProjectTeamRepository;
            _vpParameter._iProjectDefinitionRepository = _iProjectDefinitionRepository;
            _vpParameter._iCiawRepository = _iCiawRepository;
            _vpParameter._iProjectDefinitionRepository = _iProjectDefinitionRepository;
            _vpParameter._iProjectTimeRepository = _iProjectTimeRepository;

            var id = await _iVPRepository.PbsAssign(_vpParameter);
            return Ok(new ApiOkResponse(id));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("ProjectAssign")]
    public async Task<ActionResult> ProjectAssign(
        [FromBody] ProjectAssignDto projectAssignDto,
        [FromHeader(Name = "CU")] string cu, [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX, [FromHeader(Name = "isMyEnv")] bool isMyEnv)
    {
        try
        {
            var lang = langCode(langX);
            var _vpParameter = new VPParameter();
            _vpParameter.ContractingUnitSequenceId = cu;
            _vpParameter.ProjectSequenceId = project;
            _vpParameter.Lang = lang;
            _vpParameter.ProjectAssignDto = projectAssignDto;
            _vpParameter.IsMyEnv = isMyEnv;
            _vpParameter.ContextAccessor = ContextAccessor;
            _vpParameter.TenantProvider = _TenantProvider;
            _vpParameter.uPrinceCustomerContext = UPrinceCustomerContext;
            _vpParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            _vpParameter.Configuration = _iConfiguration;
            _vpParameter._iPbsRepository = _iPbsRepository;
            _vpParameter.BorRepository = _iBorRepository;
            _vpParameter._iBorResourceRepositoryRepository = _iBorResourceRepositoryRepository;
            _vpParameter._iCoporateProductCatalogRepository = _iCoporateProductCatalogRepository;
            _vpParameter._iProjectTeamRepository = _iProjectTeamRepository;
            _vpParameter._iProjectDefinitionRepository = _iProjectDefinitionRepository;
            _vpParameter._iCiawRepository = _iCiawRepository;
            _vpParameter._iProjectDefinitionRepository = _iProjectDefinitionRepository;
            _vpParameter._iProjectTimeRepository = _iProjectTimeRepository;

            var id = await _iVPRepository.ProjectAssign(_vpParameter);
            return Ok(new ApiOkResponse(id));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("ProjectSearchForVp")]
    public async Task<ActionResult> ProjectSearchForVp(
        [FromBody] ProjectSearchForVpDto projectSearchForVp,
        [FromHeader(Name = "CU")] string cu, [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX, [FromHeader(Name = "isMyEnv")] bool isMyEnv)
    {
        try
        {
            var mVpParameter = new VPParameter();

            mVpParameter.ProjectSearchForVpDto = projectSearchForVp;
            mVpParameter.ContextAccessor = ContextAccessor;
            mVpParameter.TenantProvider = _TenantProvider;
            mVpParameter.uPrinceCustomerContext = UPrinceCustomerContext;
            mVpParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            mVpParameter.Configuration = _iConfiguration;
            
            var id = await _iVPRepository.ProjectSearchForVp(mVpParameter);
            return Ok(new ApiOkResponse(id));
            
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }
}