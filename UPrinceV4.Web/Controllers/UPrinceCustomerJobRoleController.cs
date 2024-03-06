using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Controllers;


public class UPrinceCustomerJobRoleController : CommonConfigurationController
{
    private readonly IUPrinceCustomerJobRoleRepository _iUPrinceCustomerJobRoleRepository;
    private readonly UPrinceCustomerContex _uPrinceCustomerContext;

    public UPrinceCustomerJobRoleController(IUPrinceCustomerJobRoleRepository iUPrinceCustomerJobRoleRepository,
        UPrinceCustomerContex uPrinceCustomerContext, ApplicationDbContext _context,
        IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider)
        : base(_context, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse, iTenantProvider)
    {
        _uPrinceCustomerContext = uPrinceCustomerContext;
        _iUPrinceCustomerJobRoleRepository = iUPrinceCustomerJobRoleRepository;
    }

    
    [HttpGet("GetUPrinceCustomerJobRoles")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetUPrinceCustomerJobRoles()
    {
        var jobRoles = await _iUPrinceCustomerJobRoleRepository.GetUPrinceCustomerJobRoles(_uPrinceCustomerContext);
        ApiOkResponse.Result = jobRoles;
        return Ok(ApiOkResponse);
    }

   
    [HttpGet("GetUPrinceCustomerJobRoleById/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetUPrinceCustomerJobRoleById(int id)
    {
        try
        {
            var jobrole =
                await _iUPrinceCustomerJobRoleRepository.GetUPrinceCustomerJobRoleById(_uPrinceCustomerContext, id);
            ApiOkResponse.Result = jobrole;
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    // POST: api/UprinceCustomerJobRole
    //[HttpPost("Create")]
    [HttpPost("Create")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreateUPrinceCustomerJobRole(
        [FromBody] UPrinceCustomerJobRoleDto uprinceCustomerJobRoleDto)
    {
        var uprinceCustomerJobRole = new UPrinceCustomerJobRole
        {
            Role = uprinceCustomerJobRoleDto.Role
        };
        try
        {
            var id = await _iUPrinceCustomerJobRoleRepository.AddUPrinceCustomerJobRole(_uPrinceCustomerContext,
                uprinceCustomerJobRole);
            ApiOkResponse.Result = id;
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

  
    [HttpDelete("Delete/{id}")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult DeleteUPrinceCustomerJobRoleById(int id)
    {
        try
        {
            _iUPrinceCustomerJobRoleRepository.DeleteUPrinceCustomerJobRole(_uPrinceCustomerContext, id);
            ApiResponse.StatusCode = 400;
            ApiResponse.Status = false;
            ApiResponse.Message = "JobRole deleted successfully";
            return Ok(ApiResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpDelete("ReadHistory")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetUPrinceCustomerJobRoleHistory()
    {
        var customers =
            await _iUPrinceCustomerJobRoleRepository.GetUPrinceCustomersHistory(_uPrinceCustomerContext);
        ApiOkResponse.Result = customers;
        return Ok(ApiOkResponse);
    }

    //[HttpDelete("Delete")]
    [HttpDelete("Delete")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult DeleteUPrinceCustomerJobRoleHistory()
    {
        try
        {
            _iUPrinceCustomerJobRoleRepository.DeleteUPrinceCustomerJobRoleHistory(_uPrinceCustomerContext);
            ApiResponse.StatusCode = 400;
            ApiResponse.Status = false;
            ApiResponse.Message = "JobRole deleted successfully";
            return Ok(ApiResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}