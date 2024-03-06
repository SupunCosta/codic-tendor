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
using UPrinceV4.Web.UserException;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Controllers;


[Authorize]
[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class ProjectStateController : CommonConfigurationController
{
    private readonly IProjectStateRepository _projectStateRepository;

    public ProjectStateController(IProjectStateRepository projectStateRepository,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)
    {
        _projectStateRepository = projectStateRepository;
    }


    //[HttpGet("Read")]
    [HttpGet("Read")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetProjectState()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var projectStatus =
                await _projectStateRepository.GetProjectStates(UPrinceCustomerContext, lang, ItenantProvider);
            ApiOkResponse.Result = projectStatus;
            ApiOkResponse.Message = "Ok";
            return Ok(ApiOkResponse);
        }
        catch (EmptyListException ex)
        {
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = false;
            ApiResponse.Message = ex.Message;
            return Ok(ApiResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    //[HttpGet("Read/{id}")]
    [HttpGet("ReadById")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetProjectStateId(string id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            if (lang == Language.en.ToString() || string.IsNullOrEmpty(lang))
            {
                var projectState = UPrinceCustomerContext.ProjectState.Where(p => p.Id == id).ToList();
                ApiOkResponse.Result = projectState;
                ApiOkResponse.Message = "Ok";
                return Ok(ApiOkResponse);
            }
            else
            {
                var projectState = UPrinceCustomerContext.ProjectState.FirstOrDefault(p => p.Id == id);
                var localizedData = UPrinceCustomerContext.LocalizedData.FirstOrDefault(ld =>
                    ld.LocaleCode == projectState.LocaleCode && ld.LanguageCode == lang);
                projectState.Name = localizedData.Label;
                ApiOkResponse.Result = ApiOkResponse;
                ApiOkResponse.Message = "Ok";
                return Ok(ApiOkResponse);
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    //[HttpPost("Create")]
    [HttpPost("Create")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreateProjectState([FromBody] ProjectStateCreateDto state)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            if (lang == Language.en.ToString() || string.IsNullOrEmpty(lang))
            {
                if (state.IsDefault)
                {
                    var defaultValue = UPrinceCustomerContext.ProjectState.Where(p => p.IsDefault == true);
                    if (defaultValue.Count() != 0)
                    {
                        defaultValue.First().IsDefault = false;
                        UPrinceCustomerContext.ProjectState.Update(defaultValue.First());
                    }
                }

                var projectState = new ProjectState
                {
                    Id = Guid.NewGuid().ToString(),
                    IsDefault = state.IsDefault,
                    Name = state.Name,
                    LocaleCode = "ProjectState" + state.Name
                };
                UPrinceCustomerContext.ProjectState.Add(projectState);
                UPrinceCustomerContext.SaveChanges();
                ApiOkResponse.Result = projectState.Id;
                ApiOkResponse.Message = "Ok";
                return Ok(ApiOkResponse);
            }
            else
            {
                var projectState = UPrinceCustomerContext.ProjectState.FirstOrDefault(p => p.Id == state.Id);
                var localizedData = new LocalizedData
                {
                    LanguageCode = lang,
                    LocaleCode = projectState.LocaleCode,
                    Label = state.Name
                };
                UPrinceCustomerContext.LocalizedData.Update(localizedData);
                UPrinceCustomerContext.SaveChanges();
                ApiOkResponse.Result = projectState.Id;
                ApiOkResponse.Message = "Ok";
                return Ok(ApiOkResponse);
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    //[HttpPut("Update")]
    [HttpPut("Update")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> UpdateProjectState([FromBody] ProjectState state)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            if (state.IsDefault)
            {
                var defaultValue = UPrinceCustomerContext.ProjectState.Where(p => p.IsDefault == true);
                if (defaultValue.Count() != 0)
                {
                    defaultValue.First().IsDefault = false;
                    UPrinceCustomerContext.ProjectState.Update(defaultValue.First());
                }
            }

            UPrinceCustomerContext.ProjectState.Update(state);
            UPrinceCustomerContext.SaveChanges();
            ApiOkResponse.Result = state.Id;
            ApiOkResponse.Message = "Ok";
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpDelete("Delete")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteProjectState(string id)
    {
        try
        {
            var state = (from a in UPrinceCustomerContext.ProjectState
                where a.Id == id
                select a).Single();
            UPrinceCustomerContext.ProjectState.Remove(state);
            UPrinceCustomerContext.SaveChanges();
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = true;
            ApiResponse.Message = "Project State deleted successfully";
            return Ok(ApiResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}