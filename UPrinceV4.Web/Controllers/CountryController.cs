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

namespace UPrinceV4.Web.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CountryController : CommonConfigurationController
{
    private readonly CountryRepositoryParameter _countryRepositoryParameter;
    private readonly ICountryRepository _iCountryRepository;

    public CountryController(ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor,
        ApiResponse apiResponse, ApiBadRequestResponse apiBadRequestResponse,
        ApiOkResponse apiOkResponse, ICountryRepository iCountryRepository,
        CountryRepositoryParameter countryRepositoryParameter, ITenantProvider iTenantProvider)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)
    {
        _iCountryRepository = iCountryRepository;
        _countryRepositoryParameter = countryRepositoryParameter;
    }

    //[Microsoft.AspNetCore.Mvc.HttpGet("ReadCountryList")]
    [HttpGet("ReadCountryList")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetCountries()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            _countryRepositoryParameter.Lang = lang;
            _countryRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            _countryRepositoryParameter.TenantProvider = ItenantProvider;
            var countryList = await _iCountryRepository.GetCountryList(_countryRepositoryParameter);
            // ApiOkResponse.Result = countryList;
            // ApiOkResponse.Message = "Ok";
            // ApiResponse.Message = "No available calendar templates";
            // ApiResponse.StatusCode = 200;
            // ApiResponse.Status = false;
            return Ok(!countryList.Any() ? new ApiResponse(200, false,"No available calendar templates") : new ApiOkResponse(countryList));
        }
        catch (EmptyListException ex)
        {
            return Ok(new ApiResponse(200, false, ex.Message));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}