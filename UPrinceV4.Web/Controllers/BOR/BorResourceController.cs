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
using UPrinceV4.Web.Data.BOR;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces.BOR;
using UPrinceV4.Web.Repositories.Interfaces.PBS;

namespace UPrinceV4.Web.Controllers.BOR;

[Authorize]
[Route("api/[controller]")]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
[ApiController]
public class BorResourceController : CommonConfigurationController
{
    private readonly IBorResourceRepository _iBorResourceRepository;
    private readonly IPbsResourceRepository _IPbsResourceRepository;

    public BorResourceController(IPbsResourceRepository iPbsResourceRepository,
        IBorResourceRepository iBorResourceRepository,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse,
        ITenantProvider iTenantProvider)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)

    {
        _IPbsResourceRepository = iPbsResourceRepository;
        _iBorResourceRepository = iBorResourceRepository;
    }

    [HttpGet("ReadMaterialByPbsProduct/{pbsProductId}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetMaterialByPbsProduct(string pbsProductId)
    {
        try
        {
            //_logger.LogTrace("Started");
            // _logger.LogError("ReadMaterial");
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _PbsResourceParameters = new PbsResourceParameters
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                TenantProvider = ItenantProvider,
                PbsProductId = pbsProductId
            };

            return Ok(new ApiOkResponse(
                await _IPbsResourceRepository.GetMaterialByProductId(_PbsResourceParameters)));
        }
        catch (Exception ex)
        {
            var mApiResponse = new ApiResponse(400, false, "" + ContextAccessor.HttpContext.Request.Path);

            mApiResponse.StatusCode = 400;
            mApiResponse.Status = false;
            mApiResponse.Message = ex.StackTrace;
            return BadRequest(mApiResponse);
        }
    }

    [HttpGet("ReadToolByPbsProduct/{pbsProductId}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetToolByPbsProduct(string pbsProductId)
    {
        try
        {
            //_logger.LogTrace("Started");
            //_logger.LogError("ReadMaterial");
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            //string message = ApiErrorMessages.GetErrorMessage(ItenantProvider, ErrorMessageKey.Ok, lang).Message;

            var _PbsResourceParameters = new PbsResourceParameters
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                TenantProvider = ItenantProvider,
                PbsProductId = pbsProductId
            };

            var dropDownData = await _IPbsResourceRepository.GetToolByProductId(_PbsResourceParameters);
            var mApiOkResponse = new ApiOkResponse(dropDownData);
            mApiOkResponse.Result = dropDownData;
            mApiOkResponse.Status = true;

            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpGet("ReadLabourByPbsProduct/{pbsProductId}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetLabourByPbsProduct(string pbsProductId)
    {
        try
        {
            //_logger.LogTrace("Started");
            //_logger.LogError("ReadMaterial");
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            
            var _PbsResourceParameters = new PbsResourceParameters
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                TenantProvider = ItenantProvider,
                PbsProductId = pbsProductId
            };

            var dropDownData = await _IPbsResourceRepository.GetLabourByProductId(_PbsResourceParameters);
            var mApiOkResponse = new ApiOkResponse(dropDownData);
            mApiOkResponse.Result = dropDownData;
            mApiOkResponse.Status = true;
            // ApiOkResponse.Result = dropDownData;
            //ApiOkResponse.Message = message;
            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpGet("ReadConsumableByPbsProduct/{pbsProductId}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetConsumableByPbsProduct(string pbsProductId)
    {
        try
        {
            // _logger.LogTrace("Started");
            // _logger.LogError("ReadMaterial");
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            //string message = ApiErrorMessages.GetErrorMessage(ItenantProvider, ErrorMessageKey.Ok, lang).Message;
            
            var _PbsResourceParameters = new PbsResourceParameters
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                TenantProvider = ItenantProvider,
                PbsProductId = pbsProductId
            };

            var dropDownData = await _IPbsResourceRepository.GetConsumableByProductId(_PbsResourceParameters);
            var mApiOkResponse = new ApiOkResponse(dropDownData);
            mApiOkResponse.Result = dropDownData;
            mApiOkResponse.Status = true;
            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    //[HttpPost("CreateBorLabour")]
    [HttpPost("CreateBorLabour")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateBorLabour([FromBody] BorResource borDto)
    {
        try
        {
            //_logger.LogTrace("Started");
            // _logger.LogError("Create" + JsonToStringConverter.getStringFromJson(borDto));
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _borParameter = new BorParameter
            {
                Lang = lang,
                BorResourceCreateDto = borDto,
                TenantProvider = ItenantProvider
            };
            var id = await _iBorResourceRepository.CreateBorLabour(_borParameter, false);
            var mApiOkResponse = new ApiOkResponse(id);
            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    //[HttpPost("CreateBorMaterial")]
    [HttpPost("CreateBorMaterial")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateBorMaterial([FromBody] BorResource borDto)
    {
        try
        {
            // _logger.LogTrace("Started");
            // _logger.LogError("CreateBorMaterial" + JsonToStringConverter.getStringFromJson(borDto));
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _borParameter = new BorParameter
            {
                Lang = lang,
                BorResourceCreateDto = borDto,
                TenantProvider = ItenantProvider
            };
            var id = await _iBorResourceRepository.CreateBorMaterial(_borParameter, false);
            var mApiOkResponse = new ApiOkResponse(id);
            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    //[HttpPost("CreateBorConsumable")]
    [HttpPost("CreateBorConsumable")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateBorConsumable([FromBody] BorResource borDto)
    {
        try
        {
            // _logger.LogTrace("Started");
            // _logger.LogError("CreateBorConsumable" + JsonToStringConverter.getStringFromJson(borDto));
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _borParameter = new BorParameter
            {
                Lang = lang,
                BorResourceCreateDto = borDto,
                TenantProvider = ItenantProvider
            };

            var id = await _iBorResourceRepository.CreateBorConsumable(_borParameter, false);
            var mApiOkResponse = new ApiOkResponse(id);
            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    //[HttpPost("CreateBorTools")]
    [HttpPost("CreateBorTools")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateBorTools([FromBody] BorResource borDto)
    {
        try
        {
            // _logger.LogTrace("Started");
            // _logger.LogError("CreateBorTools" + JsonToStringConverter.getStringFromJson(borDto));
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _borParameter = new BorParameter
            {
                Lang = lang,
                BorResourceCreateDto = borDto,
                TenantProvider = ItenantProvider
            };

            var id = await _iBorResourceRepository.CreateBorTools(_borParameter, false);
            var mApiOkResponse = new ApiOkResponse(id);
            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }


    [HttpPost("UpdateBorMaterial")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateBorMaterial([FromBody] BorResourceUpdate borResourceUpdate)
    {
        try
        {
            // _logger.LogTrace("Started");
            // _logger.LogError("CreateBorTools" + JsonToStringConverter.getStringFromJson(borDto));
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var mBorParameterResoruce = new BorParameterResoruce
            {
                Lang = lang,
                borResourceUpdate = borResourceUpdate,
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                TenantProvider = ItenantProvider
            };
            var id = await _iBorResourceRepository.UpdateBorMaterial(mBorParameterResoruce);
            // ApiOkResponse mApiOkResponse = new ApiOkResponse(id);
            return Ok(new ApiOkResponse(id));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("UpdateBorConsumable")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateBorConsumable([FromBody] BorResourceUpdate borResourceUpdate)
    {
        try
        {
            // _logger.LogTrace("Started");
            // _logger.LogError("CreateBorTools" + JsonToStringConverter.getStringFromJson(borDto));
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var mBorParameterResoruce = new BorParameterResoruce
            {
                Lang = lang,
                borResourceUpdate = borResourceUpdate,
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                TenantProvider = ItenantProvider
            };
            var id = await _iBorResourceRepository.UpdateBorConsumable(mBorParameterResoruce);
            // ApiOkResponse mApiOkResponse = new ApiOkResponse(id);
            return Ok(new ApiOkResponse(id));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("UpdateBorTools")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateBorTools([FromBody] BorResourceUpdate borResourceUpdate)
    {
        try
        {
            // _logger.LogTrace("Started");
            // _logger.LogError("CreateBorTools" + JsonToStringConverter.getStringFromJson(borDto));
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var mBorParameterResoruce = new BorParameterResoruce
            {
                Lang = lang,
                borResourceUpdate = borResourceUpdate,
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                TenantProvider = ItenantProvider
            };
            var id = await _iBorResourceRepository.UpdateBorTools(mBorParameterResoruce);
            // ApiOkResponse mApiOkResponse = new ApiOkResponse(id);
            return Ok(new ApiOkResponse(id));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("UpdateBorLabour")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateBorLabour([FromBody] BorResourceUpdate borResourceUpdate)
    {
        try
        {
            // _logger.LogTrace("Started");
            // _logger.LogError("CreateBorTools" + JsonToStringConverter.getStringFromJson(borDto));
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var mBorParameterResoruce = new BorParameterResoruce
            {
                Lang = lang,
                borResourceUpdate = borResourceUpdate,
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                TenantProvider = ItenantProvider
            };
            var id = await _iBorResourceRepository.UpdateBorLabour(mBorParameterResoruce);
            // ApiOkResponse mApiOkResponse = new ApiOkResponse(id);
            return Ok(new ApiOkResponse(id));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpDelete("DeleteBorMaterial")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteBorMaterial([FromBody] List<string> idList)
    {
        try
        {
            // _logger.LogTrace("Started");
            // _logger.LogError("CreateBorTools" + JsonToStringConverter.getStringFromJson(borDto));
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var mBorParameterResoruce = new BorParameterResoruceDelete
            {
                Lang = lang,
                idList = idList,
                TenantProvider = ItenantProvider,
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault()
            };
            var id = await _iBorResourceRepository.DeleteBorMaterial(mBorParameterResoruce);
            // ApiOkResponse mApiOkResponse = new ApiOkResponse(id);
            return Ok(new ApiOkResponse(id));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpDelete("DeleteBorLabour")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteBorLabour([FromBody] List<string> idList)
    {
        try
        {
            // _logger.LogTrace("Started");
            // _logger.LogError("CreateBorTools" + JsonToStringConverter.getStringFromJson(borDto));
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var mBorParameterResoruce = new BorParameterResoruceDelete
            {
                Lang = lang,
                idList = idList,
                TenantProvider = ItenantProvider,
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault()
            };
            var id = await _iBorResourceRepository.DeleteBorLabour(mBorParameterResoruce);
            // ApiOkResponse mApiOkResponse = new ApiOkResponse(id);
            return Ok(new ApiOkResponse(id));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpDelete("DeleteBorConsumable")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteBorConsumable([FromBody] List<string> idList)
    {
        try
        {
            // _logger.LogTrace("Started");
            // _logger.LogError("CreateBorTools" + JsonToStringConverter.getStringFromJson(borDto));
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var mBorParameterResoruce = new BorParameterResoruceDelete
            {
                Lang = lang,
                idList = idList,
                TenantProvider = ItenantProvider,
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault()
            };
            var id = await _iBorResourceRepository.DeleteBorConsumable(mBorParameterResoruce);
            // ApiOkResponse mApiOkResponse = new ApiOkResponse(id);
            return Ok(new ApiOkResponse(id));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpDelete("DeleteBorTools")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteBorTools([FromBody] List<string> idList)
    {
        try
        {
            // _logger.LogTrace("Started");
            // _logger.LogError("CreateBorTools" + JsonToStringConverter.getStringFromJson(borDto));
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var mBorParameterResoruce = new BorParameterResoruceDelete
            {
                Lang = lang,
                idList = idList,
                TenantProvider = ItenantProvider,
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault()
            };
            var id = await _iBorResourceRepository.DeleteBorTools(mBorParameterResoruce);
            // ApiOkResponse mApiOkResponse = new ApiOkResponse(id);
            return Ok(new ApiOkResponse(id));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }
}