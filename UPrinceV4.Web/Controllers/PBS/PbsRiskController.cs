using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
public class PbsRiskController : CommonConfigurationController
{
    private readonly IPbsRiskRepository _IPbsRiskRepository;

    public PbsRiskController(IPbsRiskRepository iPbsRiskRepository,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse,
       ITenantProvider iTenantProvider)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)

    {
        _IPbsRiskRepository = iPbsRiskRepository;
    }

    [HttpPost("Create")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreatePbsRisk([FromBody] PbsRiskCreateDto pbsDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _PbsRiskParameters = new PbsRiskParameters
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                PbsRiskCreateDto = pbsDto,
                TenantProvider = ItenantProvider
            };
            var id = await _IPbsRiskRepository.CreatePbsRisk(_PbsRiskParameters);

            return Ok(new ApiOkResponse(id));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpDelete("DeletePbsRisk")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeletePbsRisk([FromBody] List<string> idList)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _PbsRiskParameters = new PbsRiskParameters
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                IdList = idList,
                TenantProvider = ItenantProvider
            };
            await _IPbsRiskRepository.DeletePbsRisk(_PbsRiskParameters);
            return Ok(new ApiOkResponse("ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadRiskByPbsProduct/{pbsProductId}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetRiskByPbsProductId(string pbsProductId)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _PbsRiskParameters = new PbsRiskParameters
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                TenantProvider = ItenantProvider,
                PbsProductId = pbsProductId,
                Lang = lang
            };

            var pbsRisk = await _IPbsRiskRepository.GetPbsRiskByPbsProductId(_PbsRiskParameters);

            return Ok(pbsRisk == null
                ? new ApiOkResponse(null, "noAvailableRiskForTheId")
                : new ApiOkResponse(pbsRisk));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetAllRiskByPbsProductId/{pbsProductId}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetAllRiskByPbsProductId(string pbsProductId)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _PbsRiskParameters = new PbsRiskParameters
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                TenantProvider = ItenantProvider,
                PbsProductId = pbsProductId,
                Lang = lang
            };

            var pbsRisk = await _IPbsRiskRepository.GetAllPbsRiskByPbsProductId(_PbsRiskParameters);

            return Ok(pbsRisk == null
                ? new ApiOkResponse(null, "noAvailableRiskForTheId")
                : new ApiOkResponse(pbsRisk));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}