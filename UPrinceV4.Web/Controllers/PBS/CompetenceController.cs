using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces.PBS;
using UPrinceV4.Web.UserException;

namespace UPrinceV4.Web.Controllers.PBS;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class CompetenceController : CommonConfigurationController
{
    private readonly ICompetenciesRepository _iCompetenciesRepository;
    private readonly IPbsExperienceRepository _iPbsExperienceRepository;
    private readonly IPbsSkillRepository _iPbsSkillRepository;
    private readonly PbsExperienceRepositoryParameter _pbsExperienceRepositoryParameter;
  

    public CompetenceController(ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor,
        ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider
        , IPbsExperienceRepository iPbsExperienceRepository, IPbsSkillRepository iPbsSkillRepository,
        ICompetenciesRepository iCompetenciesRepository,
        PbsExperienceRepositoryParameter pbsExperienceRepositoryParameter)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)
    {
        _iPbsExperienceRepository = iPbsExperienceRepository;
        _iPbsSkillRepository = iPbsSkillRepository;
        _iCompetenciesRepository = iCompetenciesRepository;
        _pbsExperienceRepositoryParameter = pbsExperienceRepositoryParameter;
    }

    [HttpPost("CreateCompetence")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateCompetence([FromBody] CompetenciesDto competence)
    {
        try
        {
            var pbsSkillExp = new PbsSkillExperience
            {
                Id = competence.Id,
                PbsExperienceId = competence.ExperienceId,
                PbsProductId = competence.PbsProductId,
                PbsSkillId = competence.SkillId
            };
            var _competenciesRepositoryParameter = new CompetenciesRepositoryParameter
                {
                    ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                    ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                    TenantProvider = ItenantProvider,
                    ApplicationDbContext = UPrinceCustomerContext,
                    PbsSkillExperience = pbsSkillExp
                };
            return Ok(new ApiOkResponse(
                await _iCompetenciesRepository.AddCompetence(_competenciesRepositoryParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetCompetenceDropdowns")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetCompetenceDropdowns()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _competenciesRepositoryParameter = new CompetenciesRepositoryParameter
            {
                TenantProvider = ItenantProvider,
                ApplicationDbContext = UPrinceCustomerContext,
                Lang = lang
            };
            var _pbsSkillRepositoryParameter = new PbsSkillRepositoryParameter
            {
                TenantProvider = ItenantProvider,
                ApplicationDbContext = UPrinceCustomerContext,
                Lang = lang
            };
            _competenciesRepositoryParameter.PbsSkillRepositoryParameter = _pbsSkillRepositoryParameter;
            _competenciesRepositoryParameter.IPbsExperienceRepository = _iPbsExperienceRepository;

            _pbsExperienceRepositoryParameter.TenantProvider = ItenantProvider;
            _pbsExperienceRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            _pbsExperienceRepositoryParameter.Lang = lang;
            _competenciesRepositoryParameter.PbsExperienceRepositoryParameter = _pbsExperienceRepositoryParameter;
            _competenciesRepositoryParameter.IPbsSkillRepository = _iPbsSkillRepository;

            return Ok(new ApiOkResponse(
                await _iCompetenciesRepository.GetCompetenciesDropdownData(_competenciesRepositoryParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadCompetenceList")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetCompetenceList()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _competenciesRepositoryParameter = new CompetenciesRepositoryParameter
                {
                    ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                    ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                    TenantProvider = ItenantProvider,
                    ApplicationDbContext = UPrinceCustomerContext,
                    Lang = lang
                };

            var competenceList = await _iCompetenciesRepository.GetCompetencies(_competenciesRepositoryParameter);

            if (competenceList == null || !competenceList.Any())
                return Ok(new ApiOkResponse(competenceList, "NoAvailableRisk"));
            return Ok(new ApiOkResponse(competenceList));
        }
        catch (EmptyListException ex)
        {
            return BadRequest(new ApiResponse(200, false, ex.Message));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadCompetence/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetCompetenceById(string id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _competenciesRepositoryParameter = new CompetenciesRepositoryParameter
                {
                    ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                    ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                    TenantProvider = ItenantProvider,
                    ApplicationDbContext = UPrinceCustomerContext,
                    Lang = lang,
                    Id = id
                };

            var risk = await _iCompetenciesRepository.GetCompetenceById(_competenciesRepositoryParameter);

            return Ok(risk == null ? new ApiOkResponse(null, "noAvailableForCompetenceId") : new ApiOkResponse(risk));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadCompetenceByPbsId/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetCompetenceByPbsId(string id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _competenciesRepositoryParameter = new CompetenciesRepositoryParameter
                {
                    ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                    ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                    TenantProvider = ItenantProvider,
                    ApplicationDbContext = UPrinceCustomerContext,
                    Lang = lang,
                    PbsId = id
                };

            var risk = await _iCompetenciesRepository.GetCompetenceByPbsId(_competenciesRepositoryParameter);
            return Ok(risk == null ? new ApiOkResponse(null, "noAvailableRiskForTheId") : new ApiOkResponse(risk));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpDelete("DeleteCompetence")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteCompetence([FromBody] List<string> idList)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _competenciesRepositoryParameter = new CompetenciesRepositoryParameter
                {
                    ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                    ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                    TenantProvider = ItenantProvider,
                    ApplicationDbContext = UPrinceCustomerContext,
                    IdList = idList
                };

            await _iCompetenciesRepository.DeleteCompetencies(_competenciesRepositoryParameter);
            return Ok(new ApiOkResponse("ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}