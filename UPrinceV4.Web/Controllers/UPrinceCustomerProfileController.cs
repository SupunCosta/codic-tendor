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
public class UPrinceCustomerProfileController : CommonConfigurationController
{
    private readonly IUPrinceCustomerProfileRepository _iUPrinceCustomerProfileRepository;
    private readonly UPrinceCustomerContex _uPrinceCustomerContext;

    public UPrinceCustomerProfileController(IUPrinceCustomerProfileRepository iUPrinceCustomerProfileRepository,
        UPrinceCustomerContex uPrinceCustomerContext, ApplicationDbContext _context,
        IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider)
        : base(_context, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse, iTenantProvider)
    {
        _uPrinceCustomerContext = uPrinceCustomerContext;
        _iUPrinceCustomerProfileRepository = iUPrinceCustomerProfileRepository;
    }


    [HttpGet("GetUPrinceCustomerProfiles")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult GetUPrinceCustomerProfiles()
    {
        return Ok(new ApiOkResponse(
            _iUPrinceCustomerProfileRepository.GetUPrinceCustomerProfiles(_uPrinceCustomerContext)));
    }


  
    [HttpGet("GetUPrinceCustomerProfileById/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetUPrinceCustomerProfileById(int id)
    {
        try
        {
            return Ok(new ApiOkResponse(
                _iUPrinceCustomerProfileRepository.GetUPrinceCustomerProfileById(_uPrinceCustomerContext, id)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    [HttpGet("Page/{pageNo}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetUPrinceCustomerProfilePage(int pageNo)
    {
        try
        {
            return Ok(new ApiOkResponse(
                _iUPrinceCustomerProfileRepository.GetUPrinceCustomerProfilePagedResult<UPrinceCustomerProfile>(
                    _uPrinceCustomerContext, pageNo)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    // POST: api/UprinceCustomerProfile
    //[HttpPost("Create")]
    [HttpPost("Create")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreateUPrinceCustomerProfile(
        [FromBody] UPrinceCustomerProfileDto uprinceCustomerProfileDto)
    {
        var uprinceCustomerProfile = new UPrinceCustomerProfile
        {
            UprinceCustomerId = uprinceCustomerProfileDto.UprinceCustomerId,
            VerificationStatus = uprinceCustomerProfileDto.VerificationStatus,
            CompanyName = uprinceCustomerProfileDto.CompanyName
        };
        try
        {
            return Ok(new ApiOkResponse(
                _iUPrinceCustomerProfileRepository.AddUPrinceCustomerProfile(_uPrinceCustomerContext,
                    uprinceCustomerProfile)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    [HttpDelete("Delete/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteUPrinceCustomerProfileById(int id)
    {
        try
        {
            _iUPrinceCustomerProfileRepository.DeleteUPrinceCustomerProfile(_uPrinceCustomerContext, id);
            ApiResponse.StatusCode = 204;
            ApiResponse.Status = true;
            ApiResponse.Message = "Customer Profile deleted successfully";
            return Ok(ApiResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}