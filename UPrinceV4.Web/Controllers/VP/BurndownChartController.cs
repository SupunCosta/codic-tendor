using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.BurndownChart;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Controllers.VP;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]

public class BurndownChartController: CommonConfigurationController
{
    private readonly BurndownChartParameter _burndownChartParameter;
    private readonly IBurndownChartRepository _iBurndownChartRepository;
    
    public BurndownChartController(ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor,
        ApiResponse apiResponse, ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, 
        ITenantProvider iTenantProvider,BurndownChartParameter burndownChartParameter,IBurndownChartRepository iBurndownChartRepository) 
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse, iTenantProvider)
    {
        _burndownChartParameter = burndownChartParameter;
        _iBurndownChartRepository = iBurndownChartRepository;
    }

    
    [HttpPost("GetBurnDownChartData")]
    public async Task<ActionResult> GetBurnDownChartData([FromBody] BurndownChartDto BurndownChartDto)
    {
        try
        {
            _burndownChartParameter.Lang = langCode(Request.Headers["lang"].FirstOrDefault());
            
            _burndownChartParameter.ContextAccessor = ContextAccessor;
            _burndownChartParameter.TenantProvider = ItenantProvider;
            _burndownChartParameter.BurndownChartDto = BurndownChartDto;
            
            _burndownChartParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            
            var data = await _iBurndownChartRepository.GetBurnDownChartData(_burndownChartParameter);

            return Ok(new ApiOkResponse(data));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
}