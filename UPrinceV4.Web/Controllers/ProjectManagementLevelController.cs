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
public class ProjectManagementLevelController : CommonConfigurationController
{
    private readonly IProjectManagementLevelRepository _projectManagementLevelRepository;

    public ProjectManagementLevelController(IProjectManagementLevelRepository projectManagementLevelRepository,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)
    {
        _projectManagementLevelRepository = projectManagementLevelRepository;
    }


    //[HttpGet("Read")]
    [HttpGet("Read")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetManagementLevel()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var projectManagementLevels =
                await _projectManagementLevelRepository.GetProjectManagementLevels(UPrinceCustomerContext, lang,
                    null);
            ApiOkResponse.Result = projectManagementLevels;
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

  
    [HttpGet("GetManagementLevelById/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetManagementLevelById(string id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            if (lang == Language.en.ToString() || string.IsNullOrEmpty(lang))
            {
                var managementLevel = UPrinceCustomerContext.ProjectManagementLevel.Where(p => p.Id == id).ToList();
                ApiOkResponse.Result = managementLevel;
                ApiOkResponse.Message = "Ok";
                return Ok(ApiOkResponse);
            }

            var projectManagementLevel =
                UPrinceCustomerContext.ProjectManagementLevel.FirstOrDefault(p =>
                    p.Id == id && p.LanguageCode == lang);
            ApiOkResponse.Result = projectManagementLevel;
            ApiOkResponse.Message = "Ok";
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    // This methods has been changed to sql retrieve
    //[HttpPost("Create")]
    [HttpPost("Create")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreateManagementLevel([FromBody] ProjectManagementLevelCreateDto level)
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
                if (level.IsDefault)
                {
                    var defaultValue =
                        UPrinceCustomerContext.ProjectManagementLevel.Where(p => p.IsDefault == true);
                    if (defaultValue.Count() != 0)
                    {
                        defaultValue.First().IsDefault = false;
                        UPrinceCustomerContext.ProjectManagementLevel.Update(defaultValue.First());
                    }
                }

                var projectManagementLevel = new ProjectManagementLevel
                {
                    Id = Guid.NewGuid().ToString(),
                    IsDefault = level.IsDefault,
                    Name = level.Name
                    // LocaleCode = "ProjectManagementLevel" + level.Name
                };
                UPrinceCustomerContext.ProjectManagementLevel.Add(projectManagementLevel);
                UPrinceCustomerContext.SaveChanges();
                ApiOkResponse.Result = projectManagementLevel.Id;
                ApiOkResponse.Message = "Ok";
                return Ok(ApiOkResponse);
            }
            else
            {
                var projectManagementLevel =
                    UPrinceCustomerContext.ProjectManagementLevel.FirstOrDefault(p => p.Id == level.Id);
                var localizedData = new LocalizedData
                {
                    LanguageCode = lang,
                    //LocaleCode = projectManagementLevel.LocaleCode,
                    Label = projectManagementLevel.Name
                };
                UPrinceCustomerContext.LocalizedData.Update(localizedData);
                UPrinceCustomerContext.SaveChanges();
                ApiOkResponse.Result = projectManagementLevel.Id;
                ApiOkResponse.Message = "Ok";
                return Ok(ApiOkResponse);
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

  
    [HttpPut("Update")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> UpdateManagementLevel([FromBody] ProjectManagementLevel level)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            if (level.IsDefault)
            {
                var defaultValue =
                    UPrinceCustomerContext.ProjectManagementLevel.Where(p => p.IsDefault == true);
                if (defaultValue.Count() != 0)
                {
                    defaultValue.First().IsDefault = false;
                    UPrinceCustomerContext.ProjectManagementLevel.Update(defaultValue.First());
                }
            }

            var projectManagementLevel = new ProjectManagementLevel
            {
                Id = level.Id,
                IsDefault = level.IsDefault,
                Name = level.Name
            };
            UPrinceCustomerContext.ProjectManagementLevel.Update(projectManagementLevel);
            UPrinceCustomerContext.SaveChanges();
            ApiOkResponse.Result = projectManagementLevel.Id;
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
    public async Task<IActionResult> DeleteManagementLevel(string id)
    {
        try
        {
            var level = (from a in UPrinceCustomerContext.ProjectManagementLevel
                where a.Id == id
                select a).Single();
            UPrinceCustomerContext.ProjectManagementLevel.Remove(level);
            UPrinceCustomerContext.SaveChanges();
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = false;
            ApiResponse.Message = "Project Management Level deleted successfully";
            return Ok(ApiResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}