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

namespace UPrinceV4.Web.Controllers.PowerBi;

[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class PowerBiController : CommonConfigurationController
{
    private readonly IManagerCPCRepository _iManagerCPCRepository;

    private readonly ManagerCPCParameter _managerCPCParameter;
    private readonly ITenantProvider _tenantProvider;

    public PowerBiController(ITenantProvider tenantProvider,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider,
        IManagerCPCRepository managerCPCRepository)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)
    {
        _tenantProvider = tenantProvider;
        _managerCPCParameter = new ManagerCPCParameter();
        _iManagerCPCRepository = managerCPCRepository;
    }

    [AllowAnonymous]
    [HttpGet("ReadCPCByProjectsPM")]
    public async Task<ActionResult> ReadCPCByProjectsPM([FromHeader(Name = "AuthToken")] string AuthToken)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            //_managerCPCParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            //_managerCPCParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();

            if (AuthToken == "c6d04456-b40e-11eb-8529-0242ac130003")
            {
                _managerCPCParameter.Lang = lang;
                _managerCPCParameter.ContextAccessor = ContextAccessor;
                _managerCPCParameter.TenantProvider = _tenantProvider;

                return Ok(new ApiOkResponse(
                    await _iManagerCPCRepository.ReadCPCByProjectsPM(_managerCPCParameter)));
            }

            return BadRequest(new ApiResponse(400, false, "Please send token"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}