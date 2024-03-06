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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UPrinceV4.Web.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class WorkflowStateController : CommonConfigurationController
{
    private readonly IWorkflowStateRepository _iWorkflowStateRepository;
    private readonly ITenantProvider _TenantProvider;



    public WorkflowStateController(IWorkflowStateRepository iWorkflowStateRepository,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)
    {
        _iWorkflowStateRepository = iWorkflowStateRepository;
        _TenantProvider = iTenantProvider;

    }

    [HttpGet("GetWorkflowStates")]
    
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetWorkflowStates([FromHeader(Name = "CU")] string cu,
        [FromHeader(Name = "Project")] string Project)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            return Ok(new ApiOkResponse(
                await _iWorkflowStateRepository.GetWorkflowStates(UPrinceCustomerContext, lang,_TenantProvider)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    [HttpGet("GetWorkflowStateById/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetWorkflowStateById(int id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            return Ok(new ApiOkResponse(
                await _iWorkflowStateRepository.GetWorkflowStateById(UPrinceCustomerContext, id, lang)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    [HttpDelete("Delete/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult DeleteWorkflowStateById(int id)
    {
        //try
        //{
        //    _iUPrinceCustomerProfileRepository.DeleteUPrinceCustomerProfile(_context, id);
        //    return Ok(new ApiResponse(204, true, "Customer Profile deleted successfully"));
        //}
        //catch (Exception ex)
        //{
        //    return BadRequest(new ApiResponse(400, false, ex.StackTrace));
        //}
        return null;
    }
}