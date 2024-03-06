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

[Route("api/[controller]")]
[ApiController]
public class ProjectTemplateController : CommonConfigurationController
{
    private readonly IProjectTemplateRepository _projectTemplateRepository;

    public ProjectTemplateController(IProjectTemplateRepository projectTemplateRepository,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)
    {
        _projectTemplateRepository = projectTemplateRepository;
    }

    //[Authorize]
    //[HttpGet("Read")]
    [HttpGet("GetTemplate")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetTemplate()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var projectTemplates =
                _projectTemplateRepository.GetProjectTemplates(UPrinceCustomerContext, lang, null);
            ApiOkResponse.Result = projectTemplates;
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

   
    [HttpGet("Read/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetTemplateById(string id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            if (lang == Language.en.ToString() || string.IsNullOrEmpty(lang))
            {
                var projectTemplate = UPrinceCustomerContext.ProjectTemplate.Where(p => p.Id == id).ToList();
                ApiOkResponse.Result = projectTemplate;
                ApiOkResponse.Message = "Ok";
                return Ok(ApiOkResponse);
            }
            else
            {
                var projectTemplate = UPrinceCustomerContext.ProjectTemplate.FirstOrDefault(p => p.Id == id);
                //var localizedData = UPrinceCustomerContext.LocalizedData.FirstOrDefault(ld => ld.LocaleCode == projectTemplate.LocaleCode && ld.LanguageCode == lang);
                //projectTemplate.Name = localizedData.Label;
                ApiOkResponse.Result = projectTemplate;
                ApiOkResponse.Message = "Ok";
                return Ok(ApiOkResponse);
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    //[Authorize]
    //[HttpPost("Create")]
    [HttpPost("Create")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreateProjectTemplate([FromBody] ProjectTemplateCreateDto temp)
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
                var projectTemplate = new ProjectTemplate { Id = Guid.NewGuid().ToString(), Name = temp.Name };
                UPrinceCustomerContext.ProjectTemplate.Add(projectTemplate);
                UPrinceCustomerContext.SaveChanges();
                ApiOkResponse.Result = projectTemplate.Id;
                ApiOkResponse.Message = "Ok";
                return Ok(ApiOkResponse);
            }
            else
            {
                var projectTemplate =
                    UPrinceCustomerContext.ProjectTemplate.FirstOrDefault(p => p.Id == temp.Id);
                var localizedData = new LocalizedData
                {
                    LanguageCode = lang,
                    // LocaleCode = projectTemplate.LocaleCode,
                    Label = temp.Name
                };
                UPrinceCustomerContext.LocalizedData.Update(localizedData);
                UPrinceCustomerContext.SaveChanges();
                ApiOkResponse.Result = projectTemplate.Id;
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
    public async Task<ActionResult<int>> UpdateProjectTemplate([FromBody] ProjectTemplate temp)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            UPrinceCustomerContext.ProjectTemplate.Update(temp);
            UPrinceCustomerContext.SaveChanges();
            ApiOkResponse.Result = temp.Id;
            ApiOkResponse.Message = "Ok";
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    //[Authorize]
    //[HttpDelete("Delete/{id}")]
    [HttpDelete("Delete/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult DeleteProjectTemplate(string id)
    {
        try
        {
            var projectTemplate = (from a in UPrinceCustomerContext.ProjectTemplate
                where a.Id == id
                select a).Single();
            UPrinceCustomerContext.ProjectTemplate.Remove(projectTemplate);
            UPrinceCustomerContext.SaveChanges();
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = true;
            ApiResponse.Message = "Project Template deleted successfully";
            return Ok(ApiResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}