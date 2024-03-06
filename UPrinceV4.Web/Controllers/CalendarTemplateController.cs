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

namespace UPrinceV4.Web.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CalendarTemplateController : CommonConfigurationController
{
    private readonly ICalendarTemplateRepository _iCalendarTemplateRepository;

    public CalendarTemplateController(ICalendarTemplateRepository iCalendarTemplateRepository,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)

    {
        _iCalendarTemplateRepository = iCalendarTemplateRepository;
    }
    
    [HttpGet("Read")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetCalendarTemplate()
    {
        try
        {
            var calenderTemplates = await _iCalendarTemplateRepository.GetCalendarTemplates(UPrinceCustomerContext);
            ApiOkResponse.Result = calenderTemplates;
            ApiOkResponse.Message = "Ok";
            ApiResponse.Message = "No available calendar templates";
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = false;
            return Ok(!calenderTemplates.Any() ? ApiResponse : ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpGet("ReadById/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetCalendarTemplateById(string id)
    {
        try
        {
            var calenderTemplate =
                await _iCalendarTemplateRepository.GetCalendarTemplateById(UPrinceCustomerContext, id);
            return Ok(calenderTemplate == null
                ? new ApiResponse(200, false, "No Calendar Templates available for the Id " + id)
                : new ApiOkResponse(calenderTemplate, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    //[Microsoft.AspNetCore.Mvc.HttpPost("Create")]
    [HttpPost("Create")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreateCalendarTemplate(
        [FromBody] CalendarTemplateCreateDto cal)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            return Ok(new ApiOkResponse(
                await _iCalendarTemplateRepository.CreateCalendarTemplate(UPrinceCustomerContext, cal), "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    //[Microsoft.AspNetCore.Mvc.HttpPut("Update")]
    [HttpPut("Update")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> UpdateCalendarTemplate(
        [FromBody] CalendarTemplate cal)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiBadRequestResponse(ModelState));

            return Ok(new ApiOkResponse(
                await _iCalendarTemplateRepository.UpdateCalendarTemplate(UPrinceCustomerContext, cal)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpDelete("Delete/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteCalendarTemplate(string id)
    {
        try
        {
            return Ok(new ApiResponse(200,
                await _iCalendarTemplateRepository.DeleteCalendarTemplate(UPrinceCustomerContext, id),
                "Calendar Template deleted successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}