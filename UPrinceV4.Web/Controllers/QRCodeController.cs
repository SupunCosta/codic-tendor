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
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class QrCodeController : CommonConfigurationController
{
    private readonly IQrCodeRepository _iQrCodeRepository;
    private readonly ITimeClockActivityTypeRepository _iTimeClockActivityTypeRepository;

    public QrCodeController(IQrCodeRepository iQrCodeRepository,
        ITimeClockActivityTypeRepository iTimeClockActivityTypeRepository,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)
    {
        _iQrCodeRepository = iQrCodeRepository;
        _iTimeClockActivityTypeRepository = iTimeClockActivityTypeRepository;
    }

    [HttpGet("Read")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetQrCode()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            var ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            var qrCode = await _iQrCodeRepository.GetQrCode(UPrinceCustomerContext, lang,
                _iTimeClockActivityTypeRepository, ContractingUnitSequenceId, ProjectSequenceId, ItenantProvider);

            return Ok(!qrCode.Any()
                ? new ApiResponse(400, false, "noQrCodesAvailable")
                : new ApiOkResponse(qrCode));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    //[Microsoft.AspNetCore.Mvc.HttpGet("Read/{id}")]
    [HttpGet("ReadById/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetQrCodeById(string id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            var ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            var qrCode = await _iQrCodeRepository.GetTqrCodeById(UPrinceCustomerContext, id, lang, ItenantProvider,
                ContractingUnitSequenceId, ProjectSequenceId);

            return Ok(qrCode == null
                ? new ApiResponse(400, false, "noAvailableQrCodeForTheId")
                : new ApiOkResponse(qrCode));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    //[Microsoft.AspNetCore.Mvc.HttpGet("ReadByType/{type}")]
    [HttpGet("ReadByType/{type}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetQrCodeByType(int type)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());


            var ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            var ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            var qrCode = await _iQrCodeRepository.GetQrCodeByType(UPrinceCustomerContext, type,
                ContractingUnitSequenceId, ProjectSequenceId, ItenantProvider);

            return Ok(qrCode != null && !qrCode.Any()
                ? new ApiResponse(200, false, "NoAvailableQrCodeForTheType")
                : new ApiOkResponse(qrCode));
        }
        catch (Exception ex)
        {
            ApiResponse.StatusCode = 400;
            ApiResponse.Status = false;
            ApiResponse.Message = ex.Message;
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    //[Microsoft.AspNetCore.Mvc.HttpPost("Create")]
    [HttpPost("Create")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> CreateQrCode([FromBody] CreateQRCodeDto qrDto)
    {
        try
        {
           
            var ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            var ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var user = UPrinceCustomerContext.ApplicationUser.Where(u => u.OId == objectIdentifier)
                .FirstOrDefault();

            return Ok(new ApiOkResponse(await _iQrCodeRepository.CreateQrCode(UPrinceCustomerContext, qrDto, user.OId,
                ContractingUnitSequenceId, ProjectSequenceId, ItenantProvider)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

   
    [HttpPut("Update")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> UpdateQrCode(
        [FromBody] UpdateQRCodeDto timeClockDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());


            var ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            var ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            var id = await _iQrCodeRepository.UpdateQrCode(UPrinceCustomerContext, timeClockDto,
                ContractingUnitSequenceId, ProjectSequenceId, ItenantProvider);

            return Ok(new ApiOkResponse(id));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpDelete("Delete/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult DeleteQrCode(string id)
    {
        try
        {
            var ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            var ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _iQrCodeRepository.DeleteQrCode(UPrinceCustomerContext, id, ContractingUnitSequenceId,
                ProjectSequenceId, ItenantProvider);

            return Ok(new ApiResponse(200, false, "ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    //[Microsoft.AspNetCore.Mvc.HttpPost("QrCodeFilter")]
    [HttpPost("QrCodeFilter")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Filter([FromBody] QRCodeFilter filter)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(new ApiBadRequestResponse(ModelState));

            var ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            var ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());


            var qrCodes = await _iQrCodeRepository.Filter(UPrinceCustomerContext, filter, lang,
                _iTimeClockActivityTypeRepository, ItenantProvider, ContractingUnitSequenceId,
                ProjectSequenceId);
            var mApiOkResponse = new ApiOkResponse(qrCodes);
            mApiOkResponse.Result = qrCodes;
            mApiOkResponse.Message = ErrorMessageKey.Ok.ToString();
            mApiOkResponse.StatusCode = 200;


            if (qrCodes.Any()) return Ok(mApiOkResponse);
            var mApiResponse = new ApiOkResponse(null, "noQRCodesavailable")
            {
                Status = false
            };
            return Ok(mApiResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}