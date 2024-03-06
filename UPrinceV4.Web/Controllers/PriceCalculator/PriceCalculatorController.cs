using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.PriceCalculator;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces.PriceCalaculator;

namespace UPrinceV4.Web.Controllers.PriceCalculator;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class PriceCalculatorController : CommonConfigurationController
{
    private readonly IPriceCalculatorRepository _iPriceCalculatorRepository;
    private readonly PriceCalculatorParameter _priceCalculatorParameter;
    private readonly ITenantProvider _TenantProvider;


    public PriceCalculatorController(ITenantProvider tenantProvider,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider,
        IPriceCalculatorRepository iPriceCalculatorRepository, IConfiguration iConfiguration)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)

    {
        _iPriceCalculatorRepository = iPriceCalculatorRepository;
        _priceCalculatorParameter = new PriceCalculatorParameter();
        _TenantProvider = tenantProvider;
        _iConfiguration = iConfiguration;
    }

    private IConfiguration _iConfiguration { get; }

    [HttpPost("CreatePriceCalculatorTaxonomy")]
    public async Task<ActionResult> CreatePriceCalculatorTaxonomy([FromBody] CreatePriceCalculatorTaxonomy dto,
        [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _priceCalculatorParameter = new PriceCalculatorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                CreatePriceCalculatorTaxonomy = dto
            };

            return Ok(new ApiOkResponse(
                await _iPriceCalculatorRepository.CreatePriceCalculatorTaxonomy(_priceCalculatorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpDelete("DeletePriceCalculatorTaxonomy/{Id}")]
    public async Task<ActionResult> DeletePriceCalculatorTaxonomy(string Id, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _priceCalculatorParameter = new PriceCalculatorParameter
            {
                Id = Id,
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider
            };

            return Ok(new ApiOkResponse(
                await _iPriceCalculatorRepository.DeletePriceCalculatorTaxonomy(_priceCalculatorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetPriceCalculatorTaxonomy")]
    public async Task<ActionResult> GetPriceCalculatorTaxonomy([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _priceCalculatorParameter = new PriceCalculatorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider
            };

            return Ok(new ApiOkResponse(
                await _iPriceCalculatorRepository.GetPriceCalculatorTaxonomy(_priceCalculatorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetPriceCalculatorTaxonomyLevels")]
    public async Task<ActionResult> GetPriceCalculatorTaxonomyLevels([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _priceCalculatorParameter = new PriceCalculatorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider
            };

            return Ok(new ApiOkResponse(
                await _iPriceCalculatorRepository.GetPriceCalculatorTaxonomyLevels(_priceCalculatorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}