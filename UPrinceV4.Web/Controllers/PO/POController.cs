using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.PO;
using UPrinceV4.Web.Data.ProjectLocationDetails;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces.PO;

namespace UPrinceV4.Web.Controllers.PO;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class POController : CommonConfigurationController
{
    private readonly IPORepository _iPORepository;

    private readonly ITenantProvider _TenantProvider;


    public POController(ITenantProvider tenantProvider,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse,
        ITenantProvider iTenantProvider
        , IPORepository iPoRepository, IConfiguration iConfiguration)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)

    {
        _iPORepository = iPoRepository;
        _TenantProvider = tenantProvider;
        _iConfiguration = iConfiguration;
    }

    private IConfiguration _iConfiguration { get; }

    [HttpGet("ShortcutPaneData")]
    public async Task<ActionResult> ReadPOShortcutPaneData([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _poParameter = new POParameter();
            _poParameter.ContractingUnitSequenceId = CU;
            _poParameter.ProjectSequenceId = Project;
            _poParameter.Lang = lang;
            _poParameter.ContextAccessor = ContextAccessor;
            _poParameter.TenantProvider = _TenantProvider;
            var shortcutPaneData = await _iPORepository.GetShortcutpaneData(_poParameter);

            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("FilterPO")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetPOList([FromBody] POFilter filter,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _poParameter = new POParameter();
            _poParameter.ContractingUnitSequenceId = CU;
            _poParameter.ProjectSequenceId = Project;
            _poParameter.Lang = lang;
            _poParameter.Filter = filter;
            _poParameter.ContextAccessor = ContextAccessor;
            _poParameter.TenantProvider = _TenantProvider;
            var result = await _iPORepository.GetPOList(_poParameter);
            var mApiOkResponse = new ApiOkResponse(result);


            if (!result.Any())
            {
                var mApiResponse = new ApiOkResponse(null, "noAvailablePo")
                {
                    Status = false
                };
                return Ok(mApiResponse);
            }

            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CreateHeader")]
    public async Task<ActionResult> CreateHeader([FromBody] POCreateDto PoDto,
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
            var _poParameter = new POParameter();
            _poParameter.ContractingUnitSequenceId = CU;
            _poParameter.ProjectSequenceId = Project;
            if (Project == null) PoDto.IsCu = true;

            _poParameter.Lang = lang;
            _poParameter.PoDto = PoDto;
            _poParameter.ContextAccessor = ContextAccessor;
            _poParameter.TenantProvider = ItenantProvider;
            _poParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var s = await _iPORepository.CreateHeader(_poParameter, false, false);
            return Ok(new ApiOkResponse(s));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetPoDropdownData")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetPoDropdownData([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX, CancellationToken cancellationToken)
    {
        try
        {
            var lang = langCode(langX);
            var _poParameter = new POParameter();
            _poParameter.ContractingUnitSequenceId = CU;
            _poParameter.ProjectSequenceId = project;
            _poParameter.Lang = lang;

            _poParameter.TenantProvider = ItenantProvider;
            return Ok(new ApiOkResponse(await _iPORepository.GetPbsDropdown(_poParameter, cancellationToken)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    [HttpGet("ReadPOHeaderById/{SequenceId}")]
    public async Task<ActionResult> GetPOHeaderById(string SequenceId, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _poParameter = new POParameter();
            _poParameter.ContractingUnitSequenceId = CU;
            _poParameter.ProjectSequenceId = Project;
            _poParameter.Lang = lang;
            _poParameter.Id = SequenceId;
            _poParameter.ContextAccessor = ContextAccessor;
            _poParameter.TenantProvider = ItenantProvider;
            _poParameter.Configuration = _iConfiguration;
            return Ok(new ApiOkResponse(await _iPORepository.GetPOById(_poParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
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
            var _poParameter = new POParameter();
            _poParameter.ContractingUnitSequenceId = CU;
            _poParameter.ProjectSequenceId = Project;
            _poParameter.Lang = lang;

            var client = new FileClient();
            var url = client.PersistPhotoInNewFolder(image.Files.FirstOrDefault()?.FileName, _TenantProvider
                , image.Files.FirstOrDefault(), "PO Documents");

            var response = new ApiOkResponse(url);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("ProjectSend")]
    public async Task<ActionResult> ProjectSend([FromBody] POCreateDto PoDto,
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
            var _poParameter = new POParameter();
            _poParameter.ContractingUnitSequenceId = CU;
            _poParameter.ProjectSequenceId = Project;
            _poParameter.Lang = lang;
            _poParameter.PoDto = PoDto;
            _poParameter.ContextAccessor = ContextAccessor;
            _poParameter.TenantProvider = ItenantProvider;
            _poParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            return Ok(new ApiOkResponse(await _iPORepository.ProjectSend(_poParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CUSend")]
    public async Task<ActionResult> CUSend([FromBody] POCreateDto PoDto,
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
            var _poParameter = new POParameter();
            _poParameter.ContractingUnitSequenceId = CU;
            _poParameter.ProjectSequenceId = null;
            _poParameter.Lang = lang;
            _poParameter.PoDto = PoDto;
            _poParameter.ContextAccessor = ContextAccessor;
            _poParameter.TenantProvider = ItenantProvider;
            _poParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var s = await _iPORepository.CUSend(_poParameter);
            return Ok(new ApiOkResponse(s));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CUApprove")]
    public async Task<ActionResult> CUApprove([FromBody] POCreateDto PoDto,
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
            var _poParameter = new POParameter();
            _poParameter.ContractingUnitSequenceId = CU;
            _poParameter.ProjectSequenceId = Project;
            _poParameter.Lang = lang;
            _poParameter.PoDto = PoDto;
            _poParameter.ContextAccessor = ContextAccessor;
            _poParameter.TenantProvider = ItenantProvider;
            _poParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var s = await _iPORepository.CUApprove(_poParameter);
            return Ok(new ApiOkResponse(s));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CULevelApprove")]
    public async Task<ActionResult> CULevelApprove([FromBody] POCreateDto PoDto,
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
            var _poParameter = new POParameter();
            _poParameter.ContractingUnitSequenceId = CU;
            _poParameter.ProjectSequenceId = null;
            _poParameter.Lang = lang;
            _poParameter.PoDto = PoDto;
            _poParameter.ContextAccessor = ContextAccessor;
            _poParameter.TenantProvider = ItenantProvider;
            _poParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var s = await _iPORepository.CULevelApprove(_poParameter);
            return Ok(new ApiOkResponse(s));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CUFeedback")]
    public async Task<ActionResult> CUFeedback([FromBody] POCreateDto PoDto,
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
            var _poParameter = new POParameter();
            _poParameter.ContractingUnitSequenceId = CU;
            _poParameter.ProjectSequenceId = Project;
            _poParameter.Lang = lang;
            _poParameter.PoDto = PoDto;
            _poParameter.ContextAccessor = ContextAccessor;
            _poParameter.TenantProvider = ItenantProvider;
            _poParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var s = await _iPORepository.CUFeedback(_poParameter);
            return Ok(new ApiOkResponse(s));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("PMApprove")]
    public async Task<ActionResult> PMApprove([FromBody] POCreateDto PoDto,
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
            var _poParameter = new POParameter();
            _poParameter.ContractingUnitSequenceId = CU;
            _poParameter.ProjectSequenceId = Project;
            _poParameter.Lang = lang;
            _poParameter.PoDto = PoDto;
            _poParameter.ContextAccessor = ContextAccessor;
            _poParameter.TenantProvider = ItenantProvider;
            _poParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var s = await _iPORepository.PMApprove(_poParameter);
            return Ok(new ApiOkResponse(s));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("PMAccept")]
    public async Task<ActionResult> PMAccept([FromBody] POCreateDto PoDto,
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
            var _poParameter = new POParameter();
            _poParameter.ContractingUnitSequenceId = CU;
            _poParameter.ProjectSequenceId = Project;
            _poParameter.Lang = lang;
            _poParameter.PoDto = PoDto;
            _poParameter.ContextAccessor = ContextAccessor;
            _poParameter.TenantProvider = ItenantProvider;
            _poParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var s = await _iPORepository.PMAccept(_poParameter);
            return Ok(new ApiOkResponse(s));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CreateLocation")]
    public async Task<ActionResult> CreateLocation([FromBody] MapLocation Location,
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
            var _poParameter = new POParameter();
            _poParameter.ContractingUnitSequenceId = CU;
            _poParameter.ProjectSequenceId = Project;
            _poParameter.Lang = lang;
            _poParameter.ContextAccessor = ContextAccessor;
            _poParameter.TenantProvider = ItenantProvider;
            _poParameter.MapLocation = Location;
            _poParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var id = await _iPORepository.CreateLocation(_poParameter);
            return Ok(new ApiOkResponse(id));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("POResourceFilter")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> POResourceFilter(
        [FromBody] POResourceFilterDto pOResourceFilterDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _poParameter = new POParameter();
            _poParameter.ContractingUnitSequenceId = CU;
            _poParameter.ProjectSequenceId = Project;
            _poParameter.Lang = lang;
            _poParameter.POResourceFilterDto = pOResourceFilterDto;
            _poParameter.ContextAccessor = ContextAccessor;
            _poParameter.TenantProvider = _TenantProvider;

            return Ok(new ApiOkResponse(await _iPORepository.POResourceFilter(_poParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CUPOResourceFilter")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CUPOResourceFilter(
        [FromBody] POResourceFilterDto pOResourceFilterDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _poParameter = new POParameter();
            _poParameter.ContractingUnitSequenceId = CU;
            _poParameter.ProjectSequenceId = Project;
            _poParameter.Lang = lang;
            _poParameter.POResourceFilterDto = pOResourceFilterDto;
            _poParameter.ContextAccessor = ContextAccessor;
            _poParameter.TenantProvider = _TenantProvider;

            return Ok(new ApiOkResponse(await _iPORepository.CUPOResourceFilter(_poParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    [HttpPost("POResourceStockUpdate")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> POResourceStockUpdate(
        [FromBody] POResourceStockUpdate POResourceStockUpdate,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _poParameter = new POParameter();
            _poParameter.ContractingUnitSequenceId = CU;
            _poParameter.ProjectSequenceId = Project;
            _poParameter.Lang = lang;
            _poParameter.POResourceStockUpdate = POResourceStockUpdate;
            _poParameter.ContextAccessor = ContextAccessor;
            _poParameter.TenantProvider = _TenantProvider;
            _poParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            var result = await _iPORepository.POResourceStockUpdate(_poParameter);
            var mApiOkResponse = new ApiOkResponse(result);
            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CUPOResourceStockUpdate")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CUPOResourceStockUpdate(
        [FromBody] POResourceStockUpdate poResourceStockUpdate,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _poParameter = new POParameter();
            _poParameter.ContractingUnitSequenceId = CU;
            _poParameter.ProjectSequenceId = Project;
            _poParameter.Lang = lang;
            _poParameter.POResourceStockUpdate = poResourceStockUpdate;
            _poParameter.ContextAccessor = ContextAccessor;
            _poParameter.TenantProvider = _TenantProvider;
            _poParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            var result = await _iPORepository.CUPOResourceStockUpdate(_poParameter);
            var mApiOkResponse = new ApiOkResponse(result);
            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("POProductIsPoUpdate")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> POProductIsPoUpdate(
        [FromBody] POProductIsPoUpdate POProductIsPoUpdate,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _poParameter = new POParameter();
            _poParameter.ContractingUnitSequenceId = CU;
            _poParameter.ProjectSequenceId = Project;
            _poParameter.Lang = lang;
            _poParameter.POProductIsPoUpdate = POProductIsPoUpdate;
            _poParameter.ContextAccessor = ContextAccessor;
            _poParameter.TenantProvider = _TenantProvider;
            _poParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            var result = await _iPORepository.POProductIsPoUpdate(_poParameter);
            var mApiOkResponse = new ApiOkResponse(result);
            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CUPOProductIsPoUpdate")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CUPOProductIsPoUpdate(
        [FromBody] POProductIsPoUpdate POProductIsPoUpdate,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _poParameter = new POParameter();
            _poParameter.ContractingUnitSequenceId = CU;
            _poParameter.ProjectSequenceId = Project;
            _poParameter.Lang = lang;
            _poParameter.POProductIsPoUpdate = POProductIsPoUpdate;
            _poParameter.ContextAccessor = ContextAccessor;
            _poParameter.TenantProvider = _TenantProvider;
            _poParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            var result = await _iPORepository.CUPOProductIsPoUpdate(_poParameter);
            var mApiOkResponse = new ApiOkResponse(result);
            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ExcelPOHeaderById/{SequenceId}")]
    public async Task<ActionResult> ExcelPOHeaderById(string SequenceId, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _poParameter = new POParameter();
            _poParameter.ContractingUnitSequenceId = CU;
            _poParameter.ProjectSequenceId = Project;
            _poParameter.Lang = lang;
            _poParameter.Id = SequenceId;
            _poParameter.ContextAccessor = ContextAccessor;
            _poParameter.TenantProvider = ItenantProvider;
            var data = await _iPORepository.GetPOById(_poParameter);

            return Ok(new ApiOkResponse(data));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("POProjectList")]
    public async Task<ActionResult> POProjectList([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _poParameter = new POParameter();
            _poParameter.ContractingUnitSequenceId = CU;
            _poParameter.ProjectSequenceId = Project;
            _poParameter.Lang = lang;
            _poParameter.ContextAccessor = ContextAccessor;
            _poParameter.TenantProvider = ItenantProvider;
            var data = await _iPORepository.POProjectList(_poParameter);

            return Ok(new ApiOkResponse(data));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("POBorResourceFilter")]
    public async Task<ActionResult> POBorResourceFilter(
        [FromBody] POBorResourceFilter POBorResourceFilter,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _poParameter = new POParameter();
            _poParameter.ContractingUnitSequenceId = CU;
            _poParameter.ProjectSequenceId = Project;
            _poParameter.Lang = lang;
            _poParameter.ContextAccessor = ContextAccessor;
            _poParameter.TenantProvider = ItenantProvider;
            _poParameter.POBorResourceFilter = POBorResourceFilter;
            var data = await _iPORepository.POBorResourceFilter(_poParameter);

            if (!data.Any())
            {
                var mApiResponse = new ApiOkResponse(null, "noAvailableBillOfResource");
                mApiResponse.Status = false;
                return Ok(mApiResponse);
            }

            return Ok(new ApiOkResponse(data));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("POPBSResourceFilter")]
    public async Task<ActionResult> POPBSResourceFilter(
        [FromBody] POPbsResourceFilter POPbsResourceFilter,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _poParameter = new POParameter();
            _poParameter.ContractingUnitSequenceId = CU;
            _poParameter.ProjectSequenceId = Project;
            _poParameter.Lang = lang;
            _poParameter.ContextAccessor = ContextAccessor;
            _poParameter.TenantProvider = ItenantProvider;
            _poParameter.POPbsResourceFilter = POPbsResourceFilter;
            var data = await _iPORepository.POPBSResourceFilter(_poParameter);

            if (!data.Any())
            {
                var mApiResponse = new ApiOkResponse(null, "noAvailableprojectbreakdownstructure");
                mApiResponse.Status = false;
                return Ok(mApiResponse);
            }

            return Ok(new ApiOkResponse(data));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("POBorUpdate")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> POBorUpdate(
        [FromBody] POResourceStockUpdate POResourceStockUpdate,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _poParameter = new POParameter();
            _poParameter.ContractingUnitSequenceId = CU;
            _poParameter.ProjectSequenceId = Project;
            _poParameter.Lang = lang;
            _poParameter.POResourceStockUpdate = POResourceStockUpdate;
            _poParameter.ContextAccessor = ContextAccessor;
            _poParameter.TenantProvider = _TenantProvider;
            _poParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            return Ok(new ApiOkResponse(await _iPORepository.POBorUpdate(_poParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CUPOBorUpdate")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CUPOBorUpdate(
        [FromBody] POResourceStockUpdate POResourceStockUpdate,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _poParameter = new POParameter();
            _poParameter.ContractingUnitSequenceId = CU;
            _poParameter.ProjectSequenceId = Project;
            _poParameter.Lang = lang;
            _poParameter.POResourceStockUpdate = POResourceStockUpdate;
            _poParameter.ContextAccessor = ContextAccessor;
            _poParameter.TenantProvider = _TenantProvider;
            _poParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            var result = await _iPORepository.CUPOBorUpdate(_poParameter);
            var mApiOkResponse = new ApiOkResponse(result);
            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("ConvertToPo")]
    public async Task<ActionResult> ConvertToPo(POCreateDto PoDto, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _poParameter = new POParameter();
            _poParameter.ContractingUnitSequenceId = CU;
            _poParameter.ProjectSequenceId = Project;
            _poParameter.Lang = lang;
            _poParameter.PoDto = PoDto;
            _poParameter.ContextAccessor = ContextAccessor;
            _poParameter.TenantProvider = ItenantProvider;
            _poParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            return Ok(new ApiOkResponse(await _iPORepository.ConvertToPo(_poParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("AddPoLabourTeam")]
    public async Task<ActionResult> AddPoLabourTeam(POLabourTeam POLabourTeam, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _poParameter = new POParameter();
            _poParameter.ContractingUnitSequenceId = CU;
            _poParameter.ProjectSequenceId = Project;
            _poParameter.Lang = lang;
            _poParameter.POLabourTeam = POLabourTeam;
            _poParameter.ContextAccessor = ContextAccessor;
            _poParameter.TenantProvider = ItenantProvider;
            _poParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            return Ok(new ApiOkResponse(await _iPORepository.AddPoLabourTeam(_poParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetPoLabourTeam")]
    public async Task<ActionResult> GetPoLabourTeam([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _poParameter = new POParameter();
            _poParameter.ContractingUnitSequenceId = CU;
            _poParameter.ProjectSequenceId = Project;
            _poParameter.Lang = lang;
            _poParameter.ContextAccessor = ContextAccessor;
            _poParameter.TenantProvider = ItenantProvider;
            _poParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            return Ok(new ApiOkResponse(await _iPORepository.GetPoLabourTeam(_poParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("AddPoToolPool")]
    public async Task<ActionResult> AddPoToolPool(POToolPool POToolPool, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _poParameter = new POParameter();
            _poParameter.ContractingUnitSequenceId = CU;
            _poParameter.ProjectSequenceId = Project;
            _poParameter.Lang = lang;
            _poParameter.POToolPool = POToolPool;
            _poParameter.ContextAccessor = ContextAccessor;
            _poParameter.TenantProvider = ItenantProvider;
            _poParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            return Ok(new ApiOkResponse(await _iPORepository.AddPoToolPool(_poParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}