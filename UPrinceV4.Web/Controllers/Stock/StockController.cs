using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.Stock;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Controllers.Stock;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class StockController : CommonConfigurationController
{
    private readonly IStockRepository _iStockRepository;
    private readonly ITenantProvider _TenantProvider;

    public StockController(ITenantProvider tenantProvider,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApplicationDbContext dbContext,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse,
        ILogger<StockController> logger, ITenantProvider iTenantProvider
        , IStockRepository iStockRepository)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)

    {
        _iStockRepository = iStockRepository;
        _TenantProvider = tenantProvider;
    }

    [HttpGet("ShortcutPaneData")]
    public async Task<ActionResult> ReadWHShortcutPaneData([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            var _stockParameter = new StockParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = _TenantProvider
            };
            return Ok(new ApiOkResponse(await _iStockRepository.GetShortcutpaneData(_stockParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("FilterStock")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetStockList([FromBody] StockFilter filter,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            var _stockParameter = new StockParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                Filter = filter,
                ContextAccessor = ContextAccessor,
                TenantProvider = _TenantProvider
            };

            var result = await _iStockRepository.GetStockList(_stockParameter);
            var mApiOkResponse = new ApiOkResponse(result);


            if (!result.Any())
            {
                var mApiResponse = new ApiOkResponse(null, "noAvailableStock");
                mApiResponse.Status = false;
                return Ok(mApiResponse);
            }

            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CreateHeader")]
    public async Task<ActionResult> CreateHeader([FromBody] StockCreateDto StockDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(langX);

            var _stockParameter = new StockParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                StockDto = StockDto,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };
            var s = await _iStockRepository.CreateHeader(_stockParameter);
            return Ok(new ApiOkResponse(s));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetStockDropdown")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetStockDropdown([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _stockParameter = new StockParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                TenantProvider = ItenantProvider
            };

            return Ok(new ApiOkResponse(await _iStockRepository.GetStockDropdown(_stockParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetById/{SequenceId}")]
    public async Task<ActionResult> GetStockById(string SequenceId, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _stockParameter = new StockParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                Id = SequenceId,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider
            };
            return Ok(new ApiOkResponse(await _iStockRepository.GetStockById(_stockParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}