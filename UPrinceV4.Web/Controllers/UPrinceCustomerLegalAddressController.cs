using System;
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
public class UprinceCustomerLegalAddressController : CommonConfigurationController
{
    private readonly IUPrinceCustomerLegalAddressRepository _iUPrinceCustomerLegalAddressRepository;
    private readonly UPrinceCustomerContex _uPrinceCustomerContext;

    public UprinceCustomerLegalAddressController(
        IUPrinceCustomerLegalAddressRepository iUPrinceCustomerLegalAddressRepository,
        UPrinceCustomerContex uPrinceCustomerContext, ApplicationDbContext _context,
        IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider)
        : base(_context, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse, iTenantProvider)
    {
        _uPrinceCustomerContext = uPrinceCustomerContext;
        _iUPrinceCustomerLegalAddressRepository = iUPrinceCustomerLegalAddressRepository;
    }


    [HttpGet("GetUPrinceCustomerLegalAddresses")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetUPrinceCustomerLegalAddresses()
    {
        var addresses =
            await _iUPrinceCustomerLegalAddressRepository.GetUPrinceCustomerLegalAddress(_uPrinceCustomerContext);
        ApiOkResponse.Result = addresses;
        return Ok(ApiOkResponse);
    }

    // GET: api/UprinceCustomerLegalAddress/5
    //[HttpGet("Read/{id}")]
    [HttpGet("GetUPrinceCustomerLegalAddressById/{id}")]
    
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetUPrinceCustomerLegalAddressById(int id)
    {
        try
        {
            var uprinceCustomerLegalAddress =
                await _iUPrinceCustomerLegalAddressRepository.GetUPrinceCustomerLegalAddressById(
                    _uPrinceCustomerContext, id);
            ApiOkResponse.Result = uprinceCustomerLegalAddress;
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    // POST: api/UprinceCustomerLegalAddress
    //[HttpPost("Create")]
    [HttpPost("Create")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreateUPrinceCustomerLegalAddress(
        [FromBody] UPrinceCustomerLegalAddressDto uprinceCustomerLegalAddressDto)
    {
        var uprinceCustomerLegalAddress = new UPrinceCustomerLegalAddress
        {
            Street = uprinceCustomerLegalAddressDto.Street,
            City = uprinceCustomerLegalAddressDto.City,
            Country = uprinceCustomerLegalAddressDto.Country,
            PostalCode = uprinceCustomerLegalAddressDto.PostalCode
        };
        try
        {
            var id = await _iUPrinceCustomerLegalAddressRepository.AddUPrinceCustomerLegalAddress(
                _uPrinceCustomerContext, uprinceCustomerLegalAddress);
            ApiOkResponse.Result = id;
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    // DELETE: api/ApiWithActions/5
    //[HttpDelete("Delete/{id}")]
    [HttpDelete("Delete/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult DeleteUPrinceCustomerLegalAddressById(int id)
    {
        try
        {
            _iUPrinceCustomerLegalAddressRepository.DeleteUPrinceCustomerLegalAddress(_uPrinceCustomerContext, id);
            return Ok(new ApiResponse(204, true, "LegalAddress deleted successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    // [HttpGet("ReadHistory")]
    [HttpGet("ReadHistory")]
    
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetUPrinceCustomerLegalAddressHistory()
    {
        var customers =
            await _iUPrinceCustomerLegalAddressRepository.GetUPrinceCustomerLegalAddressHistory(
                _uPrinceCustomerContext);
        ApiOkResponse.Result = customers;
        return Ok(ApiOkResponse);
    }
}