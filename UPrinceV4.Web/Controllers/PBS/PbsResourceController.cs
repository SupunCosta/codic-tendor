using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Repositories.Interfaces.BOR;
using UPrinceV4.Web.Repositories.Interfaces.CPC;
using UPrinceV4.Web.Repositories.Interfaces.PBS;

namespace UPrinceV4.Web.Controllers.PBS;


[Authorize]
[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class PbsResourceController : CommonConfigurationController
{
   
    private readonly ICoporateProductCatalogRepository _ICoporateProductCatalogRepository;
    private readonly IPbsResourceRepository _IPbsResourceRepository;
    private readonly IBorResourceRepository _iBorResourceRepositoryRepository;
    private readonly IBorRepository _iBorRepository;
    private readonly IVPRepository _iVpRepository;
    private readonly IProjectDefinitionRepository _iProjectDefinitionRepository;

    public PbsResourceController(IPbsResourceRepository iPbsResourceRepository,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse,
        ITenantProvider iTenantProvider,
        ICoporateProductCatalogRepository iCoporateProductCatalogRepository,IBorResourceRepository iBorResourceRepository,IBorRepository iBorRepository ,
        IVPRepository iVpRepository,IConfiguration iConfiguration,IProjectDefinitionRepository iProjectDefinitionRepository)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)

    {
        _IPbsResourceRepository = iPbsResourceRepository;
        _ICoporateProductCatalogRepository = iCoporateProductCatalogRepository;
        _iBorResourceRepositoryRepository = iBorResourceRepository;
        _iBorRepository = iBorRepository;
        _iVpRepository = iVpRepository;
        _iConfiguration = iConfiguration;
        _iProjectDefinitionRepository = iProjectDefinitionRepository;
    }
    
    private IConfiguration _iConfiguration { get; }

    [HttpPost("CreatePbsLabour")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreatePbsLabour(
        [FromBody] LabourForPbsCreateDto pbsDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _PbsResourceParameters = new PbsResourceParameters();
            _PbsResourceParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsResourceParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsResourceParameters.Lang = lang;
            _PbsResourceParameters.PbsLabourCreateDto = pbsDto;
            _PbsResourceParameters.TenantProvider = ItenantProvider;
            _PbsResourceParameters.ContextAccessor = ContextAccessor;
            _PbsResourceParameters.ICoporateProductCatalogRepository = _ICoporateProductCatalogRepository;
            _PbsResourceParameters.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            return Ok(new ApiOkResponse(await _IPbsResourceRepository.CreatePbsLabour(_PbsResourceParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CreatePbsTool")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreatePbsTool([FromBody] ToolsForPbsCreateDto pbsDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _PbsResourceParameters = new PbsResourceParameters();

            _PbsResourceParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsResourceParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsResourceParameters.Lang = lang;
            _PbsResourceParameters.PbsToolCreateDto = pbsDto;
            _PbsResourceParameters.TenantProvider = ItenantProvider;
            _PbsResourceParameters.ContextAccessor = ContextAccessor;
            _PbsResourceParameters.ICoporateProductCatalogRepository = _ICoporateProductCatalogRepository;
            _PbsResourceParameters.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            return Ok(new ApiOkResponse(await _IPbsResourceRepository.CreatePbsTool(_PbsResourceParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CreatePbsMaterial")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreatePbsMaterial(
        [FromBody] MaterialForPbsCreateDto pbsDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _PbsResourceParameters = new PbsResourceParameters();

            _PbsResourceParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsResourceParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsResourceParameters.Lang = lang;
            _PbsResourceParameters.PbsMaterialCreateDto = pbsDto;
            _PbsResourceParameters.TenantProvider = ItenantProvider;
            _PbsResourceParameters.ContextAccessor = ContextAccessor;
            _PbsResourceParameters.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            _PbsResourceParameters.ICoporateProductCatalogRepository = _ICoporateProductCatalogRepository;
            return Ok(new ApiOkResponse(await _IPbsResourceRepository.CreatePbsMaterial(_PbsResourceParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CreatePbsConsumable")]
    
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreatePbsConsumable(
        [FromBody] ConsumableForPbsCreateDto pbsDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _PbsResourceParameters = new PbsResourceParameters();

            _PbsResourceParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsResourceParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsResourceParameters.Lang = lang;
            _PbsResourceParameters.PbsConsumableCreateDto = pbsDto;
            _PbsResourceParameters.TenantProvider = ItenantProvider;
            _PbsResourceParameters.ContextAccessor = ContextAccessor;
            _PbsResourceParameters.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            _PbsResourceParameters.ICoporateProductCatalogRepository = _ICoporateProductCatalogRepository;
            return Ok(new ApiOkResponse(await _IPbsResourceRepository.CreatePbsConsumable(_PbsResourceParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpDelete("DeletePbsLabour")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeletePbsLabour([FromBody] List<string> idList)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _PbsResourceParameters = new PbsResourceParameters();
            _PbsResourceParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsResourceParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsResourceParameters.Lang = lang;
            _PbsResourceParameters.IdList = idList;
            _PbsResourceParameters.TenantProvider = ItenantProvider;
            await _IPbsResourceRepository.DeletePbsLabour(_PbsResourceParameters);
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpDelete("DeletePbsTool")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeletePbsTool([FromBody] List<string> idList)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _PbsResourceParameters = new PbsResourceParameters();
            _PbsResourceParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsResourceParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsResourceParameters.Lang = lang;
            _PbsResourceParameters.IdList = idList;
            _PbsResourceParameters.TenantProvider = ItenantProvider;
            await _IPbsResourceRepository.DeletePbsTool(_PbsResourceParameters);
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpDelete("DeletePbsMaterial")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeletePbsMaterial([FromBody] List<string> idList)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _PbsResourceParameters = new PbsResourceParameters();
            _PbsResourceParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsResourceParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsResourceParameters.Lang = lang;
            _PbsResourceParameters.IdList = idList;
            _PbsResourceParameters.TenantProvider = ItenantProvider;
            await _IPbsResourceRepository.DeletePbsMaterial(_PbsResourceParameters);
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpDelete("DeletePbsConsumable")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeletePbsConsumable([FromBody] List<string> idList)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _PbsResourceParameters = new PbsResourceParameters();
            _PbsResourceParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsResourceParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsResourceParameters.Lang = lang;
            _PbsResourceParameters.IdList = idList;
            _PbsResourceParameters.TenantProvider = ItenantProvider;
            await _IPbsResourceRepository.DeletePbsConsumable(_PbsResourceParameters);
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadMaterial")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetMaterial()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _PbsResourceParameters = new PbsResourceParameters();

            _PbsResourceParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsResourceParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsResourceParameters.Lang = lang;
            _PbsResourceParameters.TenantProvider = ItenantProvider;
            _PbsResourceParameters.Name = HttpContext.Request.Query["name"];
            return Ok(new ApiOkResponse(await _IPbsResourceRepository.GetMaterial(_PbsResourceParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadMaterialByPbsProduct/{pbsProductId}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetMaterialByPbsProduct(string pbsProductId)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _PbsResourceParameters = new PbsResourceParameters();

            _PbsResourceParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsResourceParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsResourceParameters.Lang = lang;
            _PbsResourceParameters.TenantProvider = ItenantProvider;
            _PbsResourceParameters.PbsProductId = pbsProductId;
            return Ok(new ApiOkResponse(await _IPbsResourceRepository.GetMaterialByProductId(_PbsResourceParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadTool")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetTool()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _PbsResourceParameters = new PbsResourceParameters();

            _PbsResourceParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsResourceParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsResourceParameters.Lang = lang;
            _PbsResourceParameters.TenantProvider = ItenantProvider;
            _PbsResourceParameters.Name = HttpContext.Request.Query["name"];
            var dropDownData = await _IPbsResourceRepository.GetTool(_PbsResourceParameters);

            return Ok(new ApiOkResponse(dropDownData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadToolByPbsProduct/{pbsProductId}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetToolByPbsProduct(string pbsProductId)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _PbsResourceParameters = new PbsResourceParameters();

            _PbsResourceParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsResourceParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsResourceParameters.Lang = lang;
            _PbsResourceParameters.TenantProvider = ItenantProvider;
            _PbsResourceParameters.PbsProductId = pbsProductId;
            return Ok(new ApiOkResponse(await _IPbsResourceRepository.GetToolByProductId(_PbsResourceParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadLabour")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetLabour()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _PbsResourceParameters = new PbsResourceParameters();

            _PbsResourceParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsResourceParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsResourceParameters.Lang = lang;
            _PbsResourceParameters.TenantProvider = ItenantProvider;
            _PbsResourceParameters.Name = HttpContext.Request.Query["name"];
            return Ok(new ApiOkResponse(await _IPbsResourceRepository.GetLabour(_PbsResourceParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadLabourByPbsProduct/{pbsProductId}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetLabourByPbsProduct(string pbsProductId)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _PbsResourceParameters = new PbsResourceParameters
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                TenantProvider = ItenantProvider,
                PbsProductId = pbsProductId
            };
            return Ok(new ApiOkResponse(await _IPbsResourceRepository.GetLabourByProductId(_PbsResourceParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    //[Microsoft.AspNetCore.Mvc.HttpGet("ReadConsumable")]
    [HttpGet("ReadConsumable")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetConsumable()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var pbsResourceParameters = new PbsResourceParameters
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                TenantProvider = ItenantProvider,
                Name = HttpContext.Request.Query["name"]
            };

            return Ok(new ApiOkResponse(await _IPbsResourceRepository.GetConsumable(pbsResourceParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadConsumableByPbsProduct/{pbsProductId}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetConsumableByPbsProduct(string pbsProductId)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var pbsResourceParameters = new PbsResourceParameters
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                TenantProvider = ItenantProvider,
                PbsProductId = pbsProductId
            };

            return Ok(new ApiOkResponse(
                await _IPbsResourceRepository.GetConsumableByProductId(pbsResourceParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CreatePbsService")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreatePbsService([FromBody] PbsServiceCreateDto pbsDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var pbsResourceParameters = new PbsResourceParameters
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                PbsServiceCreateDto = pbsDto,
                TenantProvider = ItenantProvider,
                ContextAccessor = ContextAccessor
            };

            return Ok(new ApiOkResponse(await _IPbsResourceRepository.CreatePbsService(pbsResourceParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadServiceByPbsProduct/{pbsProductId}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ReadServiceByPbsProduct(string pbsProductId)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _PbsResourceParameters = new PbsResourceParameters();

            _PbsResourceParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsResourceParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsResourceParameters.Lang = lang;
            _PbsResourceParameters.TenantProvider = ItenantProvider;
            _PbsResourceParameters.PbsProductId = pbsProductId;
            return Ok(new ApiOkResponse(await _IPbsResourceRepository.ReadServiceByPbsProduct(_PbsResourceParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("PbsResourcesEnvironment")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> PbsResourcesEnvironment([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var pbsResourceParameters = new PbsResourceParameters
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                TenantProvider = ItenantProvider
            };

            return Ok(new ApiOkResponse(await _IPbsResourceRepository.PbsResourcesEnvironment(pbsResourceParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadPbsResourcesByPbsProduct/{pbsProductId}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ReadPbsResourcesByPbsProduct(string pbsProductId)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _PbsResourceParameters = new PbsResourceParameters();

            _PbsResourceParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsResourceParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsResourceParameters.Lang = lang;
            _PbsResourceParameters.TenantProvider = ItenantProvider;
            _PbsResourceParameters.PbsProductId = pbsProductId;
            return Ok(new ApiOkResponse(
                await _IPbsResourceRepository.ReadPbsResourcesByPbsProduct(_PbsResourceParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("PbsLabourAssign")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> PbsLabourAssign([FromBody] PbsAssignedLabourDto pbsAssignedLabour)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _PbsResourceParameters = new PbsResourceParameters();

            _PbsResourceParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsResourceParameters.Lang = lang;
            _PbsResourceParameters.PbsAssignedLabour = pbsAssignedLabour;
            _PbsResourceParameters.TenantProvider = ItenantProvider;
            _PbsResourceParameters.ContextAccessor = ContextAccessor;
            _PbsResourceParameters.BorRepository = _iBorRepository;
            _PbsResourceParameters._iBorResourceRepositoryRepository = _iBorResourceRepositoryRepository;
            _PbsResourceParameters.ICoporateProductCatalogRepository = _ICoporateProductCatalogRepository;
            _PbsResourceParameters.IVPRepository = _iVpRepository;
            _PbsResourceParameters.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            _PbsResourceParameters.Configuration = _iConfiguration;
            _PbsResourceParameters.uPrinceCustomerContext = UPrinceCustomerContext;
            _PbsResourceParameters.ProjectDefinitionRepository = _iProjectDefinitionRepository; 
            return Ok(new ApiOkResponse(await _IPbsResourceRepository.PbsLabourAssign(_PbsResourceParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("PbsAssignedLabourDelete")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> PbsAssignedLabourDelete([FromBody] PbsAssignedLabourDeleteDto pbsAssignedLabourDelete)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _PbsResourceParameters = new PbsResourceParameters();

            _PbsResourceParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsResourceParameters.Lang = lang;
            _PbsResourceParameters.PbsAssignedLabourDelete = pbsAssignedLabourDelete;
            _PbsResourceParameters.TenantProvider = ItenantProvider;
            _PbsResourceParameters.ContextAccessor = ContextAccessor;
            _PbsResourceParameters.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            _PbsResourceParameters.Configuration = _iConfiguration; 
            return Ok(new ApiOkResponse(await _IPbsResourceRepository.PbsAssignedLabourDelete(_PbsResourceParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}