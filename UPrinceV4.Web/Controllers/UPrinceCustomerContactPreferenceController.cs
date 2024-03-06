using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Controllers;


[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UPrinceCustomerContactPreferenceController : CommonConfigurationController
{
    private readonly IUPrinceCustomerContactPreferenceRepository _uprinceCustomerContactPreferenceRepository;
    private readonly UPrinceCustomerContex _uPrinceCustomerContext;

    public UPrinceCustomerContactPreferenceController(
        IUPrinceCustomerContactPreferenceRepository uprinceCustomerContactPreferenceRepository,
        UPrinceCustomerContex uPrinceCustomerContext
        , ApplicationDbContext _context, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider)
        : base(_context, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse, iTenantProvider)
    {
        _uPrinceCustomerContext = uPrinceCustomerContext;
        _uprinceCustomerContactPreferenceRepository = uprinceCustomerContactPreferenceRepository;
    }
    
    [HttpPut("GetUprinceCustomerContactPreferences")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetUprinceCustomerContactPreferences()
    {
        var contactPreferences =
            await _uprinceCustomerContactPreferenceRepository.GetUPrinceCustomerContactPreferences(
                _uPrinceCustomerContext);
        ApiOkResponse.Result = contactPreferences;
        return Ok(ApiOkResponse);
    }

    
    [HttpGet("GetUprinceCustomerContactPreferenceById/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetUprinceCustomerContactPreferenceById(int id)
    {
        try
        {
            var uprinceCustomerContactPreference =
                await _uprinceCustomerContactPreferenceRepository.GetUPrinceCustomerContactPreferenceById(
                    _uPrinceCustomerContext, id);
            ApiOkResponse.Result = uprinceCustomerContactPreference;
            return Ok(ApiOkResponse);
        }
        catch (Exception)
        {
            ApiResponse.StatusCode = 400;
            ApiResponse.Status = false;
            ApiResponse.Message = ApiErrorMessages.CAN_NOT_FIND_MESSAGE;
            return BadRequest(ApiResponse);
        }
    }

    // POST: api/UprinceCustomerContactPreference
    //[Microsoft.AspNetCore.Mvc.HttpPost("Create")]
    [HttpPost("Create")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreateUprinceCustomerContactPreference(
        [FromBody] UPrinceCustomerContactPreferenceDto uprinceCustomerPreferenceDto)
    {
        try
        {
            var uprinceCustomerPreference = new UPrinceCustomerContactPreference
            {
                UprinceCustomerId = uprinceCustomerPreferenceDto.UprinceCustomerId,
                UPrinceCustomerJobRoleId = uprinceCustomerPreferenceDto.UPrinceCustomerJobRoleId,
                FirstName = uprinceCustomerPreferenceDto.FirstName,
                LastName = uprinceCustomerPreferenceDto.LastName,
                Email = uprinceCustomerPreferenceDto.Email
            };
            var state = await _uprinceCustomerContactPreferenceRepository.AddUPrinceCustomerContactPreference(
                _uPrinceCustomerContext, uprinceCustomerPreference);
            ApiOkResponse.Result = uprinceCustomerPreference.ID;
            return Ok(ApiOkResponse);
        }
        catch (Exception)
        {
            ApiResponse.StatusCode = 400;
            ApiResponse.Status = false;
            ApiResponse.Message = ApiErrorMessages.ALREADY_EXISTS_MESSAGE;
            return BadRequest(ApiResponse);
        }
    }

   
    [HttpDelete("Delete")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult DeleteUprinceCustomerContactPreferenceById(int id)
    {
        try
        {
            _uprinceCustomerContactPreferenceRepository.DeleteUPrinceCustomerContactPreference(
                _uPrinceCustomerContext, id);
            ApiResponse.StatusCode = 204;
            ApiResponse.Status = true;
            ApiResponse.Message = ApiErrorMessages.DELETED_SUCCESSFULLY_MESSAGE;
            return Ok(ApiResponse);
        }
        catch (Exception)
        {
            ApiResponse.StatusCode = 400;
            ApiResponse.Status = false;
            ApiResponse.Message = ApiErrorMessages.CAN_NOT_DELETE_MESSAGE;
            return BadRequest(ApiResponse);
        }
    }

    
    [HttpGet("ReadHistory")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetUprinceCustomerContactPreferenceHistory()
    {
        var contactPreference =
            await _uprinceCustomerContactPreferenceRepository.GetUprinceCustomerContactPreferenceHistory(
                _uPrinceCustomerContext);
        ApiOkResponse.Result = contactPreference;
        return Ok(ApiOkResponse);
    }
}