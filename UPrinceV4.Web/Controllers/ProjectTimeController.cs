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
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
[Route("api/[controller]")]
[ApiController]
public class ProjectTimeController : CommonConfigurationController
{
    private readonly IProjectTimeRepository _iProjectTimeRepository;

    public ProjectTimeController(IProjectTimeRepository iProjectTimeRepository,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)
    {
        _iProjectTimeRepository = iProjectTimeRepository;
    }


    //[HttpGet("Read")]
    [HttpGet("Read")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetTime()
    {
        try
        {
            var projectTimes = UPrinceCustomerContext.ProjectTime
                .Include(t => t.ProjectDefinition)
                .Include(t => t.CalendarTemplate).ToList();
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = false;
            ApiResponse.Message = "No Project Time available";
            ApiOkResponse.Result = projectTimes;
            ApiOkResponse.Message = "Ok";
            return Ok(!projectTimes.Any() ? ApiResponse : ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    //[HttpGet("Read/{id}")]
    [HttpGet("ReadById")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetTimeById(string id)
    {
        try
        {
            var projectTime = UPrinceCustomerContext.ProjectTime
                .Include(t => t.ProjectDefinition)
                .Include(t => t.CalendarTemplate).Where(t => t.Id == id).ToList();
            //ApiResponse.StatusCode = 200;
            //ApiResponse.Status = false;
            //ApiResponse.Message = "No available project time for the Id ";
            //ApiOkResponse.Result = projectTime;
            //ApiOkResponse.Message = "Ok";
            return Ok(!projectTime.Any()
                ? new ApiResponse(400, false, "noAvailableProjectTimeForTheId")
                : new ApiOkResponse(projectTime));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    //[HttpPost("Create")]
    [HttpPost("Create")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreateProjectTime([FromBody] ProjectTimeCreateDto timedto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var user = UPrinceCustomerContext.ApplicationUser.Where(u => u.OId == objectIdentifier)
                .FirstOrDefault();
            var Id = _iProjectTimeRepository.CreateProjectTime( timedto, user, ItenantProvider);
            //ApiOkResponse.Result = Id;
            //ApiOkResponse.Message = "Ok";
            return Ok(new ApiOkResponse(Id));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    //[HttpPut("Update")]
    [HttpPut("Update")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> UpdateProjectTime([FromBody] ProjectTimeUpdateDto timedto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var user = UPrinceCustomerContext.ApplicationUser.Where(u => u.OId == objectIdentifier)
                .FirstOrDefault();
            var Id = _iProjectTimeRepository.UpdateProjectTime( timedto, user, ItenantProvider);
            //ApiOkResponse.Result = Id;
            //ApiOkResponse.Message = "Ok";
            return Ok(new ApiOkResponse(Id));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpDelete("Delete")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult DeleteProjectTime(string id)
    {
        try
        {
            var projectTime = (from a in UPrinceCustomerContext.ProjectTime
                where a.Id == id
                select a).Single();
            UPrinceCustomerContext.ProjectTime.Remove(projectTime);
            UPrinceCustomerContext.SaveChanges();
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = false;
            ApiResponse.Message = "Project Time deleted successfully";
            return Ok(ApiResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}