using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Newtonsoft.Json;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.Comment;
using UPrinceV4.Web.Data.Contractor;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Data.PdfToExcel;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Repositories.Interfaces.BM;

namespace UPrinceV4.Web.Controllers.Contractor;

[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class ContractorController : CommonConfigurationController
{
    private readonly GraphServiceClient _graphServiceClient;
    private readonly IContractorReopsitory _iContractorReopsitory;
    private readonly IGraphRepository _iGraphRepository;
    private readonly IPdfToExcelRepository _iPdfToExcelRepository;
    private readonly ISendGridRepositorie _iSendGridRepositorie;
    private readonly ITenantProvider _TenantProvider;


    public ContractorController(ITenantProvider tenantProvider,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider,
        IGraphRepository iGraphRepository, ISendGridRepositorie iSendGridRepositorie
        , IContractorReopsitory iContractorReopsitory, GraphServiceClient graphServiceClient,
        IPdfToExcelRepository iPdfToExcelRepository, IConfiguration iConfiguration)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)

    {
        _iContractorReopsitory = iContractorReopsitory;
        _TenantProvider = iTenantProvider;
        _graphServiceClient = graphServiceClient;
        _iGraphRepository = iGraphRepository;
        _iGraphRepository = iGraphRepository;
        _iSendGridRepositorie = iSendGridRepositorie;
        _iPdfToExcelRepository = iPdfToExcelRepository;
        _iConfiguration = iConfiguration;
    }

    private IConfiguration _iConfiguration { get; }

    [Authorize]
    [HttpGet("GetContractorDropdown")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetContractorDropdown([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                Configuration = _iConfiguration,
                TenantProvider = ItenantProvider
            };
            return Ok(new ApiOkResponse(
                await _iContractorReopsitory.GetContractorDropDownData(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpPost("CreateHeader")]
    public async Task<ActionResult> CreateHeader([FromBody] ContractorHeaderDto BMLotHeaderDto,
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
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                BMLotHeaderDto = BMLotHeaderDto,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                GraphServiceClient = _graphServiceClient,
                Configuration = _iConfiguration,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };
            var s = await _iContractorReopsitory.CreateHeader(_contractorParameter, _iGraphRepository,
                _iSendGridRepositorie);
            return Ok(new ApiOkResponse(s));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpPost("ContractorFilter")]
    public async Task<ActionResult> ContractorFilter([FromBody] ContractorFilterDto LotFilterDto,
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
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                Filter = LotFilterDto,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                Configuration = _iConfiguration,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };
            var result = await _iContractorReopsitory.ContractorFilter(_contractorParameter);
            if (!result.Any())
            {
                var mApiResponse = new ApiOkResponse(null, "noAvailableLot")
                {
                    Status = false
                };
                return Ok(mApiResponse);
            }
            return Ok(new ApiOkResponse(result));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpPost("ContractorFilterGetLots")]
    public async Task<ActionResult> ContractorFilterGetLots([FromBody] ContractorFilterDto LotFilterDto,
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
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                Filter = LotFilterDto,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                Configuration = _iConfiguration,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };
            var result = await _iContractorReopsitory.ContractorFilterGetLots(_contractorParameter);
            if (!result.Any())
            {
                var mApiResponse = new ApiOkResponse(null, "noAvailableLot")
                {
                    Status = false
                };
                return Ok(mApiResponse);
            }
            return Ok(new ApiOkResponse(result));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpGet("GetContractorById/{SequenceId}")]
    public async Task<ActionResult> GetContractorById(string SequenceId,
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
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                Id = SequenceId,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                _pdfToExcelRepository = _iPdfToExcelRepository,
                Configuration = _iConfiguration,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };
            var s = await _iContractorReopsitory.GetContractorById(_contractorParameter);
            return Ok(new ApiOkResponse(s));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpPost("UpdateContractorWorkflow")]
    public async Task<ActionResult> UpdateContractorWorkflow([FromBody] ConstructorLotInfoDto ConstructorLotInfoDto,
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
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ConstructorLotInfoDto = ConstructorLotInfoDto,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                Configuration = _iConfiguration,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };
            var s = await _iContractorReopsitory.UpdateContractorWorkflow(_contractorParameter);
            return Ok(new ApiOkResponse(s));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpGet("ShortcutPaneData")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ShortcutPaneData([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                Configuration = _iConfiguration,
                TenantProvider = ItenantProvider
            };
            return Ok(new ApiOkResponse(await _iContractorReopsitory.ShortcutPaneData(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpGet("GetConstructorById/{SequenceId}")]
    public async Task<ActionResult> GetConstructorById(string SequenceId,
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
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                Id = SequenceId,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                Configuration = _iConfiguration,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };
            var s = await _iContractorReopsitory.GetConstructorById(_contractorParameter);
            return Ok(new ApiOkResponse(s));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpPost("GetConstructorByTaxonomy")]
    public async Task<ActionResult> GetConstructorByTaxonomy([FromBody] List<string> TaxonomyId)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }
            var _contractorParameter = new ContractorParameter
            {
                IdList = TaxonomyId,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                Configuration = _iConfiguration
            };
            var s = await _iContractorReopsitory.GetConstructorByTaxonomy(_contractorParameter);
            return Ok(new ApiOkResponse(s));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpPost("ExcelUpload")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ExcelUpload([FromBody] CBCExcelLotDataDto uploadExcelDto)
    {
        try
        {
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault()
            };
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            _contractorParameter.Lang = lang;
            _contractorParameter.UploadExcelDto = uploadExcelDto;
            _contractorParameter.ContextAccessor = ContextAccessor;
            _contractorParameter.TenantProvider = ItenantProvider;
            _contractorParameter.GraphServiceClient = _graphServiceClient;
            _contractorParameter.Configuration = _iConfiguration;
            return Ok(new ApiOkResponse(await _iContractorReopsitory.ExcelUpload(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpPost("ExcelUploadTest")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ExcelUploadTest([FromForm] string uploadExcelDto, [FromForm] IFormCollection pdf)
    {
        try
        {
            var myObj = JsonConvert.DeserializeObject<CBCExcelLotDataDto>(uploadExcelDto);
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault()
            };
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }
            var file = pdf.Files.FirstOrDefault();
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            _contractorParameter.Lang = lang;
            _contractorParameter.UploadExcelDto = myObj;
            _contractorParameter.ContextAccessor = ContextAccessor;
            _contractorParameter.TenantProvider = ItenantProvider;
            _contractorParameter.File = file;
            _contractorParameter.GraphServiceClient = _graphServiceClient;
            _contractorParameter.Configuration = _iConfiguration;
            return Ok(new ApiOkResponse(await _iContractorReopsitory.ExcelUploadTest(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpPost("ContractorPsUpload")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ContractorPsUpload([FromForm] string uploadExcelDto, [FromForm] IFormCollection pdf)
    {
        try
        {
            var myObj = JsonConvert.DeserializeObject<UploadContractorPs>(uploadExcelDto);
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault()
            };
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var user = UPrinceCustomerContext.ApplicationUser?.Where(u => u.OId == objectIdentifier)
                .FirstOrDefault();
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }
            var file = pdf.Files.FirstOrDefault();
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            _contractorParameter.UserId = objectIdentifier;
            _contractorParameter.Lang = lang;
            _contractorParameter.ContractorPsUploadDto = myObj;
            _contractorParameter.ContextAccessor = ContextAccessor;
            _contractorParameter.TenantProvider = ItenantProvider;
            _contractorParameter.File = file;
            _contractorParameter.GraphServiceClient = _graphServiceClient;
            _contractorParameter.Configuration = _iConfiguration;
            return Ok(new ApiOkResponse(await _iContractorReopsitory.ContractorPsUpload(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpPost("PersonFilter")]
    public async Task<ActionResult> PersonFilter([FromBody] ConstructorTeamListFilter PersonFilter,
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
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                PersonFilter = PersonFilter,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                Configuration = _iConfiguration
            };
            var result = await _iContractorReopsitory.GetConstructorTeam(_contractorParameter);
            if (!result.Any())
            {
                var mApiResponse = new ApiOkResponse(null, "noAvailablePerson")
                {
                    Status = false
                };
                return Ok(mApiResponse);
            }
            return Ok(new ApiOkResponse(result));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpPost("LotPersonFilter")]
    public async Task<ActionResult> LotPersonFilter([FromBody] ConstructorTeamListFilter PersonFilter,
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
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                PersonFilter = PersonFilter,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                Configuration = _iConfiguration
            };
            var result = await _iContractorReopsitory.LotPersonFilter(_contractorParameter);
            if (!result.Any())
            {
                var mApiResponse = new ApiOkResponse(null, "noAvailablePerson")
                {
                    Status = false
                };
                return Ok(mApiResponse);
            }
            return Ok(new ApiOkResponse(result));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpGet("GetCbcExcelLotData/{conractId}")]
    public async Task<ActionResult> GetCbcExcelLotData(
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, string conractId)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                Id = conractId,
                Configuration = _iConfiguration
            };
            return Ok(new ApiOkResponse(await _iContractorReopsitory.GetCbcExcelLotData(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpGet("GetCbcExcelLotDataTest/{conractId}")]
    public async Task<ActionResult> GetCbcExcelLotDataTest(
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, string conractId)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                Id = conractId,
                Configuration = _iConfiguration
            };
            return Ok(new ApiOkResponse(await _iContractorReopsitory.GetCbcExcelLotDataTest(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpGet("GetCbcExcelLotDataTestForZeroState/{conractId}")]
    public async Task<ActionResult> GetCbcExcelLotDataTestForZeroState(
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, string conractId)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                Id = conractId,
                Configuration = _iConfiguration
            };
            return Ok(new ApiOkResponse(
                await _iContractorReopsitory.GetCbcExcelLotDataTestForZeroState(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpGet("GetCbcExcelLotDataFilterContractor/{conractId}")]
    public async Task<ActionResult> GetCbcExcelLotDataFilterContractor(
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, string conractId)
    {
        try
        {
            var _contractorParameter = new ContractorParameter
            {
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };
            var lang = langCode(langX);
            _contractorParameter.ContractingUnitSequenceId = CU;
            _contractorParameter.ProjectSequenceId = Project;
            _contractorParameter.Lang = lang;
            _contractorParameter.ContextAccessor = ContextAccessor;
            _contractorParameter.TenantProvider = ItenantProvider;
            _contractorParameter.Id = conractId;
            _contractorParameter.Configuration = _iConfiguration;
            return Ok(new ApiOkResponse(
                await _iContractorReopsitory.GetCbcExcelLotDataFilterContractor(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }


    [Authorize]
    [HttpGet("GetContractorsByLotId/{conractId}")]
    public async Task<ActionResult> GetContractorsByLotId(
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, string conractId)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                _pdfToExcelRepository = _iPdfToExcelRepository,
                Id = conractId,
                Configuration = _iConfiguration
            };
            return Ok(new ApiOkResponse(await _iContractorReopsitory.GetContractorsByLotId(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpGet("GetContractorsByLotIdForZeroState/{conractId}")]
    public async Task<ActionResult> GetContractorsByLotIdForZeroState(
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, string conractId)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                _pdfToExcelRepository = _iPdfToExcelRepository,
                Id = conractId,
                Configuration = _iConfiguration
            };
            return Ok(new ApiOkResponse(
                await _iContractorReopsitory.GetContractorsByLotIdForZeroState(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpGet("GetContractorsByLotIdFilterContractor/{conractId}")]
    public async Task<ActionResult> GetContractorsByLotIdFilterContractor(
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, string conractId)
    {
        try
        {
            var _contractorParameter = new ContractorParameter
            {
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };
            var lang = langCode(langX);
            _contractorParameter.ContractingUnitSequenceId = CU;
            _contractorParameter.ProjectSequenceId = Project;
            _contractorParameter.Lang = lang;
            _contractorParameter.ContextAccessor = ContextAccessor;
            _contractorParameter.TenantProvider = ItenantProvider;
            _contractorParameter._pdfToExcelRepository = _iPdfToExcelRepository;
            _contractorParameter.Id = conractId;
            _contractorParameter.Configuration = _iConfiguration;
            return Ok(new ApiOkResponse(
                await _iContractorReopsitory.GetContractorsByLotIdFilterContractor(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpPost("ConstructorWorkFlowDelete")]
    public async Task<ActionResult> ConstructorWorkFlowDelete(
        [FromBody] ConstructorWorkFlowDelete ConstructorWorkFlowDelete,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                ConstructorWorkFlowDelete = ConstructorWorkFlowDelete,
                TenantProvider = ItenantProvider,
                Configuration = _iConfiguration
            };
            var result = await _iContractorReopsitory.ConstructorWorkFlowDelete(_contractorParameter);
            return Ok(new ApiOkResponse(result));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpGet("GetContractorsErrorListByLotId/{conractId}")]
    public async Task<ActionResult> GetContractorsErrorListByLotId(
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, string conractId)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                Id = conractId,
                Configuration = _iConfiguration
            };
            return Ok(new ApiOkResponse(
                await _iContractorReopsitory.GetContractorsErrorListByLotId(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpPost("CrateCommentCard")]
    public async Task<ActionResult> CrateCommentCard([FromBody] CommentCard CommentCard,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                CommentCard = CommentCard,
                Configuration = _iConfiguration,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };
            return Ok(new ApiOkResponse(await _iContractorReopsitory.CrateCommentCard(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpPost("AddComment")]
    public async Task<ActionResult> AddComment([FromBody] ContractorComment ContractorComment,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                ContractorComment = ContractorComment,
                Configuration = _iConfiguration,
                SendGridRepositorie = _iSendGridRepositorie,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };
            return Ok(new ApiOkResponse(
                await _iContractorReopsitory.AddComment(_contractorParameter, _iSendGridRepositorie)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpPost("GetComment")]
    public async Task<ActionResult> GetComment([FromBody] CommentFilter CommentFilter,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                CommentFilter = CommentFilter,
                Configuration = _iConfiguration,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };
            return Ok(new ApiOkResponse(await _iContractorReopsitory.GetComment(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpPost("ContractorLotExcelUpload")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ContractorLotExcelUpload([FromForm] string uploadExcelDto,
        [FromForm] IFormCollection pdf)
    {
        try
        {
            var myObj = JsonConvert.DeserializeObject<ContractorLotExcelData>(uploadExcelDto);
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault()
            };
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            _contractorParameter.UserId = objectIdentifier;
            var file = pdf.Files.FirstOrDefault();

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            _contractorParameter.Lang = lang;
            _contractorParameter.ContractorLotExcelData = myObj;
            _contractorParameter.ContextAccessor = ContextAccessor;
            _contractorParameter.TenantProvider = ItenantProvider;
            _contractorParameter.File = file;
            _contractorParameter.GraphServiceClient = _graphServiceClient;
            _contractorParameter.Configuration = _iConfiguration;
            return Ok(new ApiOkResponse(await _iContractorReopsitory.ContractorLotExcelUpload(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpPost("SendInvitation")]
    public async Task<ActionResult> SendInvitation([FromBody] ContractorTeam ContractorTeam,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)

    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                ContractorTeam = ContractorTeam,
                Configuration = _iConfiguration
            };
            return Ok(new ApiOkResponse(
                await _iContractorReopsitory.SendInvitation(_contractorParameter, _iSendGridRepositorie)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpPost("PublishTender")]
    public async Task<ActionResult> AcceptInvitation([FromBody] ContractorTeam ContractorTeam,
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
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContractorTeam = ContractorTeam,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                GraphServiceClient = _graphServiceClient,
                Configuration = _iConfiguration
            };
            var s = await _iContractorReopsitory.PublishTender(_contractorParameter, _iGraphRepository);
            return Ok(new ApiOkResponse(s));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }


    [AllowAnonymous]
    [HttpPost("ApproveInvitation")]
    public async Task<ActionResult> ApproveInvitation([FromBody] AcceptInvitationDto AcceptInvitationDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)

    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                SendGridRepositorie = _iSendGridRepositorie,
                AcceptInvitationDto = AcceptInvitationDto,
                Configuration = _iConfiguration
            };
            return Ok(new ApiOkResponse(await _iContractorReopsitory.ApproveInvitation(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }


    [HttpGet("LotPublish/{lotId}")]
    public async Task<ActionResult> LotPublish(
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, string lotId)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                Configuration = _iConfiguration,
                Id = lotId,
                SendGridRepositorie = _iSendGridRepositorie,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };
            return Ok(new ApiOkResponse(await _iContractorReopsitory.LotPublish(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpPost("ApproveComment")]
    public async Task<ActionResult> ApproveComment([FromBody] AcceptComment AcceptComment,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                AcceptComment = AcceptComment,
                Configuration = _iConfiguration,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };
            return Ok(new ApiOkResponse(await _iContractorReopsitory.AcceptComment(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpGet("GetCommentLogDropDownData")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetCommentLogDropDownData([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                Configuration = _iConfiguration,
                TenantProvider = ItenantProvider
            };
            return Ok(new ApiOkResponse(
                await _iContractorReopsitory.GetCommentLogDropDownData(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpPost("UpdateCommentLogDropDown")]
    public async Task<ActionResult> UpdateCommentLogDropDown(
        [FromBody] CommentCardContractorDto CommentCardContractorDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                CommentCardContractorDto = CommentCardContractorDto,
                Configuration = _iConfiguration,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };
            return Ok(new ApiOkResponse(await _iContractorReopsitory.UpdateCommentLogDropDown(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpPost("GetLotByUser")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetLotByUser([FromBody] FilterByUser FilterByUser,
        [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                FilterByUser = FilterByUser,
                Configuration = _iConfiguration,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider
            };
            return Ok(new ApiOkResponse(
                await _iContractorReopsitory.GetLotByUser(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpPost("GetContractingUnitByUser")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetContractingUnitByUser([FromBody] FilterByUser FilterByUser,
        [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                FilterByUser = FilterByUser,
                Configuration = _iConfiguration,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider
            };
            return Ok(new ApiOkResponse(
                await _iContractorReopsitory.GetContractingUnitByUser(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpPost("GetProjectsByUser")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetProjectsByUser([FromBody] FilterByUser FilterByUser,
        [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                FilterByUser = FilterByUser,
                Configuration = _iConfiguration,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider
            };
            return Ok(new ApiOkResponse(
                await _iContractorReopsitory.GetProjectsByUser(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpGet("GetLotTotalPriceById/{lotId}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetLotTotalPriceById([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, string lotId)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                Id = lotId,
                Configuration = _iConfiguration,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider
            };
            return Ok(new ApiOkResponse(
                await _iContractorReopsitory.GetLotTotalPriceById(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpPost("AddTenderAwardWinner")]
    public async Task<ActionResult> AddTenderAwardWinner([FromBody] AwardWinner awardWinner,
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
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                AwardWinner = awardWinner,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                SendGridRepositorie = _iSendGridRepositorie,
                Configuration = _iConfiguration,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };
            var s = await _iContractorReopsitory.AddTenderAwardWinner(_contractorParameter, _iSendGridRepositorie);
            return Ok(new ApiOkResponse(s));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpPost("ContractorLotUploadDocuments")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ContractorLotUploadDocuments([FromForm] string documentsData,
        [FromForm] IFormCollection doc)
    {
        try
        {
            var myObj = JsonConvert.DeserializeObject<ContractorLotUploadedDocs>(documentsData);
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };

            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var file = doc.Files.FirstOrDefault();

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            _contractorParameter.Lang = lang;
            _contractorParameter.ContractorLotUploadedDocs = myObj;
            _contractorParameter.ContextAccessor = ContextAccessor;
            _contractorParameter.TenantProvider = ItenantProvider;
            _contractorParameter.File = file;
            _contractorParameter.Configuration = _iConfiguration;
            _contractorParameter.GraphServiceClient = _graphServiceClient;
            return Ok(
                new ApiOkResponse(await _iContractorReopsitory.ContractorLotUploadDocuments(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpGet("GetContractorLotUploadDocuments/{lotId}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetContractorLotUploadDocuments([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, string lotId)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                Id = lotId,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider,
                Configuration = _iConfiguration
            };
            return Ok(new ApiOkResponse(
                await _iContractorReopsitory.GetContractorLotUploadDocuments(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpGet("GetUserInformation")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetUserInformation()
    {
        try
        {
            var _contractorParameter = new ContractorParameter
            {
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider,
                Configuration = _iConfiguration
            };
            return Ok(new ApiOkResponse(
                await _iContractorReopsitory.GetUserInformation(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpGet("GetLoggedUserType")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetLoggedUserType()
    {
        try
        {
            var _contractorParameter = new ContractorParameter
            {
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                TenantProvider = ItenantProvider,
                Configuration = _iConfiguration
            };
            return Ok(new ApiOkResponse(
                await _iContractorReopsitory.GetLoggedUserType(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    //[AllowAnonymous]
    [HttpPost("DownloadLotDocuments")]
    public async Task<ActionResult> DownloadLotDocuments([FromBody] DownloadLotDocsDto DownloadLotDocsDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)

    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                SendGridRepositorie = _iSendGridRepositorie,
                DownloadLotDocsDto = DownloadLotDocsDto,
                Configuration = _iConfiguration
            };
            return Ok(new ApiOkResponse(await _iContractorReopsitory.DownloadLotDocuments(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [AllowAnonymous]
    [HttpPost("SubscribeLot")]
    public async Task<ActionResult> SubscribeLot([FromBody] DownloadLotDocsDto DownloadLotDocsDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)

    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                SendGridRepositorie = _iSendGridRepositorie,
                DownloadLotDocsDto = DownloadLotDocsDto,
                GraphRepository = _iGraphRepository,
                Configuration = _iConfiguration,
                GraphServiceClient = _graphServiceClient
            };
            return Ok(new ApiOkResponse(await _iContractorReopsitory.SubscribeLot(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpGet("GetLotCbcTree/{lotId}")]
    public async Task<ActionResult> GetLotCbcTree(string lotId,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)

    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                SendGridRepositorie = _iSendGridRepositorie,
                GraphRepository = _iGraphRepository,
                Configuration = _iConfiguration,
                GraphServiceClient = _graphServiceClient,
                Id = lotId
            };
            return Ok(new ApiOkResponse(await _iContractorReopsitory.GetLotCbcTree(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpGet("GetPsSequenceIdByLotIdForZeroState/{conractId}")]
    public async Task<ActionResult> GetPsSequenceIdByLotIdForZeroState(
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, string conractId)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                _pdfToExcelRepository = _iPdfToExcelRepository,
                Id = conractId,
                Configuration = _iConfiguration
            };
            return Ok(new ApiOkResponse(
                await _iContractorReopsitory.GetPsSequenceIdByLotIdForZeroState(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpGet("GetContractorPsDataForZeroState/{conractId}")]
    public async Task<ActionResult> GetContractorPsDataForZeroState(
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, string conractId)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                Id = conractId,
                Configuration = _iConfiguration
            };
            return Ok(new ApiOkResponse(
                await _iContractorReopsitory.GetContractorPsDataForZeroState(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpGet("ContractorPsErrorLogForZeroState/{conractId}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ContractorPsErrorLogForZeroState([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, string conractId)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                Id = conractId,
                Configuration = _iConfiguration
            };
            return Ok(new ApiOkResponse(
                await _iContractorReopsitory.ContractorPsErrorLogForZeroState(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpPost("SaveContractorPsData")]
    public async Task<ActionResult> SaveContractorPsData([FromBody] SaveContractorPs Ps,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                PsData = Ps,
                Configuration = _iConfiguration,
                SendGridRepositorie = _iSendGridRepositorie,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };
            return Ok(new ApiOkResponse(await _iContractorReopsitory.SaveContractorPsData(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpPost("CreateCommentCardForPs")]
    public async Task<ActionResult> CreateCommentCardForPs([FromBody] CreateCommentCardPs CommentCard,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                CommentCardPs = CommentCard,
                Configuration = _iConfiguration,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };
            return Ok(new ApiOkResponse(await _iContractorReopsitory.CreateCommentCardForPs(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpPost("GetPsComment")]
    public async Task<ActionResult> GetPsComment([FromBody] CommentFilter CommentFilter,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                CommentFilter = CommentFilter,
                Configuration = _iConfiguration,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };
            return Ok(new ApiOkResponse(await _iContractorReopsitory.GetPsComment(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpPost("AddCommentForPs")]
    public async Task<ActionResult> AddCommentForPs([FromBody] ContractorComment ContractorComment,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                ContractorComment = ContractorComment,
                Configuration = _iConfiguration,
                SendGridRepositorie = _iSendGridRepositorie,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };

            return Ok(new ApiOkResponse(
                await _iContractorReopsitory.AddCommentForPs(_contractorParameter, _iSendGridRepositorie)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpPost("UpdateCommentLogDropDownForPs")]
    public async Task<ActionResult> UpdateCommentLogDropDownForPs(
        [FromBody] CommentCardContractorDto CommentCardContractorDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                CommentCardContractorDto = CommentCardContractorDto,
                Configuration = _iConfiguration,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };
            return Ok(new ApiOkResponse(
                await _iContractorReopsitory.UpdateCommentLogDropDownForPs(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpPost("ApproveCommentForPs")]
    public async Task<ActionResult> ApproveCommentForPs([FromBody] AcceptComment AcceptComment,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                AcceptComment = AcceptComment,
                Configuration = _iConfiguration,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };
            return Ok(new ApiOkResponse(await _iContractorReopsitory.ApproveCommentForPs(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpGet("ContractorPsOrderNumberDropDown/{lotId}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ContractorPsOrderNumberDropDown([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, string lotId)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                Id = lotId,
                Configuration = _iConfiguration
            };
            return Ok(new ApiOkResponse(
                await _iContractorReopsitory.ContractorPsOrderNumberDropDown(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpGet("PublishContractorPs/{lotId}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> PublishContractorPs([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, string lotId)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                Id = lotId,
                Configuration = _iConfiguration
            };
            return Ok(new ApiOkResponse(
                await _iContractorReopsitory.PublishContractorPs(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpGet("ApproveContractorPs")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ApproveContractorPs([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, string lotId, string psSequenceId)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                lotId = lotId,
                psSequenceId = psSequenceId,
                Configuration = _iConfiguration
            };
            return Ok(new ApiOkResponse(
                await _iContractorReopsitory.ApproveContractorPs(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpPost("ContractorPsMinusPlusWork")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ContractorPsMinusPlusWork([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromForm] string uploadExcelDto, [FromForm] IFormCollection pdf)
    {
        try
        {
            var myObj = JsonConvert.DeserializeObject<ContractorPsMinusPlusWorkDto>(uploadExcelDto);
            var file = pdf.Files.FirstOrDefault();
            var _contractorParameter = new ContractorParameter
            {
                File = file,
                GraphServiceClient = _graphServiceClient
            };
            var lang = langCode(langX);
            _contractorParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            _contractorParameter.ContractingUnitSequenceId = CU;
            _contractorParameter.ProjectSequenceId = Project;
            _contractorParameter.Lang = lang;
            _contractorParameter.ContextAccessor = ContextAccessor;
            _contractorParameter.TenantProvider = ItenantProvider;
            _contractorParameter.Id = myObj.LotId;
            _contractorParameter.MinusPlusWorkDto = myObj;
            _contractorParameter.Configuration = _iConfiguration;
            return Ok(new ApiOkResponse(
                await _iContractorReopsitory.ContractorPsMinusPlusWork(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpGet("GetContractorPsMinusPlusWork/{lotId}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetContractorPsMinusPlusWork([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, string lotId)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                Id = lotId,
                Configuration = _iConfiguration
            };
            return Ok(new ApiOkResponse(
                await _iContractorReopsitory.GetContractorPsMinusPlusWork(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpPost("UpdateContractorPsMinusPlusWorkTotalPrice")]
    public async Task<ActionResult> UpdateContractorPsMinusPlusWorkTotalPrice(
        [FromBody] List<GetContractorPsMinusPlusWork> data,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                GetContractorPsMinusPlusWork = data,
                Configuration = _iConfiguration,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };
            return Ok(new ApiOkResponse(
                await _iContractorReopsitory.UpdateContractorPsMinusPlusWorkTotalPrice(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpGet("ApproveContractorPsMinusPlusWork/{Id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ApproveContractorPsMinusPlusWork([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, string Id)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                Id = Id,
                Configuration = _iConfiguration
            };
            return Ok(new ApiOkResponse(
                await _iContractorReopsitory.ApproveContractorPsMinusPlusWork(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpPost("ZipDownload")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ZipDownload([FromForm] string documentsData, [FromForm] IFormCollection doc)
    {
        try
        {
            var myObj = JsonConvert.DeserializeObject<ContractorLotUploadedDocs>(documentsData);
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };

            var file = doc.Files.FirstOrDefault();
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            _contractorParameter.Lang = lang;
            _contractorParameter.ContractorLotUploadedDocs = myObj;
            _contractorParameter.ContextAccessor = ContextAccessor;
            _contractorParameter.TenantProvider = ItenantProvider;
            _contractorParameter.File = file;
            _contractorParameter.GraphServiceClient = _graphServiceClient;
            _contractorParameter.Configuration = _iConfiguration;
            return Ok(new ApiOkResponse(await _iContractorReopsitory.ZipDownload(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [AllowAnonymous]
    [HttpPost("LotFileUpload")]
    public async Task<ActionResult<string>> LotFileUpload([FromForm] IFormCollection file, [FromForm] string lotId,
        [FromForm] string documentCategory,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var client = new FileClient();
            var url = client.PersistLotDocUpload(file.Files.FirstOrDefault()?.FileName, _TenantProvider
                , file.Files.FirstOrDefault(), lotId, documentCategory);

            var response = new ApiOkResponse(url);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [AllowAnonymous]
    [HttpPost("LotDocsZipDownload")]
    public async Task<ActionResult<string>> LotDocsZipDownload([FromForm] string lotId,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var blobContainerClient = new BlobContainerClient(_TenantProvider.GetTenant().StorageConnectionString,
                _TenantProvider.GetTenant().AzureContainer);
            var prefixPath = "LotDocuments" + "/" + lotId + "/tenderDocument";
            var blobs = blobContainerClient.GetBlobs(prefix: prefixPath);
            //var vv = blobs.Count();
            var generator = new Random();
            var r = generator.Next(0, 1000000).ToString("D6");

            var zipFileName = "LotDocs" + r + ".zip";

            foreach (var blobItem in blobs)
            {
                // Download the blob to a temporary file
                var blobClient = blobContainerClient.GetBlobClient(blobItem.Name);
                var tempFilePath = Path.GetTempFileName();
                using (var fileStream = new FileStream(tempFilePath, FileMode.Open, FileAccess.Write, FileShare.Write))
                {
                    blobClient.DownloadTo(fileStream);
                }
                // Add the temporary file to the zip archive
                using (var fileStream = new FileStream(tempFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var archive = ZipFile.Open(zipFileName, ZipArchiveMode.Update))
                    {
                        var entryName = Path.GetFileName(blobItem.Name);
                        archive.CreateEntryFromFile(fileStream.Name, entryName);
                    }
                }
                System.IO.File.Delete(tempFilePath);

                // Delete the temporary file
            }
            string zipPath = null;
            // Upload the zip file to Azure Blob Storage
            var zipBlobClient = blobContainerClient.GetBlobClient(zipFileName);
            using (var fileStream = new FileStream(zipFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                zipBlobClient.Upload(fileStream, true);
                zipPath = fileStream.Name;
            }
            //Delete the zip file
            System.IO.File.Delete(zipPath);
            return zipBlobClient.Uri.AbsoluteUri;
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpGet("GetContractorListByLotId/{Id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetContractorListByLotId([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, string Id)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                Id = Id,
                Configuration = _iConfiguration
            };

            return Ok(new ApiOkResponse(
                await _iContractorReopsitory.GetContractorListByLotId(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    //[Authorize]
    [HttpGet("GetContractorTotalPriceErrorsByLotId/{Id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetContractorTotalPriceErrorsByLotId([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, string Id)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                Id = Id,
                Configuration = _iConfiguration
            };

            return Ok(new ApiOkResponse(
                await _iContractorReopsitory.GetContractorTotalPriceErrorsByLotId(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpGet("IsUnresolvedComment/{conractId}")]
    public async Task<ActionResult> IsUnresolvedComment(
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, string conractId)
    {
        try
        {
            var _contractorParameter = new ContractorParameter
            {
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };

            var lang = langCode(langX);
            _contractorParameter.ContractingUnitSequenceId = CU;
            _contractorParameter.ProjectSequenceId = Project;
            _contractorParameter.Lang = lang;
            _contractorParameter.ContextAccessor = ContextAccessor;
            _contractorParameter.TenantProvider = ItenantProvider;
            _contractorParameter.Id = conractId;
            _contractorParameter.Configuration = _iConfiguration;

            return Ok(new ApiOkResponse(
                await _iContractorReopsitory.IsUnresolvedComment(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpGet("GetAwardedLotInProject")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetAwardedLotInProject([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, string Id)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                Id = Id,
                Configuration = _iConfiguration
            };

            return Ok(new ApiOkResponse(
                await _iContractorReopsitory.GetAwardedLotInProject(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpPost("GetAwardedContractorLotData")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetAwardedContractorLotData([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromBody] AwardedLotDataDto dto)
    {
        try
        {
            var lang = langCode(langX);
            var _contractorParameter = new ContractorParameter
            {
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                AwardedLotDataDto = dto,
                Configuration = _iConfiguration
            };
            return Ok(new ApiOkResponse(
                await _iContractorReopsitory.GetAwardedContractorLotData(_contractorParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }
    
    [Authorize]
    [HttpPost("DriveData")]
    public async Task<ActionResult> DriveData( [FromForm] IFormCollection pdf)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }
            var file = pdf.Files.FirstOrDefault();
            var drives = await _graphServiceClient.Drives.GetAsync();
            var driveId = drives.Value.FirstOrDefault(x => x.Name == "Documents").Id;
            var uploadedItemx = await _graphServiceClient
                .Drives[driveId]
                .Root
                .ItemWithPath("test1/" + file.FileName)
                .Content
                .PutAsync(file.OpenReadStream());
            
            return Ok(new ApiOkResponse(uploadedItemx.WebUrl));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }
}