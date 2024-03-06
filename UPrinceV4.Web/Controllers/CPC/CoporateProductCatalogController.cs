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
using UPrinceV4.Web.Data.CPC;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces.CPC;
using UPrinceV4.Web.UserException;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Controllers.CPC;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class CorporateProductCatalogController : CommonConfigurationController
{
    private readonly ICoporateProductCatalogRepository _iCoporateProductCatalogRepository;
    private readonly ITenantProvider _TenantProvider;

    public CorporateProductCatalogController(ITenantProvider tenantProvider,
        ICoporateProductCatalogRepository iCoporateProductCatalogRepository,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse,
       ITenantProvider iTenantProvider)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)

    {
        _iCoporateProductCatalogRepository = iCoporateProductCatalogRepository;
        _TenantProvider = tenantProvider;
       
    }

    [HttpPost("CreateCpc")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateCpc(
        [FromBody] CoperateProductCatalogCreateDto cpcDto)
    {
        try
        {
            //_logger.LogTrace("Started");
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var oid = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var userId = UPrinceCustomerContext.ApplicationUser.First(u => u.OId == oid).OId;
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var cpcParameters = new CpcParameters
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                CpcDto = cpcDto,
                Context = UPrinceCustomerContext,
                ContextAccessor = ContextAccessor,
                TenantProvider = _TenantProvider,
                isCopy = false,
                Oid = userId
            };

            return Ok(new ApiOkResponse(
                await _iCoporateProductCatalogRepository.CreateCoporateProductCatalog(cpcParameters,
                    ContextAccessor)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    //[Microsoft.AspNetCore.Mvc.HttpGet("ReadDropdownData")]
    [HttpGet("GetCpcDropdownData")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetCpcDropdownData()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var cpcParameters = new CpcParameters
            {
                // ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                // ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                TenantProvider = _TenantProvider
            };

            return Ok(new ApiOkResponse(await _iCoporateProductCatalogRepository.GetCpcDropdown(cpcParameters)));
        }
        catch (EmptyListException ex)
        {
            return BadRequest(new ApiResponse(200, false, ex.Message));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("UploadImage")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> UploadImage([FromForm] IFormCollection image)
    {
        try
        {
            // _logger.LogTrace("Started");
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var cpcParameters = new CpcParameters
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                Context = UPrinceCustomerContext,
                Image = image,
                TenantProvider = _TenantProvider
            };
            return Ok(new ApiOkResponse(await _iCoporateProductCatalogRepository.UploadImage(cpcParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpDelete("DeleteVendor")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteVendor([FromBody] List<string> idList)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var cpcParameters = new CpcParameters
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                //string message = ApiErrorMessages.GetErrorMessage(ItenantProvider, ErrorMessageKey.Ok, lang).Message;
                Lang = lang,
                Context = UPrinceCustomerContext,
                IdList = idList,
                TenantProvider = _TenantProvider
            };
            await _iCoporateProductCatalogRepository.DeleteVendor(cpcParameters);
            //ApiOkResponse.Message = message;
            return Ok(new ApiOkResponse("Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    //[Microsoft.AspNetCore.Mvc.HttpDelete("DeleteImage")]
    [HttpDelete("DeleteImage")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteImage([FromBody] List<string> idList)
    {
        try
        {
            //_logger.LogTrace("Started");
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var cpcParameters = new CpcParameters
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                Context = UPrinceCustomerContext,
                IdList = idList,
                TenantProvider = _TenantProvider
            };
            await _iCoporateProductCatalogRepository.DeleteImage(cpcParameters);
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    //[Microsoft.AspNetCore.Mvc.HttpDelete("DeleteNickName")]
    [HttpDelete("DeleteNickName")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteNickName([FromBody] List<string> idList)
    {
        try
        {
            //_logger.LogTrace("Started");
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var cpcParameters = new CpcParameters
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                //string message = ApiErrorMessages.GetErrorMessage(ItenantProvider, ErrorMessageKey.Ok, lang).Message;
                Lang = lang,
                Context = UPrinceCustomerContext,
                IdList = idList,
                TenantProvider = _TenantProvider
            };
            await _iCoporateProductCatalogRepository.DeleteNickName(cpcParameters);
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    //[Microsoft.AspNetCore.Mvc.HttpGet("Read/{id}")]
    [HttpGet("Read/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetCorporateProductCatalogById(string id)
    {
        try
        {
            //_logger.LogTrace("Started");
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var cpcParameters = new CpcParameters
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                Context = UPrinceCustomerContext,
                Id = id,
                ContextAccessor = ContextAccessor,
                TenantProvider = _TenantProvider
            };

            return Ok(new ApiOkResponse(
                await _iCoporateProductCatalogRepository.GetCorporateProductCatalogById(cpcParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }


    //[Microsoft.AspNetCore.Mvc.HttpGet("ReadCpcDetails/{id}")]
    [HttpGet("ReadCpcDetails/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetCpcDetailsById(string id)
    {
        try
        {
            //_logger.LogTrace("Started");
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var cpcParameters = new CpcParameters
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                Context = UPrinceCustomerContext,
                Id = id,
                ContextAccessor = ContextAccessor,
                TenantProvider = _TenantProvider
            };
            return Ok(new ApiOkResponse(
                await _iCoporateProductCatalogRepository.GetCpcDetailsById(cpcParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("CpcFilter")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CpcFilter([FromBody] CpcFilter filter)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var cpcParameters = new CpcParameters
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                Context = UPrinceCustomerContext,
                TenantProvider = _TenantProvider,
                filter = filter
            };
            var cpc = await _iCoporateProductCatalogRepository.GetCorporateProductCatalog(cpcParameters);
            var mApiOkResponse = new ApiOkResponse(cpc)
            {
                Message = ErrorMessageKey.Ok.ToString(),
                Result = cpc,
                Status = true
            };


            if (!cpc.Any())
            {
                var mApiResponse = new ApiOkResponse(null, "noAvailableCorporateProductCatalog")
                {
                    Status = false
                };
                return Ok(mApiResponse);
            }

            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpGet("ReadCpcShortcutPaneData")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ReadCpcShortcutPaneData()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var cpcParameters = new CpcParameters
            {
                Lang = lang,
                Context = UPrinceCustomerContext,
                TenantProvider = _TenantProvider
            };
            return Ok(new ApiOkResponse(
                await _iCoporateProductCatalogRepository.GetShortcutPaneData(cpcParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpDelete("DeleteCorporateProductCatalog")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteCpc([FromBody] List<string> idList)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var cpcParameters = new CpcParameters
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                Context = UPrinceCustomerContext,
                IdList = idList,
                TenantProvider = _TenantProvider
            };
            await _iCoporateProductCatalogRepository.DeleteCpc(cpcParameters);
            ApiOkResponse.Message = "Ok";
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpGet("GetCpcToExport")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetCpcToExport()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var cpcParameters = new CpcParameters
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                Context = UPrinceCustomerContext,
                ContextAccessor = ContextAccessor,
                TenantProvider = _TenantProvider
            };

            return Ok(new ApiOkResponse(await _iCoporateProductCatalogRepository.GetCpcToExport(cpcParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpGet("ReadResourceType")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetResourceType()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var cpcParameters = new CpcParameters
            {
                Lang = lang,
                Context = UPrinceCustomerContext,
                ContextAccessor = ContextAccessor,
                TenantProvider = _TenantProvider
            };
            return Ok(new ApiOkResponse(
                await _iCoporateProductCatalogRepository.getCpcResouceType(cpcParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    //[Microsoft.AspNetCore.Mvc.HttpGet("ReadCpcForBor/{id}")]
    [HttpGet("ReadCpcForBor/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetCpcForBor(string id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var cpcParameters = new CpcParameters
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                Context = UPrinceCustomerContext,
                Id = id,
                ContextAccessor = ContextAccessor,
                TenantProvider = _TenantProvider
            };

            return Ok(new ApiOkResponse(await _iCoporateProductCatalogRepository.GetCpcForBorById(cpcParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpGet("ReadVehiclesForQr/{title}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ReadVehiclesForQr(string title)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var cpcParameters = new CpcParameters
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                Context = UPrinceCustomerContext,
                ContextAccessor = ContextAccessor,
                TenantProvider = _TenantProvider,
                Title = title
            };

            return Ok(new ApiOkResponse(
                await _iCoporateProductCatalogRepository.ReadVehiclesForQr(cpcParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("CpcLobourFilterMyEnv")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CpcLobourFilterMyEnv([FromBody] CpcLobourFilterMyEnvDto cpcLobourFilterMyEnvDto)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var cpcParameters = new CpcParameters
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                Context = UPrinceCustomerContext,
                ContextAccessor = ContextAccessor,
                TenantProvider = _TenantProvider,
                CpcLobourFilterMyEnvDto = cpcLobourFilterMyEnvDto
            };

            return Ok(new ApiOkResponse(
                await _iCoporateProductCatalogRepository.CpcLobourFilterMyEnv(cpcParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }
}