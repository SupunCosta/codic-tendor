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
public class ProjectToleranceStateController : CommonConfigurationController
{
    private readonly IProjectToleranceStateRepository _iProjectToleranceStateRepository;

    public ProjectToleranceStateController(IProjectToleranceStateRepository iProjectToleranceStateRepository,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)
    {
        _iProjectToleranceStateRepository = iProjectToleranceStateRepository;
    }

    //[HttpGet("Read")]
    [HttpGet("Read")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetToleranceState()
    {
        try
        {
            var api = ApiResponse;
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var projectToleranceStates =
                await _iProjectToleranceStateRepository.GetProjectToleranceStates(UPrinceCustomerContext, lang,
                    null);
            ApiOkResponse.Result = projectToleranceStates;
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

   
    [HttpGet("GetToleranceStateById/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetToleranceStateById(string id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            if (lang == Language.en.ToString() || string.IsNullOrEmpty(lang))
            {
                var projectToleranceState =
                    UPrinceCustomerContext.ProjectToleranceState.Where(p => p.Id == id).ToList();
                ApiResponse.StatusCode = 200;
                ApiResponse.Status = false;
                ApiResponse.Message = "No available Project Tolerance State for the Id ";
                ApiOkResponse.Result = projectToleranceState;
                ApiOkResponse.Message = "Ok";
                return Ok(!projectToleranceState.Any() ? ApiResponse : ApiOkResponse);
            }
            else
            {
                var projectToleranceState =
                    UPrinceCustomerContext.ProjectToleranceState.FirstOrDefault(p => p.Id == id);
                //var localizedData = UPrinceCustomerContext.LocalizedData.FirstOrDefault(ld => ld.LocaleCode == projectToleranceState.LocaleCode && ld.LanguageCode == lang);
                //projectToleranceState.Name = localizedData.Label;
                ApiOkResponse.Result = projectToleranceState;
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
    public async Task<ActionResult<int>> CreateToleranceState([FromBody] ProjectToleranceStateCreateDto state)
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
                    var defaultValue =
                        UPrinceCustomerContext.ProjectToleranceState.Where(p => p.IsDefault == true);
                    if (defaultValue.Count() != 0)
                    {
                        defaultValue.First().IsDefault = false;
                        UPrinceCustomerContext.ProjectToleranceState.Update(defaultValue.First());
                    }
                }

                var projectToleranceState = new ProjectToleranceState();
                projectToleranceState.Id = Guid.NewGuid().ToString();
                projectToleranceState.IsDefault = state.IsDefault;
                projectToleranceState.Name = state.Name;
                UPrinceCustomerContext.ProjectToleranceState.Add(projectToleranceState);
                UPrinceCustomerContext.SaveChanges();
                ApiOkResponse.Result = projectToleranceState.Id;
                ApiOkResponse.Message = "Ok";
                return Ok(ApiOkResponse);
            }
            else
            {
                var projectToleranceState =
                    UPrinceCustomerContext.ProjectToleranceState.FirstOrDefault(p => p.Id == state.Id);
                var localizedData = new LocalizedData
                {
                    LanguageCode = lang,
                    //LocaleCode = projectToleranceState.LocaleCode,
                    Label = state.Name
                };
                UPrinceCustomerContext.LocalizedData.Update(localizedData);
                UPrinceCustomerContext.SaveChanges();
                ApiOkResponse.Result = projectToleranceState.Id;
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
    public async Task<ActionResult<int>> UpdateToleranceState([FromBody] ProjectToleranceState state)
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
                var defaultValue = UPrinceCustomerContext.ProjectToleranceState.Where(p => p.IsDefault == true);
                if (defaultValue.Count() != 0)
                {
                    defaultValue.First().IsDefault = false;
                    UPrinceCustomerContext.ProjectToleranceState.Update(defaultValue.First());
                }
            }

            UPrinceCustomerContext.ProjectToleranceState.Update(state);
            UPrinceCustomerContext.SaveChanges();
            ApiOkResponse.Result = state.Id;
            ApiOkResponse.Message = "Ok";
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    //[HttpDelete("Delete/{id}")]
    [HttpDelete("Delete/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult DeleteToleranceState(string id)
    {
        try
        {
            var projectToleranceState = (from a in UPrinceCustomerContext.ProjectToleranceState
                where a.Id == id
                select a).Single();
            UPrinceCustomerContext.ProjectToleranceState.Remove(projectToleranceState);
            UPrinceCustomerContext.SaveChanges();
            ApiResponse.StatusCode = 400;
            ApiResponse.Status = false;
            ApiResponse.Message = "Project Tolerance State deleted successfully";
            return Ok(ApiResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}