using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Repositories.Interfaces.PBS;
using UPrinceV4.Web.UserException;

namespace UPrinceV4.Web.Controllers;


[Authorize]
[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class ProjectDefinitionController : CommonConfigurationController
{
    private readonly ICurrencyRepository _currencyRepository;
    private readonly IPbsRepository _IPbsRepository;
    private readonly IProjectDefinitionRepository _iProjectDefinitionRepository;
    private readonly IProjectFinanceRepository _iProjectFinanceRepository;
    private readonly IProjectTeamRepository _iProjectTeamRepository;
    private readonly IProjectTimeRepository _iProjectTimeRepository;
    private readonly IProjectManagementLevelRepository _projectManagementLevelRepository;
    private readonly IProjectStateRepository _projectStateRepository;
    private readonly IProjectTemplateRepository _projectTemplateRepository;
    private readonly IProjectToleranceStateRepository _projectToleranceStateRepository;
    private readonly IProjectTypeRepository _projectTypeRepository;
    private IConfiguration _iConfiguration { get; }



    public ProjectDefinitionController(
        IProjectDefinitionRepository iProjectDefinitionRepository,
        IProjectTimeRepository iProjectTimeRepository,
        IProjectFinanceRepository iProjectFinanceRepository,
        IProjectManagementLevelRepository projectManagementLevelRepository,
        IProjectToleranceStateRepository projectToleranceStateRepository,
        IProjectTypeRepository projectTypeRepository,
        IProjectStateRepository projectStateRepository,
        IProjectTemplateRepository projectTemplateRepository,
        ICurrencyRepository currencyRepository,
        IProjectTeamRepository projectTeamRepository,
        IPbsRepository iPbsRepository,
        IConfiguration iConfiguration,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)

    {
        _iProjectDefinitionRepository = iProjectDefinitionRepository;
        _iProjectTimeRepository = iProjectTimeRepository;
        _iProjectFinanceRepository = iProjectFinanceRepository;
        _projectManagementLevelRepository = projectManagementLevelRepository;
        _projectToleranceStateRepository = projectToleranceStateRepository;
        _projectTypeRepository = projectTypeRepository;
        _projectStateRepository = projectStateRepository;
        _projectTemplateRepository = projectTemplateRepository;
        _currencyRepository = currencyRepository;
        _iProjectTeamRepository = projectTeamRepository;
        _IPbsRepository = iPbsRepository;
        _iConfiguration = iConfiguration;

    }


    [HttpGet("Read")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetProjectDefinition()
    {
        try
        {
            //var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var projectDefinitions =
                await _iProjectDefinitionRepository.GetProjectDefinition(UPrinceCustomerContext);
            return Ok(!projectDefinitions.Any()
                ? new ApiResponse(200, false, "noAvailableProjects")
                : new ApiOkResponse(projectDefinitions));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadById/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetProjectDefinitionById(string id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var cu = langCode(Request.Headers["cu"].FirstOrDefault());
            var _iProjectDefinitionParameter = new ProjectDefinitionParameter();
            _iProjectDefinitionParameter.Lang = lang;
            _iProjectDefinitionParameter.ApplicationDbContext = UPrinceCustomerContext;
            _iProjectDefinitionParameter.Id = id;
            _iProjectDefinitionParameter.TenantProvider = ItenantProvider;
            _iProjectDefinitionParameter.ContractingUnitSequenceId = cu;

            // var projectDefinition =
            //     await _iProjectDefinitionRepository.GetProjectDefinitionById(UPrinceCustomerContext, id, lang,
            //         ItenantProvider);
            var projectDefinition =
                await _iProjectDefinitionRepository.GetProjectDefinitionByIdNew(_iProjectDefinitionParameter);
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = false;

            return Ok(projectDefinition == null ? ApiResponse : new ApiOkResponse(projectDefinition));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadLocationByProjectId/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ReadLocationByProjectId(string id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var projectDefinition =
                await _iProjectDefinitionRepository.GetLocationByProjectId( id, lang,
                    ItenantProvider);
            return Ok(projectDefinition == null
                ? new ApiResponse(200, false, "noLocationForTheId")
                : new ApiOkResponse(projectDefinition));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadProjectDataById/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ReadProjectDataById(string id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var projectData =
                await _iProjectDefinitionRepository.GetProjectDataById(UPrinceCustomerContext, id, lang,
                    ItenantProvider);
            return Ok(projectData == null
                ? new ApiResponse(400, false, "projectNotAvailable")
                : new ApiOkResponse(projectData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    //[Microsoft.AspNetCore.Mvc.HttpPost("Create")]
    [HttpPost("Create")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateProjectDefinition(
        [FromBody] ProjectDefinitionCreateDto projectDto)
    {
        try
        {
            // if (!ModelState.IsValid)
            // {
            //     ApiBadRequestResponse.ModelState = ModelState;
            //     return BadRequest(ApiBadRequestResponse);
            // }

            projectDto._iConfiguration = _iConfiguration;
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var user = UPrinceCustomerContext.ApplicationUser
                .FirstOrDefault(u => u.OId == objectIdentifier);

            return Ok(new ApiOkResponse( await _iProjectDefinitionRepository.CreateProjectDefinition(UPrinceCustomerContext,
                projectDto, _iProjectTimeRepository, _iProjectFinanceRepository, _iProjectTeamRepository,
                ItenantProvider, user, _IPbsRepository)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

 
    [HttpPut("Update")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateProjectDefinition(
        [FromBody] ProjectDefinitionUpdateDto projectDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

           // var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var user = UPrinceCustomerContext.ApplicationUser
                .FirstOrDefault(u => u.OId == objectIdentifier);
            var projectId = await _iProjectDefinitionRepository.UpdateProjectDefinition(UPrinceCustomerContext,
                projectDto, _iProjectTimeRepository, _iProjectFinanceRepository, _iProjectTeamRepository,
                ItenantProvider, user);

            return Ok(new ApiOkResponse(projectId));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    
    [HttpDelete("Delete/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteProjectDefinition(string id)
    {
        try
        {
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var user = UPrinceCustomerContext.ApplicationUser.Where(u => u.OId == objectIdentifier)
                .FirstOrDefault();
            await _iProjectDefinitionRepository.DeleteProjectDefinition(UPrinceCustomerContext, id, user);

            return Ok(new ApiResponse(200, false, "projectDeletedSuccessfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    //[Microsoft.AspNetCore.Mvc.HttpGet("ReadHistory")]
    [HttpGet("ReadHistory")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetProjectDefinitionsHistory()
    {
        return Ok(new ApiOkResponse(
            await _iProjectDefinitionRepository.GetProjectDefinitionsHistoryLog(UPrinceCustomerContext)));
    }

    //[Microsoft.AspNetCore.Mvc.HttpPost("ProjectFilter")]
    [HttpPost("ProjectFilter")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Filter([FromBody] ProjectFilter filter)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var projectDefinitions =
                await _iProjectDefinitionRepository.Filter(UPrinceCustomerContext, filter, lang, ItenantProvider);
            if (projectDefinitions.Any())
            {
                var mApiOkResponse = new ApiOkResponse(projectDefinitions);
                return Ok(mApiOkResponse);
            }

            var mApiResponse = new ApiResponse(200, false, "noAvailableProjects");
            return Ok(mApiResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    //[Microsoft.AspNetCore.Mvc.HttpGet("ReadProjectDropdowns")]
    [HttpGet("ReadProjectDropdowns")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetProjectDropdowns()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var ProjectSqCode = Request.Headers["Project"].FirstOrDefault();
            var Cu = Request.Headers["Cu"].FirstOrDefault();

            var projectDropdownLists = await _iProjectDefinitionRepository.GetDropdownData(UPrinceCustomerContext,
                lang, _projectManagementLevelRepository, _projectToleranceStateRepository,
                _projectTypeRepository, _projectStateRepository, _projectTemplateRepository, _currencyRepository,
                ItenantProvider, ProjectSqCode, Cu);
            return Ok(new ApiOkResponse(projectDropdownLists));
        }
        catch (EmptyListException ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPut("UpdateProjectViewTime/{projectId}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateProjectViewTime(string projectId)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var user = UPrinceCustomerContext.ApplicationUser.Where(u => u.OId == objectIdentifier)
                .FirstOrDefault();

            return Ok(new ApiOkResponse(
                await _iProjectDefinitionRepository.UpdateProjectViewTime(UPrinceCustomerContext, projectId, user,
                    lang, ItenantProvider)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    //[Microsoft.AspNetCore.Mvc.HttpGet("ReadLastSeenProjects/{numberOfRecords}")]
    [HttpGet("ReadLastSeenProjects/{numberOfRecords}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ReadLastSeenProjects(int numberOfRecords)
    {
        try
        {
            var oid = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var user = UPrinceCustomerContext.ApplicationUser
                .FirstOrDefault(u => u.OId == oid);
            return Ok(new ApiOkResponse(
                await _iProjectDefinitionRepository.ReadLastSeenProjects(UPrinceCustomerContext, numberOfRecords,
                    user?.Id, ItenantProvider, oid)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    //[Microsoft.AspNetCore.Mvc.HttpGet("ReadProjectsForQr")]
    [HttpGet("ReadProjectsForQr/{title}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetProjectsForQr(string title)
    {
        try
        {
            var projectDefinitions =
                await _iProjectDefinitionRepository.ProjectsForQr(UPrinceCustomerContext, ItenantProvider, title);
            return Ok(!projectDefinitions.Any()
                ? new ApiResponse(200, false, "NoAvailableProjects")
                : new ApiOkResponse(projectDefinitions));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

   
    [HttpPost("UpdateVAT")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateVAT(
        [FromBody] ProjectDefinitionVATDto projectDefinitionVATDto)
    {
        if (projectDefinitionVATDto.Id == null) return BadRequest(new ApiResponse(400, false, "Id cannot be null"));

        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var projectDefinitions = await _iProjectDefinitionRepository.ProjectsUpdateVAT(UPrinceCustomerContext,
                ItenantProvider, projectDefinitionVATDto);
            if (projectDefinitions.Any())
                return Ok(new ApiOkResponse(projectDefinitions));
            return Ok(new ApiResponse(200, false, "noAvailableProjects"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

   
    [HttpPost("AddProjectConfiguration")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> AddProjectConfiguration(
        [FromBody] ProjectConfigurationDto ProjectConfiguration)
    {
        if (ProjectConfiguration.ProjectId == null) return BadRequest(new ApiResponse(400, false, "Id cannot be null"));

        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var projectDefinitions = await _iProjectDefinitionRepository.AddProjectConfiguration(UPrinceCustomerContext,
                ItenantProvider, ProjectConfiguration);
            return Ok(new ApiOkResponse(projectDefinitions));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
   
    [HttpPost("CreateTeam")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateTeam([FromBody] CreateTeamDto createTeamDto)
    {

        try
        {
            var oid = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var result = await _iProjectDefinitionRepository.CreateTeam(UPrinceCustomerContext,
                ItenantProvider,createTeamDto,oid );
            return Ok(new ApiOkResponse(result));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
   
}