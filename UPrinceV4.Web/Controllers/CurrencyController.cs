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
public class CurrencyController : CommonConfigurationController
{
    private readonly ICurrencyRepository _currencyRepository;

    public CurrencyController(ICurrencyRepository currencyRepository,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)
    {
        _currencyRepository = currencyRepository;
    }


    //[Microsoft.AspNetCore.Mvc.HttpGet("Read")]
    [HttpGet("Read")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetCurrency()
    {
        try
        {
            var currencies = await _currencyRepository.GetCurrencies(UPrinceCustomerContext);
            ApiOkResponse.Result = currencies;
            ApiOkResponse.Message = "Ok";
            ApiResponse.Message = "No available calendar templates";
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = false;
            return Ok(!currencies.Any() ? ApiResponse : ApiOkResponse);
        }
        catch (EmptyListException ex)
        {
            return Ok(new ApiResponse(200, false, ex.Message));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    
    [HttpGet("ReadById/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetCurrencyById(int id)
    {
        try
        {
            var currency = await _currencyRepository.GetCurrencyById(UPrinceCustomerContext, id);

            return Ok(currency == null
                ? new ApiResponse(200, false, "No available calendar templates")
                : new ApiOkResponse(currency));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    [HttpPost("Create")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreateCurrency([FromBody] CurrencyCreateDto cur)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var currencyId = await _currencyRepository.CreateCurrency(UPrinceCustomerContext, cur);
            var isCurrencyExist = UPrinceCustomerContext.Currency.Any
                (x => x.Name == cur.Name);
            if (isCurrencyExist)
            {
                ModelState.AddModelError("Currency", "Currency already exists");
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            return Ok(new ApiOkResponse(currencyId));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPut("Update")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> UpdateCurrency([FromBody] Currency cur)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            return Ok(new ApiOkResponse(await _currencyRepository.UpdateCurrency(UPrinceCustomerContext, cur)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpDelete("Delete/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteCurrency(int id)
    {
        try
        {
            await _currencyRepository.DeleteCurrency(UPrinceCustomerContext, id);
            return Ok(new ApiResponse(200, true, "Currency deleted successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}