using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.INV;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces.INV;

namespace UPrinceV4.Web.Controllers.INV;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class InvoiceController : CommonConfigurationController
{
    private readonly IInvoiceRepository _IInvoiceRepository;
    private readonly ITenantProvider _TenantProvider;

    public InvoiceController(ITenantProvider tenantProvider, IInvoiceRepository iInvoiceRepository,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse
    )
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            tenantProvider)
    {
        _TenantProvider = tenantProvider;
        _IInvoiceRepository = iInvoiceRepository;
    }


    [HttpPost("FilterInvoice")]
    public async Task<ActionResult> FilterInvoice([FromBody] InvoiceFilter filter)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var parameter = new InvoiceParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                filter = filter,
                ContextAccessor = ContextAccessor,
                TenantProvider = _TenantProvider
            };
            var result = await _IInvoiceRepository.InvoiceFilter(parameter);


            if (!result.Any())
            {
                const string msg = "noInvoiceAvailable";
                var mApiResponse = new ApiOkResponse(null, msg)
                {
                    Status = false
                };
                return Ok(mApiResponse);
            }

            return Ok(new ApiOkResponse(result));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetInvoiceDropdownData")]
    public async Task<ActionResult> GetPsDropdownData()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var parameter = new InvoiceParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                TenantProvider = ItenantProvider
            };
            return Ok(new ApiOkResponse(await _IInvoiceRepository.GetDropdownData(parameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadInVoiceById/{inVoiceId}")]
    public async Task<ActionResult> ReadInVoiceById(string inVoiceId)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var parameter = new InvoiceParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                TenantProvider = ItenantProvider,
                InvoiceId = inVoiceId
            };
            return Ok(new ApiOkResponse(await _IInvoiceRepository.ReadInVoiceById(parameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPut("CUApprovePs/{id}")]
    public async Task<ActionResult> CUApproveInvoice(string id)
    {
        try
        {
           
            var parameter = new InvoiceParameter
            {
                ApplicationDbContext = UPrinceCustomerContext,
                InvoiceId = id,
                TenantProvider = ItenantProvider,
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault()
            };
            return Ok(new ApiOkResponse(await _IInvoiceRepository.CUApproveInvoice(parameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPut("CUInReviewInvoice/{id}")]
    public async Task<ActionResult> CUInReviewInvoice(string id)
    {
        //_logger.LogTrace("Started");
        //_logger.LogError("ApprovePs " + id);
        try
        {
            
            var parameter = new InvoiceParameter
            {
                ApplicationDbContext = UPrinceCustomerContext,
                InvoiceId = id,
                TenantProvider = ItenantProvider,
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault()
            };
            return Ok(new ApiOkResponse(await _IInvoiceRepository.CUInReviewInvoice(parameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}