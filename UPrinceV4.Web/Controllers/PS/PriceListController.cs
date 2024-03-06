using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.PC;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces.PS;

namespace UPrinceV4.Web.Controllers.PS;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class PriceListController : CommonConfigurationController
{
    private readonly IPriceListRepository _iPriceListRepository;

    public PriceListController(
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse,
        ITenantProvider iTenantProvider,
        IPriceListRepository iPriceListRepository)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)
    {
        _iPriceListRepository = iPriceListRepository;
    }

    [HttpPost("CreateResourceTypePriceList")]
    public async Task<ActionResult> CreateResourceTypePriceList([FromBody] ResourceTypePriceListCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var parameter = new PriceListParameter();
            parameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            parameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            parameter.Lang = lang;
            parameter.ResourceTypePriceListCreateDto = dto;
            parameter.ContextAccessor = ContextAccessor;
            parameter.TenantProvider = ItenantProvider;
            parameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var id = await _iPriceListRepository.CreateResourceTypePriceList(parameter);
            return Ok(new ApiOkResponse(id));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("ReadResourceTypePriceList")]
    public async Task<ActionResult> ReadResourceTypePriceList([FromBody] ResourceTypePriceListReadDto dto)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var parameter = new PriceListParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = dto.ProjectSequenceCode,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider
            };
            return Ok(new ApiOkResponse(await _iPriceListRepository.ReadResourceTypePriceList(parameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CreateResourceItemPriceList")]
    public async Task<ActionResult> CreateResourceItemPriceList([FromBody] ResourceItemPriceList dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var parameter = new PriceListParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                ResourceItemPriceListCreateDto = dto,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };
            return Ok(new ApiOkResponse(await _iPriceListRepository.CreateResourceItemPriceList(parameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadConsumablePriceList")]
    public async Task<ActionResult> ReadConsumablePriceList()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var parameter = new PriceListParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider
            };
            var priceList = await _iPriceListRepository.ReadConsumablePriceList(parameter);
            return Ok(new ApiOkResponse(priceList, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadLabourPriceList")]
    public async Task<ActionResult> ReadLabourPriceList()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var parameter = new PriceListParameter();
            parameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            parameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            parameter.Lang = lang;
            parameter.ContextAccessor = ContextAccessor;
            parameter.TenantProvider = ItenantProvider;
            var priceList = await _iPriceListRepository.ReadLabourPriceList(parameter);
            return Ok(new ApiOkResponse(priceList, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadMaterialPriceList")]
    public async Task<ActionResult> ReadMaterialPriceList()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var parameter = new PriceListParameter();
            parameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            parameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            parameter.Lang = lang;
            parameter.ContextAccessor = ContextAccessor;
            parameter.TenantProvider = ItenantProvider;
            var priceList = await _iPriceListRepository.ReadMaterialPriceList(parameter);
            return Ok(new ApiOkResponse(priceList, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadToolPriceList")]
    public async Task<ActionResult> ReadToolPriceList()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var parameter = new PriceListParameter();
            parameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            parameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            parameter.Lang = lang;
            parameter.ContextAccessor = ContextAccessor;
            parameter.TenantProvider = ItenantProvider;
            var priceList = await _iPriceListRepository.ReadToolPriceList(parameter);
            return Ok(new ApiOkResponse(priceList, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpDelete("DeleteResourceItemPriceList")]
    public async Task<IActionResult> DeleteResourceItemPriceList(
        [FromBody] List<string> idList)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var parameter = new PriceListParameter();
            parameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            parameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            parameter.Lang = lang;
            parameter.ContextAccessor = ContextAccessor;
            parameter.TenantProvider = ItenantProvider;
            parameter.idList = idList;
            var priceList = await _iPriceListRepository.DeleteResourceItemPriceList(parameter);
            return Ok(new ApiOkResponse(priceList, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}