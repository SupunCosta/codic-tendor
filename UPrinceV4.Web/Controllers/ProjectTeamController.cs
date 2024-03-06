using System;
using System.Collections.Generic;
using System.Linq;
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
public class ProjectTeamController : CommonConfigurationController
{
    private readonly IProjectTeamRepository _iProjectTeamRepository;
    private readonly ProjectTeamRoleParameter _ProjectTeamRoleParameter;

    public ProjectTeamController(IProjectTeamRepository iProjectTeamRepository,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider,
        ProjectTeamRoleParameter projectTeamRoleParameter)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)
    {
        _iProjectTeamRepository = iProjectTeamRepository;
        _ProjectTeamRoleParameter = projectTeamRoleParameter;
    }
    
    [HttpGet("Read/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetProjectTeamById(string id)
    {
        try
        {
            //var projectTeam = UPrinceCustomerContext.ProjectTeam.Include(t => t.ProjectOwner)
            //    .Include(t => t.ProjectManager)
            //    .Include(t => t.ProjectEngineer)
            //    .Include(t => t.Project).Where(t => t.Id == id).ToList();
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = false;
            ApiResponse.Message = "No available project team for the id " + id;
            //ApiOkResponse.Result = projectTeam;
            ApiOkResponse.Message = "Ok";
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

   
    [HttpGet("ReadContractingUnitByName")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ReadContractingUnitByName(string name)
    {
        try
        {
            //_logger.LogTrace("Started");
            //_logger.LogError("ReadContractingUnit");
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            _ProjectTeamRoleParameter.Lang = lang;
            _ProjectTeamRoleParameter.ContextAccessor = ContextAccessor;
            _ProjectTeamRoleParameter.TenantProvider = ItenantProvider;
            _ProjectTeamRoleParameter.Name = name;
            return Ok(new ApiOkResponse(
                await _iProjectTeamRepository.GetContractingUnit(_ProjectTeamRoleParameter), "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpGet("ReadContractingUnit")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ReadContractingUnit()
    {
        try
        {
            //_logger.LogTrace("Started");
            //_logger.LogError("ReadContractingUnit");
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            _ProjectTeamRoleParameter.Lang = lang;
            _ProjectTeamRoleParameter.ContextAccessor = ContextAccessor;
            _ProjectTeamRoleParameter.TenantProvider = ItenantProvider;

            return Ok(new ApiOkResponse(
                await _iProjectTeamRepository.GetAllContractingUnit(_ProjectTeamRoleParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    [HttpGet("ReadProjectTeamRole/{projectId}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ReadProjectTeamRole(string projectId)
    {
        try
        {
            // _logger.LogTrace("Started");
            // _logger.LogError("ReadProjectTeamRole");
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            _ProjectTeamRoleParameter.Lang = lang;
            _ProjectTeamRoleParameter.ContextAccessor = ContextAccessor;
            _ProjectTeamRoleParameter.TenantProvider = ItenantProvider;
            _ProjectTeamRoleParameter.Id = projectId;

            return Ok(
                new ApiOkResponse(await _iProjectTeamRepository.GetProjectTeam(_ProjectTeamRoleParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    
    [HttpDelete("DeleteTeamRole")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteProjectTeam([FromBody] List<string> idList)
    {
        try
        {
            // _logger.LogTrace("Started");
            // _logger.LogError("DeleteTeamRole" + JsonToStringConverter.getStringFromJson(idList));
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            _ProjectTeamRoleParameter.Lang = lang;
            _ProjectTeamRoleParameter.ProjectSequenceCode = Request.Headers["project"].FirstOrDefault();
            _ProjectTeamRoleParameter.IdList = idList;
            _ProjectTeamRoleParameter.TenantProvider = ItenantProvider;
            await _iProjectTeamRepository.DeleteProjectTeamRole(_ProjectTeamRoleParameter);
            return Ok(new ApiOkResponse("Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("CreateAdUser")]
    public async Task<string> CreateAdUser()
    {
        try
        {
            var jsonresult2 = AzureInvitation.CreateUser("ayesha.bandara94@gmail.com", "AyeshaBandara94", "message",
                "ayesha@mickiesoft.com", true, ItenantProvider.GetTenant().CatelogConnectionString);
            var jsonresult = AzureInvitation.SendInvitation("ayesha.bandara94@gmail.com", "AyeshaBandara94",
                "message", "ayesha@mickiesoft.com", true, ItenantProvider.GetTenant().CatelogConnectionString);
            return "Ok";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}