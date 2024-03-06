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
public class UPrinceCustomerLocationController : CommonConfigurationController
{
    private readonly IUPrinceCustomerLocationRepository _iUPrinceCustomerLocationRepository;
    private readonly UPrinceCustomerContex _uPrinceCustomerContext;

    public UPrinceCustomerLocationController(IUPrinceCustomerLocationRepository iUPrinceCustomerLocationRepository,
        UPrinceCustomerContex uPrinceCustomerContext, ApplicationDbContext _context,
        IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider)
        : base(_context, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse, iTenantProvider)
    {
        _uPrinceCustomerContext = uPrinceCustomerContext;
        _iUPrinceCustomerLocationRepository = iUPrinceCustomerLocationRepository;
    }

    // GET: api/UprinceCustomerLocation
    //[HttpGet("Read")]
    [HttpGet("GetUPrinceCustomerLocations")]
    
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetUPrinceCustomerLocations()
    {
        var locations =
            await _iUPrinceCustomerLocationRepository.GetUPrinceCustomerLocations(_uPrinceCustomerContext);
        ApiOkResponse.Result = locations;
        return Ok(ApiOkResponse);
    }

    [HttpGet("GetUPrinceCustomerLocationById/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetUPrinceCustomerLocationById(int id)
    {
        try
        {
            var location =
                await _iUPrinceCustomerLocationRepository.GetUPrinceCustomerLocationById(_uPrinceCustomerContext,
                    id);
            ApiOkResponse.Result = location;
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    // POST: api/UprinceCustomerLocation
    //[HttpPost("Create")]
    [HttpPost("Create")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreateUPrinceCustomerLocation(
        [FromBody] UPrinceCustomerLocationDto uprinceCustomerLocationDto)
    {
        var uprinceCustomerLocation = new UPrinceCustomerLocation
        {
            Street = uprinceCustomerLocationDto.Street,
            City = uprinceCustomerLocationDto.City,
            Country = uprinceCustomerLocationDto.Country,
            PostalCode = uprinceCustomerLocationDto.PostalCode,
            UprinceCustomerId = uprinceCustomerLocationDto.UprinceCustomerId
        };
        try
        {
            var id = await _iUPrinceCustomerLocationRepository.AddUPrinceCustomerCustomerLocation(
                _uPrinceCustomerContext, uprinceCustomerLocation);
            ApiOkResponse.Result = uprinceCustomerLocation.ID;
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
    public IActionResult DeleteUPrinceCustomerLocationById(int id)
    {
        try
        {
            _iUPrinceCustomerLocationRepository.DeleteUPrinceCustomerLocation(_uPrinceCustomerContext, id);
            ApiResponse.StatusCode = 204;
            ApiResponse.Status = true;
            ApiResponse.Message = "CustomerLocation deleted successfully";
            return Ok(ApiResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    //[HttpGet("ReadHistory")]
    [HttpGet("ReadHistory")]
    
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetUPrinceCustomerLegalAddressHistory()
    {
        var customers =
            await _iUPrinceCustomerLocationRepository
                .GetUPrinceCustomerLegalAddressHistory(_uPrinceCustomerContext);
        ApiOkResponse.Result = customers;
        return Ok(ApiOkResponse);
    }
}