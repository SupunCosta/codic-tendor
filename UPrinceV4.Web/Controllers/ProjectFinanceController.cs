using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class ProjectFinanceController : CommonConfigurationController
{
    private readonly IProjectFinanceRepository _iProjectFinanceRepository;

    public ProjectFinanceController(IProjectFinanceRepository iProjectFinanceRepository,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)
    {
        _iProjectFinanceRepository = iProjectFinanceRepository;
    }

    //[HttpGet("Read")]
    [HttpGet("Read")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetFinance()
    {
        try
        {
            var finance = UPrinceCustomerContext.ProjectFinance
                .Include(t => t.ProjectDefinition)
                .Include(t => t.Currency).ToList();
            ApiResponse.Message = "No Project Finance available";
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = false;
            ApiOkResponse.Result = finance;
            ApiOkResponse.Message = "Ok";
            return Ok(!finance.Any() ? ApiResponse : ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpGet("GetFinanceById/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetFinanceById(string id)
    {
        try
        {
            var finance = UPrinceCustomerContext.ProjectFinance
                .Include(t => t.ProjectDefinition)
                .Include(t => t.Currency)
                .Where(f => f.Id == id).ToList();
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = false;
            ApiResponse.Message = "No available Finance for the Id " + id;
            ApiOkResponse.Result = finance;
            ApiOkResponse.Message = "Ok";
            return Ok(!finance.Any() ? ApiResponse : ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    //[HttpPost("Create")]
    [HttpPost("Create")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateProjectFinance([FromBody] ProjectFinanceCreateDto financeDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }
            
            return Ok(new ApiOkResponse(_iProjectFinanceRepository.CreateProjectFinance( financeDto, 
                ItenantProvider)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPut("Update")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateProjectFinance([FromBody] ProjectFinanceUpdateDto financedto)
    {
        try
        {
            return Ok(new ApiOkResponse( _iProjectFinanceRepository.UpdateProjectFinance(financedto, 
                ItenantProvider)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

  
    [HttpDelete("Delete/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteProjectFinance(string id)
    {
        try
        {
            var finance = (from a in UPrinceCustomerContext.ProjectFinance
                where a.Id == id
                select a).Single();
            UPrinceCustomerContext.ProjectFinance.Remove(finance);
            UPrinceCustomerContext.SaveChanges();
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = true;
            ApiResponse.Message = "Project Finance deleted successfully";
            return Ok(ApiResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}