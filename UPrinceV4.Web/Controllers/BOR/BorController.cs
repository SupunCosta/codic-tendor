using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.BOR;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces.BOR;
using UPrinceV4.Web.Repositories.Interfaces.CPC;
using UPrinceV4.Web.Repositories.Interfaces.PBS;

namespace UPrinceV4.Web.Controllers.BOR;

//[Authorize]
[Authorize]
[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class BorController : CommonConfigurationController
{
    private readonly IBorRepository _iBorRepository;
    private readonly IBorResourceRepository _iBorResourceRepository;
    private readonly ICoporateProductCatalogRepository _iCoporateProductCatalogRepository;
    private readonly IPbsRepository _iPbsRepository;
    private readonly ITenantProvider _TenantProvider;
    private readonly IPbsResourceRepository _iPbsResourceRepository;
    
    public BorController(ITenantProvider tenantProvider, IBorRepository iBorRepository,
        IPbsRepository iPbsRepository,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse,
         ITenantProvider iTenantProvider,
        IBorResourceRepository iBorResourceRepository,
        ICoporateProductCatalogRepository _CoporateProductCatalogRepository,IPbsResourceRepository iPbsResourceRepository)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)

    {
        _iBorRepository = iBorRepository;
        _TenantProvider = tenantProvider;
        _iPbsRepository = iPbsRepository;
        _iBorResourceRepository = iBorResourceRepository;
        _iCoporateProductCatalogRepository = _CoporateProductCatalogRepository;
        _iCoporateProductCatalogRepository = _CoporateProductCatalogRepository;
        _iPbsResourceRepository = iPbsResourceRepository;
    }

    //[HttpPost("Create")]
    [HttpPost("Create")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateBor([FromBody] BorDto borDto)
    {
        try
        {
            //_logger.LogTrace("Started");
            //_logger.LogError("Create" + JsonToStringConverter.getStringFromJson(borDto));
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var oid = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var userId = UPrinceCustomerContext.ApplicationUser.First(u => u.OId == oid).OId;
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _borParameter = new BorParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = _TenantProvider,
                BorDto = borDto,
                IBorResourceRepository = _iBorResourceRepository,
                ICoporateProductCatalogRepository = _iCoporateProductCatalogRepository
            };
            _borParameter.CpcParameters = new CpcParameters()
            {
                Oid = userId
            };
            var borItemId = await _iBorRepository.CreateBor(_borParameter);
            return Ok(new ApiOkResponse(borItemId));
        }
        catch (Exception ex)
        {
            var mApiResponse = new ApiResponse(400, false,
                "" + ContextAccessor.HttpContext.Request.Path + " " + ex.Message);

            mApiResponse.StatusCode = 400;
            mApiResponse.Status = false;
            //mApiResponse.Message = ex.StackTrace;
            return BadRequest(mApiResponse);
        }
    }

    [HttpGet("ReadShortcutPaneData")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ReadBorShortcutPaneData(CancellationToken cancellationToken = default)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _borParameter = new BorParameter
            {
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = _TenantProvider
            };

            var shortcutPaneData = await _iBorRepository.GetShortcutPaneData(_borParameter);

            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            var mApiResponse = new ApiResponse(400, false,
                "" + ContextAccessor.HttpContext.Request.Path + " " + ex.Message);

            mApiResponse.StatusCode = 400;
            mApiResponse.Status = false;
            // mApiResponse.Message = ex.StackTrace;
            return BadRequest(mApiResponse);
        }
    }

    [HttpGet("ReadProductById/{productId}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetProductById(string productId)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _pbsParameters = new PbsParameters
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = _TenantProvider,
                Id = productId,
                IPbsResourceRepository = _iPbsResourceRepository
            };

            var product = await _iPbsRepository.GetPbsById(_pbsParameters);

            return Ok(new ApiOkResponse(product));
        }
        catch (Exception ex)
        {
            var mApiResponse = new ApiResponse(400, false,
                "" + ContextAccessor.HttpContext.Request.Path + " " + ex.Message);

            mApiResponse.StatusCode = 400;
            mApiResponse.Status = false;
            //mApiResponse.Message = ex.StackTrace;
            return BadRequest(mApiResponse);
        }
    }


    [HttpPost("FilterBor")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetBorList([FromBody] BorFilter filter)
    {
        try
        {
            if (ContextAccessor.HttpContext.Request.Path.ToUriComponent().Contains("FilterBor"))
            {
                var lang = langCode(Request.Headers["lang"].FirstOrDefault());
                var _borParameter = new BorParameter
                {
                    ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                    ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                    Lang = lang,
                    BorFilter = filter,
                    ContextAccessor = ContextAccessor,
                    TenantProvider = _TenantProvider
                };

                var result = await _iBorRepository.GetBorList(_borParameter);
                var mApiOkResponse = new ApiOkResponse(result);


                if (!result.Any())
                {
                    var mApiResponse = new ApiOkResponse(null, "noAvailableBOR")
                    {
                        Status = false
                    };
                    return Ok(mApiResponse);
                }

                mApiOkResponse.StatusCode = 200;
                mApiOkResponse.Status = true;
                return Ok(mApiOkResponse);
            }

            {
                var mApiResponse =
                    new ApiResponse(400, false, "" + ContextAccessor.HttpContext.Request.Path);

                return BadRequest(ApiResponse);
            }
        }
        catch (Exception ex)
        {
            var mApiResponse = new ApiResponse(400, false,
                "" + ContextAccessor.HttpContext.Request.Path + " " + ex.Message);
            return BadRequest(mApiResponse);
        }
    }


    [HttpPost("FilterBorPo")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> FilterBorPo([FromBody] BorFilter filter)
    {
        try
        {
            if (ContextAccessor.HttpContext.Request.Path.ToUriComponent().Contains("FilterBor"))
            {
                var lang = langCode(Request.Headers["lang"].FirstOrDefault());
                var _borParameter = new BorParameter
                {
                    ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                    ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                    Lang = lang,
                    BorFilter = filter,
                    ContextAccessor = ContextAccessor,
                    TenantProvider = _TenantProvider
                };

                var result = await _iBorRepository.FilterBorPo(_borParameter);
                var mApiOkResponse = new ApiOkResponse(result);


                if (!result.Any())
                {
                    var mApiResponse = new ApiOkResponse(null, "noAvailableBOR")
                    {
                        Status = false
                    };
                    return Ok(mApiResponse);
                }

                mApiOkResponse.StatusCode = 200;
                mApiOkResponse.Status = true;
                return Ok(mApiOkResponse);
            }

            {
                var mApiResponse =
                    new ApiResponse(400, false, "" + ContextAccessor.HttpContext.Request.Path);

                return BadRequest(ApiResponse);
            }
        }
        catch (Exception ex)
        {
            var mApiResponse = new ApiResponse(400, false,
                "" + ContextAccessor.HttpContext.Request.Path + " " + ex.Message);
            return BadRequest(mApiResponse);
        }
    }

   
    [HttpPost("FilterBorResource")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetBorResourceList([FromBody] BorResourceFilter filter)
    {
        try
        {
            // _logger.LogTrace("Started");
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _borParameter = new BorParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                BorResourceFilter = filter,
                ContextAccessor = ContextAccessor,
                TenantProvider = _TenantProvider
            };

            var result = await _iBorRepository.GetBorResourceList(_borParameter);
            return Ok(!result.Any() ? new ApiResponse(200, false, "noBorAvailable") : new ApiOkResponse(result, "ok"));
        }
        catch (Exception ex)
        {
            var mApiResponse = new ApiResponse(400, false,
                "" + ContextAccessor.HttpContext.Request.Path + " " + ex.Message)
            {
                StatusCode = 400,
                Status = false
            };

            //mApiResponse.Message = ex.StackTrace;
            return BadRequest(mApiResponse);
        }
    }


   
    [HttpPost("FilterProduct")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> FilterProduct([FromBody] BorProductFilter filter)
    {
        try
        {
            if (ContextAccessor.HttpContext.Request.Path.ToUriComponent().Contains("FilterProduct"))
            {
                //_logger.LogTrace("Started");
                var lang = langCode(Request.Headers["lang"].FirstOrDefault());
                var _borParameter = new BorParameter
                {
                    ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                    ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                    //string message = ApiErrorMessages.GetErrorMessage(_TenantProvider, ErrorMessageKey.Ok, lang).Message;
                    Lang = lang,
                    ContextAccessor = ContextAccessor,
                    TenantProvider = _TenantProvider,
                    Title = filter.Title
                };

                var dropDownData = await _iBorRepository.GetProduct(_borParameter);
                var mApiOkResponse = new ApiOkResponse(dropDownData)
                {
                    Status = true,
                    Result = dropDownData,
                    Message = ContextAccessor.HttpContext.Request.Path
                };
                return Ok(mApiOkResponse);
            }

            var mApiResponse =
                new ApiResponse(400, false, "" + ContextAccessor.HttpContext.Request.Path)
                {
                    StatusCode = 400,
                    Status = false
                };

            //mApiResponse.Message = ContextAccessor.HttpContext.Request.Path;
            return BadRequest(mApiResponse);
        }
        catch (Exception ex)
        {
            var mApiResponse = new ApiResponse(400, false,
                "" + ContextAccessor.HttpContext.Request.Path + " " + ex.Message)
            {
                StatusCode = 400,
                Status = false
            };
            // mApiResponse.Message = ex.StackTrace;
            return BadRequest(mApiResponse);
        }
    }


    [HttpPost("GetBorListByProduct")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetBorListByProduct([FromBody] BorFilter filter)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _borParameter = new BorParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                BorFilter = filter,
                ContextAccessor = ContextAccessor,
                TenantProvider = _TenantProvider
            };

            var result = await _iBorRepository.GetBorListByProduct(_borParameter);
            var mApiOkResponse = new ApiOkResponse(result)
            {
                Result = result,
                Message = "ok"
            };


            if (!result.Any())
            {
                mApiOkResponse.Status = false;
                //string message = ApiErrorMessages.GetErrorMessage(_TenantProvider, ErrorMessageKey.NoBorAvailable, lang).Message;
                mApiOkResponse.Message = "noBorAvailable";
                return Ok(mApiOkResponse);
            }

            mApiOkResponse.StatusCode = 200;
            mApiOkResponse.Status = true;
            mApiOkResponse.Message = "Ok";
            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            var mApiResponse = new ApiResponse(400, false, ex.Message);
            return BadRequest(mApiResponse);
        }
    }


    [HttpGet("ReadBor/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetBorById(string id)
    {
        try
        {
            //_logger.LogTrace("Started");
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _borParameter = new BorParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                // string message = ApiErrorMessages.GetErrorMessage(_TenantProvider, ErrorMessageKey.Ok, lang).Message;
                Lang = lang,
                Id = id,
                ContextAccessor = ContextAccessor,
                TenantProvider = _TenantProvider
            };

            var cpc = await _iBorRepository.GetBorById(_borParameter);
            var mApiOkResponse = new ApiOkResponse(cpc);
            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            var mApiResponse = new ApiResponse(400, false,
                "" + ContextAccessor.HttpContext.Request.Path + " " + ex.Message);
            return BadRequest(mApiResponse);
        }
    }

    [HttpGet("GetBorDropdownData")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetBorDropdownData()
    {
        try
        {
            //_logger.LogTrace("Started");
            //_logger.LogError("ReadDropdownData");
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _borParameter = new BorParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                TenantProvider = ItenantProvider
            };

            return Ok(new ApiOkResponse(await _iBorRepository.GetBorDropdownData(_borParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPut("UpdateBorStatus")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateBorStatus([FromBody] BorStatusUpdateDto dto)
    {
        try
        {
            //_logger.LogTrace("Started");
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _borParameter = new BorParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault()
            };

            var message = "ok";
            _borParameter.Lang = lang;
            _borParameter.ContextAccessor = ContextAccessor;
            _borParameter.TenantProvider = _TenantProvider;
            _borParameter.borStatusUpdateDto = dto;
            var dropDownData = await _iBorRepository.UpdateBorStatus(_borParameter);
            return Ok(new ApiOkResponse(dropDownData, message));
        }
        catch (Exception ex)
        {
            var mApiResponse = new ApiResponse(400, false,
                "" + ContextAccessor.HttpContext.Request.Path + " " + ex.Message);
            mApiResponse.StatusCode = 400;
            mApiResponse.Status = false;
            mApiResponse.Message = ex.StackTrace;
            return BadRequest(mApiResponse);
        }
    }

    [HttpPost("BorResourcesbyIds")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> BorResourcesbyIds([FromBody] List<string> idList)
    {
        try
        {
            // _logger.LogTrace("Started");
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _borParameter = new BorParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                idList = idList,
                ContextAccessor = ContextAccessor,
                TenantProvider = _TenantProvider
            };

            var result = await _iBorRepository.GetBorResourcesbyIds(_borParameter);
            return Ok(result == null ? new ApiResponse(200, false, "noBorAvailable") : new ApiOkResponse(result));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, true, ex.Message));
        }
    }


    [HttpPost("FilterReturnBorPo")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> FilterReturnBorPo([FromBody] BorFilter filter)
    {
        try
        {
            if (ContextAccessor.HttpContext.Request.Path.ToUriComponent().Contains("FilterReturnBorPo"))
            {
                var lang = langCode(Request.Headers["lang"].FirstOrDefault());
                var _borParameter = new BorParameter
                {
                    ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                    ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                    Lang = lang,
                    BorFilter = filter,
                    ContextAccessor = ContextAccessor,
                    TenantProvider = _TenantProvider
                };

                var result = await _iBorRepository.FilterReturnBorPo(_borParameter);
                var mApiOkResponse = new ApiOkResponse(result);


                if (!result.Any())
                {
                    var mApiResponse = new ApiOkResponse(null, "noAvailableBOR")
                    {
                        Status = false
                    };
                    return Ok(mApiResponse);
                }

                mApiOkResponse.StatusCode = 200;
                mApiOkResponse.Status = true;
                return Ok(mApiOkResponse);
            }

            {
                var mApiResponse =
                    new ApiResponse(400, false, "" + ContextAccessor.HttpContext.Request.Path);

                return BadRequest(mApiResponse);
            }
        }
        catch (Exception ex)
        {
            var mApiResponse = new ApiResponse(400, false,
                "" + ContextAccessor.HttpContext.Request.Path + " " + ex.Message);
            return BadRequest(mApiResponse);
        }
    }

    [HttpPost("UploadPoDocuments")]
    public async Task<ActionResult<string>> UploadPoDocuments([FromForm] IFormCollection image,
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
            var _borParameter = new BorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang
            };

            var client = new FileClient();
            var url = client.PersistPhotoInNewFolder(image.Files.FirstOrDefault()?.FileName, _TenantProvider
                , image.Files.FirstOrDefault(), "BOR Documents");

            var response = new ApiOkResponse(url);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }
}