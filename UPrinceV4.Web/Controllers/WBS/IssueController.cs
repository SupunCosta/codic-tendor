using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.Issue;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Controllers.WBS;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class IssueController : CommonConfigurationController
{
    private readonly IIssueRepository _issueRepository;
    
    private readonly IWbsRepository _iWbsRepository;

    public IssueController(ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse, ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider,IIssueRepository issueRepository,IWbsRepository iWbsRepository) : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse, iTenantProvider)
    {
        _issueRepository = issueRepository;
        _iWbsRepository = iWbsRepository;
    }
    
    [HttpPost("CreateIssue")]
    public async Task<ActionResult> CreateIssue(
        [FromBody] IssueHeaderCreateDto issueHeader,
        [FromHeader(Name = "CU")] string cu, [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var issueParameter = new IssueParameter()
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                IssueHeader = issueHeader,
                TenantProvider = ItenantProvider
            };
            return Ok(new ApiOkResponse(await _issueRepository.CreateIssue(issueParameter), "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("IssueFilter")]
    public async Task<ActionResult> IssueFilter(
        [FromBody] IssueFilterDto issueFilter,
        [FromHeader(Name = "CU")] string cu, [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var issueParameter = new IssueParameter()
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                IssueFilterDto = issueFilter,
                TenantProvider = ItenantProvider
            };
            var result = await _issueRepository.IssueFilter(issueParameter);
            return Ok(new ApiOkResponse(result, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpGet("IssueGetById/{id}")]
    public async Task<ActionResult> IssueGetById(
        string id,
        [FromHeader(Name = "CU")] string cu, [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var issueParameter = new IssueParameter()
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                Id = id,
                TenantProvider = ItenantProvider,
                iWbsRepository = _iWbsRepository
            };
            var result = await _issueRepository.IssueGetById(issueParameter);
            return Ok(new ApiOkResponse(result, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpGet("GetIssueDropDownData")]
    public async Task<ActionResult> GetIssueDropDownData(
        [FromHeader(Name = "CU")] string cu, [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var issueParameter = new IssueParameter()
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider
            };
            var result = await _issueRepository.GetIssueDropDownData(issueParameter);
            return Ok(new ApiOkResponse(result, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpDelete("DeleteIssueDocuments")]
    public async Task<ActionResult> DeleteIssueDocuments(
        [FromHeader(Name = "CU")] string cu, [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX,List<string> idList)
    {
        try
        {
            var lang = langCode(langX);
            var issueParameter = new IssueParameter()
            {
                ContractingUnitSequenceId = cu,
                ProjectSequenceId = project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider,
                IdList = idList
            };
            var result = await _issueRepository.DeleteIssueDocuments(issueParameter);
            return Ok(new ApiOkResponse(result, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}