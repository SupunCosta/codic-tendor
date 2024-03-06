using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.WBS;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Repositories.Interfaces.PBS;

namespace UPrinceV4.Web.Controllers.WBS;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]

public class WbsController : CommonConfigurationController
{
    private readonly WbsParameter _wbsParameter;
    private readonly IWbsRepository _iWbsRepository;
    private readonly IPbsRepository _iPbsRepository;
    private readonly IPbsInstructionsRepository _iPbsInstructionsRepository;
    private readonly IIssueRepository _iIssueRepository;

    
    public WbsController(ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse, 
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse,IPbsInstructionsRepository iPbsInstructionsRepository,
        ITenantProvider iTenantProvider,IWbsRepository iWbsRepository,WbsParameter wbsParameter,IPbsRepository iPbsRepository,IIssueRepository iIssueRepository) : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse, iTenantProvider)
    {
        _iWbsRepository = iWbsRepository;
        _wbsParameter = wbsParameter;
        _iPbsRepository = iPbsRepository;
        _iPbsInstructionsRepository = iPbsInstructionsRepository;
        _iIssueRepository = iIssueRepository;

    }
    
    [HttpPost("CreateWbs")]
    public async Task<ActionResult> CreateWbs(
        [FromBody] WbsTaxonomy wbsTaxonomy,
        [FromHeader(Name = "CU")] string cu, [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            
            
            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                WbsTaxonomy = wbsTaxonomy,
                TenantProvider = ItenantProvider,
                IPbsRepository = _iPbsRepository,
                iIssueRepository = _iIssueRepository

            };
            var result = await _iWbsRepository.CreateWbs(wbsParam);
            return Ok(new ApiOkResponse(result, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("CreateTemplate")]
    public async Task<ActionResult> CreateTemplate(
        [FromBody] WbsTemplate wbsTemplate,
        [FromHeader(Name = "CU")] string cu, [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            
            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                WbsTemplate = wbsTemplate,
                TenantProvider = ItenantProvider
            };
            
            var result = await _iWbsRepository.CreateTemplate(wbsParam);
            return Ok(new ApiOkResponse(result, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("WbsFilter")]
    public async Task<ActionResult> WbsFilter(
        [FromBody] WbsFilterDto wbsFilter,
        [FromHeader(Name = "CU")] string cu, [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            
            
            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                WbsFilter = wbsFilter,
                TenantProvider = ItenantProvider
            };
            var result = await _iWbsRepository.WbsFilter(wbsParam);
            return Ok(new ApiOkResponse(result, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpGet("GetWbsDropdown")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetWbsDropdown([FromHeader(Name = "CU")] string cu,
        [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            
            
            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                TenantProvider = ItenantProvider
            };
            return Ok(new ApiOkResponse(await _iWbsRepository.GetWbsDropdown(wbsParam)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpGet("GetWbsById/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetWbsById([FromHeader(Name = "CU")] string cu,
        [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX, string id)
    {
        try
        {
            var lang = langCode(langX);

            
            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                Id = id,
                TenantProvider = ItenantProvider
            };
            return Ok(new ApiOkResponse(await _iWbsRepository.GetWbsById(wbsParam)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpGet("WbsTaxonomyList")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> WbsTaxonomyList([FromHeader(Name = "CU")] string cu,
        [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                TenantProvider = ItenantProvider
            };
           
            return Ok(new ApiOkResponse(await _iWbsRepository.WbsTaxonomyList(wbsParam)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpGet("GetWbsTaxonomyByTemplate/{id}")]
    public async Task<ActionResult> GetWbsTaxonomyByTemplate([FromHeader(Name = "CU")] string cu,
        [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX, string id)
    {
        try
        {
            var lang = langCode(langX);
            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                Id = id,
                TenantProvider = ItenantProvider
            };

            return Ok(new ApiOkResponse(await _iWbsRepository.GetWbsTaxonomyByTemplate(wbsParam)));

            
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("WbsProductFilter")]
    public async Task<ActionResult> WbsProductFilter(
        [FromBody] WbsProductFilterDto wbsProductFilterDto,
        [FromHeader(Name = "CU")] string cu, [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                WbsProductFilter = wbsProductFilterDto,
                IPbsRepository = _iPbsRepository,
                TenantProvider = ItenantProvider
            };
            
            var result = await _iWbsRepository.WbsProductFilter(wbsParam);
            return Ok(new ApiOkResponse(result, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("WbsTaskCreate")]
    public async Task<ActionResult> WbsTaskCreate(
        [FromBody] WbsTaskCreate wbsTaskCreate,
        [FromHeader(Name = "CU")] string cu, [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                wbsTaskCreate = wbsTaskCreate,
                IPbsRepository = _iPbsRepository,
                TenantProvider = ItenantProvider
            };
            var result = await _iWbsRepository.WbsTaskCreate(wbsParam);
            return Ok(new ApiOkResponse(result, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("WbsTaskGetById")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> WbsTaskGetById([FromHeader(Name = "CU")] string cu,
        [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX, [FromBody] TaskIsFavouriteDto dto)
    {
        try
        {
            var lang = langCode(langX);

            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                iPbsInstructionsRepository = _iPbsInstructionsRepository,
                TenantProvider = ItenantProvider,
                Id = dto.Id,
                TaskIsFavouriteDto = dto
                
            };
            
            return Ok(new ApiOkResponse(await _iWbsRepository.WbsTaskGetById(wbsParam)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpDelete("WbsTaskDelete/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> WbsTaskDelete([FromHeader(Name = "CU")] string cu,
        [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX, string id)
    {
        try
        {
            var lang = langCode(langX);
            
            
            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider,
                Id = id,
                
            };
            return Ok(new ApiOkResponse(await _iWbsRepository.WbsTaskDelete(wbsParam)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [Route("WbsTaskDocumentsDelete")]
    [HttpDelete]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> WbsTaskDocumentsDelete([FromHeader(Name = "CU")] string cu,
        [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX, List<string> idList)
    {
        try
        {
            var lang = langCode(langX);
            
            
            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider,
                IdList = idList,
                
            };
            return Ok(new ApiOkResponse(await _iWbsRepository.WbsTaskDocumentsDelete(wbsParam)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [Route("WbsProductDocumentsDelete")]
    [HttpDelete]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> WbsProductDocumentsDelete([FromHeader(Name = "CU")] string cu,
        [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX, List<string> idList)
    {
        try
        {
            var lang = langCode(langX);
            
            
            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider,
                IdList = idList,
                
            };
            return Ok(new ApiOkResponse(await _iWbsRepository.WbsProductDocumentsDelete(wbsParam)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("WbsTaskGroupFilter")]
    public async Task<ActionResult> WbsTaskGroupFilter(
        [FromBody] WbsTaskFilterDto wbsTaskFilterDto,
        [FromHeader(Name = "CU")] string cu, [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            
            
            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider,
                wbsTaskFilterDto = wbsTaskFilterDto,
                IPbsRepository = _iPbsRepository
                
            };
            if (project != null)
            {
                var result = await _iWbsRepository.WbsTaskGroupFilter(wbsParam);
                return Ok(new ApiOkResponse(result, "Ok"));
            }
            else
            {
                var result = await _iWbsRepository.WbsTaskAllProjectsGroupFilter(wbsParam);
                return Ok(new ApiOkResponse(result, "Ok"));
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    [HttpPost("WbsTaskFilter")]
    public async Task<ActionResult> WbsTaskFilter(
        [FromBody] WbsTaskFilterDto wbsTaskFilterDto,
        [FromHeader(Name = "CU")] string cu, [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            
            
            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider,
                wbsTaskFilterDto = wbsTaskFilterDto,
                IPbsRepository = _iPbsRepository
                
            };
            return Ok(new ApiOkResponse( await _iWbsRepository.WbsTaskFilter(wbsParam), "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("GetWbsTaskInstructionList")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetWbsTaskInstructionList([FromHeader(Name = "CU")] string cu,
        [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX, string id, [FromBody] GetWbsTaskInstructionList dto)
    {
        try
        {
            var lang = langCode(langX);

            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider,
                getWbsTaskInstructionList = dto,
                Id = id,
                iPbsInstructionsRepository = _iPbsInstructionsRepository
                
            };
            return Ok(new ApiOkResponse(await _iWbsRepository.GetWbsTaskInstructionList(wbsParam)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpGet("GetWbsList")]
    
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetWbsList([FromHeader(Name = "CU")] string cu,
        [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider,

            };
            return Ok(new ApiOkResponse(await _iWbsRepository.GetWbsList(wbsParam)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("WbsProductCreate")]
    public async Task<ActionResult> WbsProductCreate(
        [FromBody] WbsProductCreate wbsProductCreate,
        [FromHeader(Name = "CU")] string cu, [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            
            
            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider,
                wbsProductCreate = wbsProductCreate,
                IPbsRepository = _iPbsRepository
            };
            var result = await _iWbsRepository.WbsProductCreate(wbsParam);
            return Ok(new ApiOkResponse(result, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpGet("WbsProductGetById/{id}")]
    
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> WbsProductGetById([FromHeader(Name = "CU")] string cu,
        [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX, string id)
    {
        try
        {
            var lang = langCode(langX);
            
            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider,
                iPbsInstructionsRepository = _iPbsInstructionsRepository,
                Id = id
            };
            return Ok(new ApiOkResponse(await _iWbsRepository.WbsProductGetById(wbsParam)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("WbsTaskIsFavourite")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> WbsTaskIsFavourite([FromHeader(Name = "CU")] string cu,
        [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX,[FromBody] TaskIsFavouriteDto dto)
    {
        try
        {
            var lang = langCode(langX);
            
            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider,
                iPbsInstructionsRepository = _iPbsInstructionsRepository,
                TaskIsFavouriteDto = dto
            };
            return Ok(new ApiOkResponse(await _iWbsRepository.WbsTaskIsFavourite(wbsParam)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpDelete("WbsProductDelete/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> WbsProductDelete([FromHeader(Name = "CU")] string cu,
        [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX, string id)
    {
        try
        {
            var lang = langCode(langX);

            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider,
                Id = id
            };
            return Ok(new ApiOkResponse(await _iWbsRepository.WbsProductDelete(wbsParam)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpGet("CopyWbsToProject/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CopyWbsToProject([FromHeader(Name = "CU")] string cu,
        [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX, string id)
    {
        try
        {
            var lang = langCode(langX);

            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider,
                iPbsInstructionsRepository = _iPbsInstructionsRepository,
                Id = id
            };
            return Ok(new ApiOkResponse(await _iWbsRepository.CopyWbsToProject(wbsParam)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("WbsEditTemplateName")]
    public async Task<ActionResult> WbsEditTemplateName(
        [FromBody] WbsTemplateUpdate wbsTemplateUpdate,
        [FromHeader(Name = "CU")] string cu, [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider,
                IPbsRepository = _iPbsRepository,
                WbsTemplateUpdate = wbsTemplateUpdate
            };
            var result = await _iWbsRepository.WbsEditTemplateName(wbsParam);
            return Ok(new ApiOkResponse(result, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("CreateWbsConversation")]
    public async Task<ActionResult> CreateWbsConversation(
        [FromBody] List<WbsConversationDto> dto,
        [FromHeader(Name = "CU")] string cu, [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider,
                IPbsRepository = _iPbsRepository,
                WbsConversationDto = dto
            };
            var result = await _iWbsRepository.CreateWbsConversation(wbsParam);
            return Ok(new ApiOkResponse(result, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpGet("GetWbsConversation/{Id}")]
    public async Task<ActionResult> GetWbsConversation([FromHeader(Name = "CU")] string cu, [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX,string Id)
    {
        try
        {
            var lang = langCode(langX);

            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider,
                IPbsRepository = _iPbsRepository,
                Id = Id
            };
            var result = await _iWbsRepository.GetWbsConversation(wbsParam);
            return Ok(new ApiOkResponse(result, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("GetWbChecklistById")]
    public async Task<ActionResult> GetWbChecklistById([FromHeader(Name = "CU")] string cu, [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX,[FromBody] TaskIsFavouriteDto dto)
    {
        try
        {
            var lang = langCode(langX);

            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider,
                IPbsRepository = _iPbsRepository,
                Id = dto.Id
            };
            var result = await _iWbsRepository.GetWbChecklistById(wbsParam);
            return Ok(new ApiOkResponse(result, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("AddWbsProductEmails")]
    public async Task<ActionResult> AddWbsProductEmails([FromHeader(Name = "CU")] string cu, [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX,[FromBody] WbsProductEmai dto)
    {
        try
        {
            var lang = langCode(langX);

            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider,
                IPbsRepository = _iPbsRepository,
                WbsProductEmai = dto
            };
            var result = await _iWbsRepository.AddWbsProductEmails(wbsParam);
            return Ok(new ApiOkResponse(result, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpGet("GetTagList")]
    public async Task<ActionResult> GetTagList([FromHeader(Name = "CU")] string cu, [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider,
                IPbsRepository = _iPbsRepository,
            };
            var result = await _iWbsRepository.GetTagList(wbsParam);
            return Ok(new ApiOkResponse(result, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("WbsDragAndDrop")]
    public async Task<ActionResult> WbsDragAndDrop(
        [FromBody] WbsDragAndDrop wbsDragAndDrop,
        [FromHeader(Name = "CU")] string cu, [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider,
                WbsDragAndDrop = wbsDragAndDrop,
                IPbsRepository = _iPbsRepository
            };
            var result = await _iWbsRepository.WbsDragAndDrop(wbsParam);
            return Ok(new ApiOkResponse(result, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("WbsTaskStatusUpdate")]
    public async Task<ActionResult> WbsTaskStatusUpdate(
        [FromBody]TaskStatusUpdateDto dto,
        [FromHeader(Name = "CU")] string cu, [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            
            
            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider,
                IPbsRepository = _iPbsRepository,
                TaskStatusUpdateDto = dto
            };
            var result = await _iWbsRepository.WbsTaskStatusUpdate(wbsParam);
            return Ok(new ApiOkResponse(result, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("UploadWbsDocument")]
    public async Task<ActionResult> UploadWbsDocument(
        [FromForm] string uploadWbsDocumentDto,[FromForm] IFormCollection doc,
        [FromHeader(Name = "CU")] string cu, [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            
            var dto = JsonConvert.DeserializeObject<UploadWbsDocumentDto>(uploadWbsDocumentDto);
            var file = doc.Files.ToList();
            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider,
                IPbsRepository = _iPbsRepository,
                UploadWbsDocumentDto = dto,
                doc = file
            };
            var result = await _iWbsRepository.UploadWbsDocument(wbsParam);
            return Ok(new ApiOkResponse(result, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        
        }
    }
    
    [HttpPost("UploadWbsDocumentMetaData")]
    public async Task<ActionResult> UploadWbsDocumentMetaData(
        [FromBody] UploadWbsDocumentDto uploadWbsDocumentDto,
        [FromHeader(Name = "CU")] string cu, [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider,
                IPbsRepository = _iPbsRepository,
                UploadWbsDocumentDto = uploadWbsDocumentDto
            };
            var result = await _iWbsRepository.UploadWbsDocumentMetaData(wbsParam);
            return Ok(new ApiOkResponse(result, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("UpdateWbsDocumentUploadData")]
    public async Task<ActionResult> UpdateWbsDocumentUploadData(
        [FromBody] UpdateWbsDocumentDto updateWbsDocumentDto,
        [FromHeader(Name = "CU")] string cu, [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider,
                IPbsRepository = _iPbsRepository,
                UpdateWbsDocumentDto = updateWbsDocumentDto
            };
            var result = await _iWbsRepository.UpdateWbsDocumentUploadData(wbsParam);
            return Ok(new ApiOkResponse(result, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("GetWbsDocument")]
    public async Task<ActionResult> GetWbsDocument([FromBody] UploadWbsDocumentDto uploadWbsDocumentDto,
        [FromHeader(Name = "CU")] string cu, [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            
            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider,
                IPbsRepository = _iPbsRepository,
                UploadWbsDocumentDto = uploadWbsDocumentDto
            };
            var result = await _iWbsRepository.GetWbsDocument(wbsParam);
            return Ok(new ApiOkResponse(result, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("GetWbsDocumentIdByUrl")]
    public async Task<ActionResult> GetWbsDocumentIdByUrl([FromBody] GetWbsDocumentIdByUrlDto getWbsDocumentIdByUrl,
        [FromHeader(Name = "CU")] string cu, [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            
            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider,
                IPbsRepository = _iPbsRepository,
                GetWbsDocumentIdByUrlDto = getWbsDocumentIdByUrl
            };
            var result = await _iWbsRepository.GetWbsDocumentIdByUrl(wbsParam);
            return Ok(new ApiOkResponse(result, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("GetWbsDocumentIdByMailId")]
    public async Task<ActionResult> GetWbsDocumentIdByMailId([FromBody] UrlDto urlDto,
        [FromHeader(Name = "CU")] string cu, [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            
            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider,
                IPbsRepository = _iPbsRepository,
                UrlDto = urlDto
            };
            var result = await _iWbsRepository.GetWbsDocumentIdByMailId(wbsParam);
            return Ok(new ApiOkResponse(result, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }
    
    [HttpPost("GetWbsDocumentIdByTaskId")]
    public async Task<ActionResult> GetWbsDocumentIdByTaskId([FromBody] GetWbsDocumentIdByTaskId getWbsDocumentIdByTaskId,
        [FromHeader(Name = "CU")] string cu, [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            
            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider,
                IPbsRepository = _iPbsRepository,
                GetWbsDocumentIdByTaskId = getWbsDocumentIdByTaskId
            };
            var result = await _iWbsRepository.GetWbsDocumentIdByTaskId(wbsParam);
            return Ok(new ApiOkResponse(result, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }
    
    [HttpPost("ReadWbsDocumentIdByUrl")]
    public async Task<ActionResult> ReadWbsDocumentIdByUrl([FromBody] GetWbsDocumentIdByTaskId getWbsDocumentIdByTaskId,
        [FromHeader(Name = "CU")] string cu, [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            
            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider,
                IPbsRepository = _iPbsRepository,
                GetWbsDocumentIdByTaskId = getWbsDocumentIdByTaskId
            };
            var result = await _iWbsRepository.ReadWbsDocumentIdByUrl(wbsParam);
            return Ok(new ApiOkResponse(result, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }
    
    [HttpPost("GetContentTypes")]
    public async Task<ActionResult> GetContentTypes([FromBody] GetWbsDocumentIdByTaskId getWbsDocumentIdByTaskId,
        [FromHeader(Name = "CU")] string cu, [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            
            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider,
                IPbsRepository = _iPbsRepository,
                GetWbsDocumentIdByTaskId = getWbsDocumentIdByTaskId
            };
            var result = await _iWbsRepository.GetContentTypes(wbsParam);
            return Ok(new ApiOkResponse(result, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }
    
    [HttpPost("UpdateFile")]
    public async Task<ActionResult> UpdateFile([FromBody] UpdateFileDto updateFileDto,
        [FromHeader(Name = "CU")] string cu, [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            
            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider,
                IPbsRepository = _iPbsRepository,
                UpdateFileDto = updateFileDto
            };
            var result = await _iWbsRepository.UpdateFile(wbsParam);
            return Ok(new ApiOkResponse(result, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }
    
    [HttpPost("WbsTaskDateUpdate")]
    public async Task<ActionResult> WbsTaskDateUpdate(
        [FromBody] WbsTaskDateUpdateDto wbsTaskDateUpdateDto,
        [FromHeader(Name = "CU")] string cu, [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var wbsParam = new WbsParameter
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider,
                WbsTaskDateUpdateDto = wbsTaskDateUpdateDto,
                IPbsRepository = _iPbsRepository
            };
            var result = await _iWbsRepository.WbsTaskDateUpdate(wbsParam);
            return Ok(new ApiOkResponse(result, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
   
}