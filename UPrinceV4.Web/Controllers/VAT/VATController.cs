using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces.PS;

namespace UPrinceV4.Web.Controllers.VAT;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class VATController : CommonConfigurationController
{
    private readonly IVATRepository _IVATRepository;
    private readonly VATParameter _VATParameter;

    public VATController(ApplicationDbContext uPrinceCustomerContext, IVATRepository iVATRepository,
        IHttpContextAccessor contextAccessor, VATParameter VATParameter,
        ApiResponse apiResponse, ApiBadRequestResponse apiBadRequestResponse,
        ApiOkResponse apiOkResponse,
        ITenantProvider iTenantProvider) : base(uPrinceCustomerContext, contextAccessor, apiResponse,
        apiBadRequestResponse,
        apiOkResponse, iTenantProvider)
    {
        _IVATRepository = iVATRepository;
        _VATParameter = VATParameter;
    }


    [HttpGet("Filter")]
    
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> VATFilter()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            _VATParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _VATParameter.ProjectSequenceId = Request.Headers["project"].FirstOrDefault();
            _VATParameter.Lang = lang;
            _VATParameter.TenantProvider = ItenantProvider;
            var dropDownData = _IVATRepository.VATFilter(_VATParameter);
            ApiOkResponse.Result = dropDownData;
            ApiOkResponse.Message = "ok";
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}