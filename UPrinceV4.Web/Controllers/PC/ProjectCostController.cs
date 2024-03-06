using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces.PC;
using UPrinceV4.Web.UserException;

namespace UPrinceV4.Web.Controllers.PC;

[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class ProjectCostController : CommonConfigurationController
{
    private readonly IProjectCostRepository _projectCostRepository;

    public ProjectCostController(ApplicationDbContext uPrinceCustomerContext,
        IProjectCostRepository projectCostRepository,
        IHttpContextAccessor contextAccessor, ApiResponse apiResponse, ApiBadRequestResponse apiBadRequestResponse,
        ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider) : base(uPrinceCustomerContext,
        contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse, iTenantProvider)
    {
        _projectCostRepository = projectCostRepository;
    }

    [HttpPost("ProjectCostFilter")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ProjectCostFilter(
        [FromBody] ProjectCostFilter projectCostFilter)
    {
        var lang = langCode(Request.Headers["lang"].FirstOrDefault());

        try
        {
            var _projectCostRepositoryParameter = new ProjectCostRepositoryParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                ApplicationDbContext = UPrinceCustomerContext,
                ProjectCostFilter = projectCostFilter,
                TenantProvider = ItenantProvider,
                Lang = lang
            };

            return Ok(new ApiOkResponse(await _projectCostRepository.Filter(_projectCostRepositoryParameter)));
        }
        catch (EmptyListException)
        {
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = false;
            ApiResponse.Message = "noCostAvailable";
            return Ok(ApiResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpDelete("IgnoreProjectCostCbcResources")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> IgnoreProjectCostCbcResources(
        [FromBody] List<string> idList)
    {
        var lang = langCode(Request.Headers["lang"].FirstOrDefault());

        try
        {
            var _projectCostRepositoryParameter = new ProjectCostRepositoryParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                ApplicationDbContext = UPrinceCustomerContext,
                IdList = idList,
                TenantProvider = ItenantProvider,
                Lang = lang
            };

            return Ok(new ApiOkResponse(await _projectCostRepository.IgnoreProjectCostCbcResources(_projectCostRepositoryParameter)));
        }
        catch (EmptyListException)
        {
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = false;
            ApiResponse.Message = "noCostAvailable";
            return Ok(ApiResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}