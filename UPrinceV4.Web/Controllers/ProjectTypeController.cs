using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.UserException;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Controllers;


public class ProjectTypeController : CommonConfigurationController
{
    private readonly IProjectTypeRepository _projectTypeRepository;

    public ProjectTypeController(IProjectTypeRepository projectTypeRepository,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)
    {
        _projectTypeRepository = projectTypeRepository;
    }

    //[HttpGet("Read")]
    [HttpGet("GetProjectType")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetProjectType()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var projectTypes = await _projectTypeRepository.GetProjectTypes(UPrinceCustomerContext, lang, null);
            ApiOkResponse.Result = projectTypes;
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
    
    [HttpGet("GetProjectTypeById/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetProjectTypeById(string id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            if (lang == Language.en.ToString() || string.IsNullOrEmpty(lang))
            {
                var projectType = UPrinceCustomerContext.ProjectType.Where(p => p.Id == id).ToList();
                ApiOkResponse.Result = projectType;
                ApiOkResponse.Message = "Ok";
                return Ok(ApiOkResponse);
            }
            else
            {
                var projectType = UPrinceCustomerContext.ProjectType.FirstOrDefault(p => p.Id == id);
                // var localizedData = UPrinceCustomerContext.LocalizedData.FirstOrDefault(ld =>
                //  ld.LocaleCode == projectType.LocaleCode && ld.LanguageCode == lang);
                // projectType.Name = localizedData.Label;
                ApiOkResponse.Result = projectType;
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
    public async Task<ActionResult<int>> CreateProjectType([FromBody] ProjectTypeCreateDto type)
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
                if (type.IsDefault)
                {
                    var defaultValue = UPrinceCustomerContext.ProjectType.Where(p => p.IsDefault == true);
                    if (defaultValue.Count() != 0)
                    {
                        defaultValue.First().IsDefault = false;
                        UPrinceCustomerContext.ProjectType.Update(defaultValue.First());
                    }
                }

                var projectType = new ProjectType
                {
                    Id = Guid.NewGuid().ToString(),
                    IsDefault = type.IsDefault,
                    Name = type.Name
                    //LocaleCode = "ProjectType" + type.Name
                };
                UPrinceCustomerContext.ProjectType.Add(projectType);
                UPrinceCustomerContext.SaveChanges();
                ApiOkResponse.Result = projectType.Id;
                ApiOkResponse.Message = "Ok";
                return Ok(ApiOkResponse);
            }
            else
            {
                var projectType = UPrinceCustomerContext.ProjectType.FirstOrDefault(p => p.Id == type.Id);
                var localizedData = new LocalizedData
                {
                    LanguageCode = lang,
                    //LocaleCode = projectType.LocaleCode,
                    Label = type.Name
                };
                UPrinceCustomerContext.LocalizedData.Update(localizedData);
                UPrinceCustomerContext.SaveChanges();
                ApiOkResponse.Result = projectType.Id;
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
    public async Task<ActionResult<int>> UpdateProjectType([FromBody] ProjectType type)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(new ApiBadRequestResponse(ModelState));

            if (type.IsDefault)
            {
                var defaultValue = UPrinceCustomerContext.ProjectType.Where(p => p.IsDefault == true);
                if (defaultValue.Count() != 0)
                {
                    defaultValue.First().IsDefault = false;
                    UPrinceCustomerContext.ProjectType.Update(defaultValue.First());
                }
            }

            UPrinceCustomerContext.ProjectType.Update(type);
            UPrinceCustomerContext.SaveChanges();
            ApiOkResponse.Result = type.Id;
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
    public IActionResult DeleteProjectType(string id)
    {
        try
        {
            var projectType = (from a in UPrinceCustomerContext.ProjectType
                where a.Id == id
                select a).Single();
            UPrinceCustomerContext.ProjectType.Remove(projectType);
            UPrinceCustomerContext.SaveChanges();
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = true;
            ApiResponse.Message = "Project Type deleted successfully";
            return Ok(ApiResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}