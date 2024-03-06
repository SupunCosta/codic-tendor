using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UPrinceV4.Web.Controllers;


[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UPrinceCustomerController : CommonConfigurationController
{
    private readonly IUPrinceCustomerRepository _iUPrinceCustomerRepository;
    private readonly UPrinceCustomerContex _uPrinceCustomerContext;

    public UPrinceCustomerController(IUPrinceCustomerRepository iUPrinceCustomerRepository,
        UPrinceCustomerContex uPrinceCustomerContext, ApplicationDbContext _context,
        IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider)
        : base(_context, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse, iTenantProvider)
    {
        _uPrinceCustomerContext = uPrinceCustomerContext;
        _iUPrinceCustomerRepository = iUPrinceCustomerRepository;
    }

   
    [HttpGet("GetUPrinceCustomers")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetUPrinceCustomers()
    {
        var customers = await _iUPrinceCustomerRepository.GetUPrinceCustomers(_uPrinceCustomerContext);
        ApiOkResponse.Result = customers;
        return Ok(ApiOkResponse);
    }
    
    [HttpGet("GetUPrinceCustomerById/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetUPrinceCustomerById(int id)
    {
        try
        {
            var uprinceCustomer =
                await _iUPrinceCustomerRepository.GetUPrinceCustomerById(_uPrinceCustomerContext, id);
            ApiOkResponse.Result = uprinceCustomer;
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    // POST api/<controller>
    //[HttpPost("Create")]
    [HttpPost("Create")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreateUPrinceCustomer([FromBody] UPrinceCustomerDto uprinceCustomerDto)
    {
        var uprinceCustomer = new UPrinceCustomer();
        try
        {
            //var oid = _ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim => claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            //var user = _context.AllUsers.Where(u => u.OId == oid).Include(u => u.UserRole).ThenInclude(u => u.Role);
            await _iUPrinceCustomerRepository.AddUPrinceCustomer(_uPrinceCustomerContext, uprinceCustomer);
            ApiOkResponse.Result = uprinceCustomer.Id;
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    // DELETE api/<controller>/5
    //[HttpDelete("Delete/{id}")]
    [HttpDelete("Delete/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult DeleteUPrinceCustomerById(int id)
    {
        try
        {
            _iUPrinceCustomerRepository.DeleteUPrinceCustomer(_uPrinceCustomerContext, id);
            ApiResponse.StatusCode = 400;
            ApiResponse.Status = false;
            ApiResponse.Message = "UPrinceCustomer deleted successfully";
            return Ok(ApiResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

   
    [HttpGet("ReadHistory")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetUPrinceCustomersHistory()
    {
        var customers = await _iUPrinceCustomerRepository.GetUPrinceCustomersHistory(_uPrinceCustomerContext);
        ApiOkResponse.Result = customers;
        return Ok(ApiOkResponse);
    }
}