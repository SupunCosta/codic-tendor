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

namespace UPrinceV4.Web.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AppConfigurationController : CommonConfigurationController
{
    private readonly IAppConfigurationRepository _iAppConfigurationRepository;
    private readonly ITenantProvider _tenantProvider;

    public AppConfigurationController(IAppConfigurationRepository iAppConfigurationRepository,
        ITenantProvider tenantProvider,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)
    {
        _tenantProvider = tenantProvider;
        _iAppConfigurationRepository = iAppConfigurationRepository;
    }


    [HttpPost("ConfigureData")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ConfigureData([FromForm] IFormCollection csvFile)
    {
        try
        {
            return Ok(new ApiOkResponse(
                await _iAppConfigurationRepository.Configure(UPrinceCustomerContext, _tenantProvider, csvFile)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}