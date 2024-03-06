using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces.PBS;

namespace UPrinceV4.Web.Controllers.PBS;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class PbsQualityController : CommonConfigurationController
{
    private readonly IPbsQualityRepository _IPbsQualityRepository;

    public PbsQualityController( IPbsQualityRepository iPbsQualityRepository,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse,
        ITenantProvider iTenantProvider)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)

    {
        _IPbsQualityRepository = iPbsQualityRepository;
    }

    [HttpPost("Create")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreatePbsQuality([FromBody] PbsQualityCreateDto pbsDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _PbsQualityParameters = new PbsQualityParameters
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                PbsQualityCreateDto = pbsDto,
                TenantProvider = ItenantProvider
            };
            return Ok(new ApiOkResponse(await _IPbsQualityRepository.CreatePbsQuality(_PbsQualityParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpDelete("DeletePbsQuality")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeletePbsQuality([FromBody] List<string> idList)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _PbsQualityParameters = new PbsQualityParameters
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                IdList = idList,
                TenantProvider = ItenantProvider
            };
            await _IPbsQualityRepository.DeletePbsQuality(_PbsQualityParameters);
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadQualityByPbsProduct/{pbsProductId}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetQualityByPbsProductId(string pbsProductId)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _PbsQualityParameters = new PbsQualityParameters
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                TenantProvider = ItenantProvider,
                PbsProductId = pbsProductId,
                Lang = lang
            };

            var pbsQuality = await _IPbsQualityRepository.GetPbsQualityByPbsProductId(_PbsQualityParameters);

            return Ok(pbsQuality == null
                ? new ApiOkResponse(null, "noAvailableRiskForTheId")
                : new ApiOkResponse(pbsQuality));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetAllQualityByPbsProductId/{pbsProductId}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetAllQualityByPbsProductId(string pbsProductId)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _PbsQualityParameters = new PbsQualityParameters
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                TenantProvider = ItenantProvider,
                PbsProductId = pbsProductId,
                Lang = lang
            };
            var pbsQuality = await _IPbsQualityRepository.GetAllPbsQualityByPbsProductId(_PbsQualityParameters);
            return Ok(pbsQuality == null
                ? new ApiOkResponse(null, "noAvailableRiskForTheId")
                : new ApiOkResponse(pbsQuality));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}