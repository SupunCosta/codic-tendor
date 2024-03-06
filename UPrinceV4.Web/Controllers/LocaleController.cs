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
public class LocaleController : CommonConfigurationController
{
    private readonly ILocaleRepository _localeRepository;
    private readonly ITenantProvider _tenantProvider;

    public LocaleController(ITenantProvider tenantProvider, ILocaleRepository localeRepository,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)
    {
        _tenantProvider = tenantProvider;
        _localeRepository = localeRepository;
    }

    //[HttpGet("Read")]
    [HttpGet("Read")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Get()
    {
        try
        {
            var locales = UPrinceCustomerContext.Locales.ToList();
            if (!locales.Any())
            {
                ApiResponse.Message = "No Locales available";
                ApiResponse.Status = false;
                ApiResponse.StatusCode = 200;
                return Ok(ApiResponse);
            }

            ApiOkResponse.Result = locales;
            ApiOkResponse.Message = "Ok";
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    //[HttpPost("Create")]
    //content type = x-www-urlencoded-form
    [HttpPost("Create")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> Create([FromForm] IFormCollection locale)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var id = await _localeRepository.CreateLocale(UPrinceCustomerContext, _tenantProvider, locale);
            ApiOkResponse.Result = id;
            ApiOkResponse.Message = "Ok";
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    //[HttpPut("Update")]
    [HttpPut("Update")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> Update([FromBody] LocaleNameUpdateDto locale)
    {
        try
        {
            var isLocaleExist = UPrinceCustomerContext.Locales.Any
                (x => x.Locale == locale.Locale);
            if (isLocaleExist)
            {
                ModelState.AddModelError("Locale", "Locale already exists");
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var id = await _localeRepository.UpdateLocale(UPrinceCustomerContext, _tenantProvider,
                locale);
            ApiOkResponse.Result = id;
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
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var locale = (from a in UPrinceCustomerContext.Locales
                where a.Id == id
                select a).Single();
            UPrinceCustomerContext.Locales.Remove(locale);
            UPrinceCustomerContext.SaveChanges();
            ApiResponse.StatusCode = 200;
            ApiResponse.Message = "Locale deleted successfully";
            ApiResponse.Status = true;
            return Ok(ApiResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}