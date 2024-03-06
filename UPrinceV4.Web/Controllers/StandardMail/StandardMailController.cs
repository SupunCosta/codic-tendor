using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.StandardMails;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Controllers.StandardMail;

[Authorize]
[Microsoft.AspNetCore.Components.Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class StandardMailController : CommonConfigurationController
{
    private readonly IStandardMailRepositary _iStandardMailRepositary;
    private readonly ITenantProvider _TenantProvider;


    public StandardMailController(ITenantProvider tenantProvider,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider
        , IStandardMailRepositary iStandardMailRepositary)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)

    {
        _TenantProvider = tenantProvider;
        _iStandardMailRepositary = iStandardMailRepositary;
    }

    [HttpPost("CreateStandardMail")]
    public async Task<ActionResult> CreateStandardMail([FromBody] StandardMailHeaderDto StandardMailDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(langX);
            var standardMailParameter = new StandardMailParameters
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                StandardMailDto = StandardMailDto,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };
            return Ok(new ApiOkResponse(await _iStandardMailRepositary.StandardMailCreate(standardMailParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("StandardMailFilter")]
    public async Task<ActionResult> StandardMailFilter([FromBody] StandardMailFilter StandardMailFilter,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(langX);
            var standardMailParameter = new StandardMailParameters
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                Filter = StandardMailFilter,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };
            return Ok(new ApiOkResponse(await _iStandardMailRepositary.StandardMailFilter(standardMailParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("StandardMailGetById/{SequenceId}")]
    public async Task<ActionResult> StandardMailGetById(string SequenceId,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(langX);
            var standardMailParameter = new StandardMailParameters
            {
                Id = SequenceId,
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };
            return Ok(new ApiOkResponse(await _iStandardMailRepositary.StandardMailGetById(standardMailParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}