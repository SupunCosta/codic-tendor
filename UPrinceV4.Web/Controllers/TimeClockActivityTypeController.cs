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
public class TimeClockActivityTypeController : CommonConfigurationController
{
    private readonly ITimeClockActivityTypeRepository _iTimeClockActivityTypeRepository;

    public TimeClockActivityTypeController(ITimeClockActivityTypeRepository iTimeClockActivityTypeRepository,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)
    {
        _iTimeClockActivityTypeRepository = iTimeClockActivityTypeRepository;
    }

   
    [HttpGet("GetTimeClockActivityTypes")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetTimeClockActivityTypes()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            return Ok(new ApiOkResponse(
                await _iTimeClockActivityTypeRepository.GetTimeClockActivityTypes(UPrinceCustomerContext, lang)));
        }
        catch (Exception ex)
        {
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = false;
            ApiResponse.Message = ex.Message;
            return BadRequest(ApiResponse);
        }
    }

   
    [HttpGet("GetTimeClockActivityTypeById/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetTimeClockActivityTypeById(int id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var timeClockActivityType =
                await _iTimeClockActivityTypeRepository.GetTimeClockActivityTypeById(UPrinceCustomerContext, id,
                    lang);

            return Ok(new ApiOkResponse(timeClockActivityType));
        }
        catch (Exception ex)
        {
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = false;
            ApiResponse.Message = ex.Message;
            return BadRequest(ApiResponse);
        }
    }

    //[Microsoft.AspNetCore.Mvc.HttpGet("ReadByTypeId/{typeId}")]
    [HttpGet("ReadByTypeId")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetTimeClockActivityTypeByTypeId(int typeId)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var timeClockActivityType =
                await _iTimeClockActivityTypeRepository.GetTimeClockActivityTypeByTypeId(UPrinceCustomerContext,
                    typeId, lang);

            return Ok(new ApiOkResponse(timeClockActivityType));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    //[Microsoft.AspNetCore.Mvc.HttpDelete("Delete/{id}")]
    [HttpDelete("Delete/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult DeleteTimeClockActivityType(int id)
    {
        try
        {
            var type = (from a in UPrinceCustomerContext.TimeClockActivityType
                where a.Id == id
                select a).Single();
            var relatedLocalizedData = UPrinceCustomerContext.LocalizedData
                .Where(ld => ld.LocaleCode == type.LocaleCode).ToList();
            foreach (var ld in relatedLocalizedData)
            {
                UPrinceCustomerContext.LocalizedData.Remove(ld);
                UPrinceCustomerContext.SaveChanges();
            }

            UPrinceCustomerContext.TimeClockActivityType.Remove(type);
            UPrinceCustomerContext.SaveChanges();
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = false;
            ApiResponse.Message = "TimeClockActivityType deleted successfully";
            return Ok(ApiResponse);
        }
        catch (Exception)
        {
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = false;
            ApiResponse.Message = "TimeClockActivityType deleted successfully";
            return BadRequest(ApiResponse);
        }
    }
}