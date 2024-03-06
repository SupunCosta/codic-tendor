using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AdaptiveCards;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Models;

namespace UPrinceV4.Web.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class AdaptiveCardController : CommonConfigurationController
{
    private readonly ITenantProvider _TenantProvider;

    public AdaptiveCardController(ITenantProvider tenantProvider,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider
        , IConfiguration iConfiguration)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)

    {
        _TenantProvider = tenantProvider;
        _iConfiguration = iConfiguration;
    }

    private IConfiguration _iConfiguration { get; }

    [HttpGet("ParseAdaptiveCard")]
    public async Task<ActionResult> ParseAdaptiveCard([FromHeader(Name = "CU")] string cu,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            // var lang = langCode(langX);
            // _vpParameter.ContractingUnitSequenceId = cu;
            // _vpParameter.ProjectSequenceId = Project;
            // _vpParameter.Lang = lang;
            // _vpParameter.ContextAccessor = ContextAccessor;
            // _vpParameter.TenantProvider = _TenantProvider;
            // var shortcutPaneData = await _iVPRepository.GetShortcutpaneData(_vpParameter);

            var client = new HttpClient();
            var response = await client.GetAsync("http://adaptivecards.io/payloads/ActivityUpdate.json");
            var json = await response.Content.ReadAsStringAsync();

            // Parse the JSON 
            var result = AdaptiveCard.FromJson(json);

            // Get card from result
            var card = result.Card;

            // Optional: check for any parse warnings
            // This includes things like unknown element "type"
            // or unknown properties on element
            IList<AdaptiveWarning> warnings = result.Warnings;

            return Ok(new ApiOkResponse(card));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}