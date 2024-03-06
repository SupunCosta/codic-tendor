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
public class UPrinceCustomerPrimaryContactController : CommonConfigurationController
{
    private readonly IUPrinceCustomerPrimaryContactRepository _iUPrinceCustomerPrimaryContactRepository;
    private readonly UPrinceCustomerContex _uPrinceCustomerContext;

    public UPrinceCustomerPrimaryContactController(
        IUPrinceCustomerPrimaryContactRepository iUPrinceCustomerPrimaryContactRepository,
        UPrinceCustomerContex uPrinceCustomerContext, ApplicationDbContext _context,
        IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider)
        : base(_context, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse, iTenantProvider)
    {
        _uPrinceCustomerContext = uPrinceCustomerContext;
        _iUPrinceCustomerPrimaryContactRepository = iUPrinceCustomerPrimaryContactRepository;
    }


    [HttpGet("GetUPrinceCustomerPrimaryContacts")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetUPrinceCustomerPrimaryContacts()
    {
        var priContact =
            await _iUPrinceCustomerPrimaryContactRepository.GetUPrinceCustomerPrimaryContacts(
                _uPrinceCustomerContext);
        ApiOkResponse.Result = priContact;
        return Ok(ApiOkResponse);
    }

    // GET: api/UprinceCustomerPrimaryContact/5
    //[HttpGet("Read/{id}")]
    [HttpGet("GetUPrinceCustomerPrimaryContactById/{id}")]
    
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetUPrinceCustomerPrimaryContactById(int id)
    {
        try
        {
            return Ok(new ApiOkResponse(
                await _iUPrinceCustomerPrimaryContactRepository.GetUPrinceCustomerPrimaryContactById(
                    _uPrinceCustomerContext, id)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    [HttpPost("Create")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreateUPrinceCustomerPrimaryContact(
        [FromBody] UPrinceCustomerPrimaryContactDto uprinceCustomerPrimaryContactDto)
    {
        var uprinceCustomerPrimaryContact = new UPrinceCustomerPrimaryContact
        {
            UprinceCustomerProfileId = uprinceCustomerPrimaryContactDto.UprinceCustomerProfileId,
            Name = uprinceCustomerPrimaryContactDto.Name,
            phone = uprinceCustomerPrimaryContactDto.phone,
            Email = uprinceCustomerPrimaryContactDto.Email
        };
        try
        {
            var id = await _iUPrinceCustomerPrimaryContactRepository.AddUPrinceCustomerPrimaryContact(
                _uPrinceCustomerContext, uprinceCustomerPrimaryContact);
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
    public IActionResult DeleteUPrinceCustomerPrimaryContactById(int id)
    {
        try
        {
            _iUPrinceCustomerPrimaryContactRepository.DeleteUPrinceCustomerPrimaryContact(_uPrinceCustomerContext,
                id);
            ApiResponse.StatusCode = 204;
            ApiResponse.Status = true;
            ApiResponse.Message = "Primary Contact deleted successfully";
            return Ok(ApiResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}