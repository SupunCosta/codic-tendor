using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Repositories.Interfaces.PMOL;

namespace UPrinceV4.Web.Controllers;

[Authorize]
[Microsoft.AspNetCore.Components.Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class ForemanMigrationController : CommonConfigurationController
{
    private readonly ForemanMigrationParameter _foremanMigrationParameter;
    private readonly IConfiguration _iConfiguration;
    private readonly IForemanMigrationRepository _iForemanMigrationRepository;
    private readonly IPmolRepository _iPmolRepository;
    private readonly ITenantProvider _TenantProvider;


    public ForemanMigrationController(IForemanMigrationRepository iForemanMigrationRepository,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider,
        IConfiguration iConfiguration, IPmolRepository iPmolRepository)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)
    {
        _iForemanMigrationRepository = iForemanMigrationRepository;
        _foremanMigrationParameter = new ForemanMigrationParameter();
        _TenantProvider = iTenantProvider;
        _iPmolRepository = iPmolRepository;
        _iConfiguration = iConfiguration;
    }

    [HttpGet("ForemanAddToPmolTeam")]
    public async Task<ActionResult> ForemanAddToPmolTeam([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _foremanMigrationParameter.ContractingUnitSequenceId = CU;
            _foremanMigrationParameter.ProjectSequenceId = Project;
            _foremanMigrationParameter.Lang = lang;
            _foremanMigrationParameter.ContextAccessor = ContextAccessor;
            _foremanMigrationParameter.TenantProvider = _TenantProvider;
            _foremanMigrationParameter.PmolRepository = _iPmolRepository;
            _foremanMigrationParameter.Configuration = _iConfiguration;

            return Ok(new ApiOkResponse(
                await _iForemanMigrationRepository.ForemanAddToPmolTeam(_foremanMigrationParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}