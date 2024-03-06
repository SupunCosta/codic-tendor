using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.BOR;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Data.PMOL;
using UPrinceV4.Web.Data.VisualPlaane;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces.BOR;
using UPrinceV4.Web.Repositories.Interfaces.CPC;
using UPrinceV4.Web.Repositories.Interfaces.PBS;
using UPrinceV4.Web.Repositories.Interfaces.PMOL;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Controllers.PBS;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class PbsController : CommonConfigurationController
{
    // private readonly BorParameter _borParameter;
    // private readonly CpcParameters _CpcParameters;
    private readonly IBorRepository _iBorRepository;
    private readonly IBorResourceRepository _iBorResourceRepository;
    private readonly ICoporateProductCatalogRepository _iCoporateProductCatalogRepository;
    private readonly IPbsRepository _IPbsRepository;
    private readonly IPbsResourceRepository _IPbsResourceRepository;
    private readonly IPmolRepository _iPmolRepository;
    private readonly IPmolResourceRepository _iPmolResourceRepository;
    private readonly PmolParameter _pmolParameter;


    public PbsController(IPbsRepository iPbsRepository,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse,
        ITenantProvider iTenantProvider,
        IPbsResourceRepository iPbsResourceRepository, IBorRepository iBorRepository, BorParameter BorParameter,
        IBorResourceRepository iBorResourceRepository, PmolParameter PmolParameter, IPmolRepository iPmolRepository,
        ICoporateProductCatalogRepository _CoporateProductCatalogRepository, CpcParameters CpcParameters,
        IConfiguration iConfiguration, IPmolResourceRepository iPmolResourceRepository)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)

    {
        _IPbsRepository = iPbsRepository;
        _IPbsResourceRepository = iPbsResourceRepository;
        _iBorRepository = iBorRepository;
        //_borParameter = BorParameter;
        _iBorResourceRepository = iBorResourceRepository;
        _iCoporateProductCatalogRepository = _CoporateProductCatalogRepository;
        //_CpcParameters = CpcParameters;
        _iPmolRepository = iPmolRepository;
        _pmolParameter = PmolParameter;
        _iConfiguration = iConfiguration;
        _iPmolResourceRepository = iPmolResourceRepository;
    }

    private IConfiguration _iConfiguration { get; }

    [HttpGet("ReadPbsShortcutPaneData")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ReadPbsShortcutPaneData()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _PbsParameters = new PbsParameters();
            _PbsParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsParameters.Lang = lang;
            _PbsParameters.TenantProvider = ItenantProvider;
            return Ok(new ApiOkResponse(await _IPbsRepository.getShortcutPaneData(_PbsParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("Create")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreatePbs([FromBody] PbsProductCreateDto pbsDto)
    {
        try
        {
            var _PbsParameters = new PbsParameters
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault()
            };

            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var user = UPrinceCustomerContext.ApplicationUser?.Where(u => u.OId == objectIdentifier)
                .FirstOrDefault();
            _PbsParameters.ChangedUser = user;
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            _PbsParameters.Lang = lang;
            _PbsParameters.PbsDto = pbsDto;
            _PbsParameters.ContextAccessor = ContextAccessor;
            _PbsParameters.TenantProvider = ItenantProvider;
            _PbsParameters.IPbsResourceRepository = _IPbsResourceRepository;
            return Ok(new ApiOkResponse(await _IPbsRepository.CreatePbs(_PbsParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CreateNew")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreatePbsNew([FromBody] PbsProductCreateDto pbsDto)
    {
        try
        {
            var _PbsParameters = new PbsParameters();

            _PbsParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var user = UPrinceCustomerContext.ApplicationUser?.Where(u => u.OId == objectIdentifier)
                .FirstOrDefault();
            _PbsParameters.ChangedUser = user;
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            _PbsParameters.Lang = lang;
            _PbsParameters.PbsDto = pbsDto;
            _PbsParameters.ContextAccessor = ContextAccessor;
            _PbsParameters.TenantProvider = ItenantProvider;
            return Ok(new ApiOkResponse(await _IPbsRepository.CreatePbsNew(_PbsParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    [HttpGet("GetPbsDropdownData")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetPbsDropdownData()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _PbsParameters = new PbsParameters();

            _PbsParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsParameters.Lang = lang;
            _PbsParameters.TenantProvider = ItenantProvider;
            return Ok(new ApiOkResponse(await _IPbsRepository.GetPbsDropdown(_PbsParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadTreeStructureData")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetTreeStructureData()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _PbsParameters = new PbsParameters();

            _PbsParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsParameters.Lang = lang;
            _PbsParameters.TenantProvider = ItenantProvider;
            return Ok(new ApiOkResponse(await _IPbsRepository.GetTreeStructureData(_PbsParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpGet("Read/{productId}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetPbsById(string productId)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _PbsParameters = new PbsParameters();

            _PbsParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsParameters.Lang = lang;
            _PbsParameters.Id = productId;
            _PbsParameters.ContextAccessor = ContextAccessor;
            _PbsParameters.TenantProvider = ItenantProvider;
            _PbsParameters.IPbsResourceRepository = _IPbsResourceRepository;
            return Ok(new ApiOkResponse(await _IPbsRepository.GetPbsById(_PbsParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpDelete("Delete")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeletePbs([FromBody] List<string> idList)
    {
        try
        {
            var lang = Request.Headers["lang"].FirstOrDefault();
            var _PbsParameters = new PbsParameters();

            _PbsParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsParameters.Lang = lang;
            _PbsParameters.IdList = idList;
            _PbsParameters.TenantProvider = ItenantProvider;
            await _IPbsRepository.DeletePbs(_PbsParameters);

            return Ok(new ApiOkResponse("ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadTaxonomyLevels")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetTaxonomyLevels()
    {
        try
        {
            var lang = Request.Headers["lang"].FirstOrDefault();
            var _PbsParameters = new PbsParameters();

            _PbsParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsParameters.Lang = lang;
            _PbsParameters.TenantProvider = ItenantProvider;
            return Ok(new ApiOkResponse(await _IPbsRepository.GetTaxonomyLevels(_PbsParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CreateNode")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateNode([FromBody] PbsProductNodeCreateDto pbsDto)
    {
        try
        {
            var _PbsParameters = new PbsParameters();

            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var user = UPrinceCustomerContext.ApplicationUser?.Where(u => u.OId == objectIdentifier)
                .FirstOrDefault();
            _PbsParameters.ChangedUser = user;

            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = Request.Headers["lang"].FirstOrDefault();
            _PbsParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsParameters.Lang = lang;
            _PbsParameters.nodeDto = pbsDto;
            _PbsParameters.ContextAccessor = ContextAccessor;
            _PbsParameters.TenantProvider = ItenantProvider;
            return Ok(new ApiOkResponse(await _IPbsRepository.CreateNode(_PbsParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("FilterProductByTaxonomyLevel")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetProductByTaxonomyLevel(
        [FromBody] PbsFilterByTaxonomyLevel filter)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _PbsParameters = new PbsParameters();

            _PbsParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsParameters.Lang = lang;
            _PbsParameters.TenantProvider = ItenantProvider;
            _PbsParameters.taxonomyLevelFilter = filter;
            var pbs = await _IPbsRepository.GetProductByTaxonomyLevel(_PbsParameters);

            if (!pbs.Any())
            {
                var mApiResponse = new ApiOkResponse(null, "noavailableprojectbreakdownstructure")
                {
                    Status = false
                };
                return Ok(mApiResponse);
            }

            return Ok(new ApiOkResponse(pbs));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    
    [HttpPut("UpdateNodeName")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateNodeName([FromBody] PbsProductCreateDto pbsDto)
    {
        try
        {
            var _PbsParameters = new PbsParameters();

            _PbsParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var user = UPrinceCustomerContext.ApplicationUser?.Where(u => u.OId == objectIdentifier)
                .FirstOrDefault();
            _PbsParameters.ChangedUser = user;
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = Request.Headers["lang"].FirstOrDefault();
            _PbsParameters.Lang = lang;
            _PbsParameters.PbsDto = pbsDto;
            _PbsParameters.ContextAccessor = ContextAccessor;
            _PbsParameters.TenantProvider = ItenantProvider;
            return Ok(new ApiOkResponse(await _IPbsRepository.CreatePbs(_PbsParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    [HttpPost("Clone")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ClonePbs([FromBody] PbsCloneDto pbsCloneDto)
    {
        try
        {
            var _PbsParameters = new PbsParameters();

            _PbsParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var user = UPrinceCustomerContext.ApplicationUser?.Where(u => u.OId == objectIdentifier)
                .FirstOrDefault();
            _PbsParameters.ChangedUser = user;
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            _PbsParameters.Lang = lang;
            _PbsParameters.PbsCloneDto = pbsCloneDto;
            _PbsParameters.ContextAccessor = ContextAccessor;
            _PbsParameters.TenantProvider = ItenantProvider;
            _PbsParameters.IPbsResourceRepository = _IPbsResourceRepository;
            return Ok(new ApiOkResponse(await _IPbsRepository.ClonePbs(_PbsParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("GetUtilityTaxonomyForProjectPlanning")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetUtilityTaxonomyForProjectPlanning([FromBody] PbsTreeStructureFilter PbsFilter)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _PbsParameters = new PbsParameters();

            _PbsParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsParameters.Lang = lang;
            _PbsParameters.TenantProvider = ItenantProvider;
            _PbsParameters.PbsTreeStructureFilter = PbsFilter;
            return Ok(new ApiOkResponse(
                await _IPbsRepository.GetUtilityTaxonomyForProjectPlanning(_PbsParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("GetLocationTaxonomyForProjectPlanning")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetLocationTaxonomyForProjectPlanning([FromBody] PbsTreeStructureFilter PbsFilter)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _PbsParameters = new PbsParameters();

            _PbsParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsParameters.Lang = lang;
            _PbsParameters.TenantProvider = ItenantProvider;
            _PbsParameters.PbsTreeStructureFilter = PbsFilter;

            return Ok(
                new ApiOkResponse(await _IPbsRepository.GetLocationTaxonomyForProjectPlanning(_PbsParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("ExcelUpload")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ExcelUpload([FromBody] List<UploadExcelDto> uploadExcelDto)
    {
        try
        {
            var _PbsParameters = new PbsParameters();

            _PbsParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var user = UPrinceCustomerContext.ApplicationUser?.Where(u => u.OId == objectIdentifier)
                .FirstOrDefault();
            _PbsParameters.ChangedUser = user;
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            _PbsParameters.Lang = lang;
            _PbsParameters.UploadExcelDto = uploadExcelDto;
            _PbsParameters.ContextAccessor = ContextAccessor;
            _PbsParameters.TenantProvider = ItenantProvider;
            _PbsParameters.IPbsResourceRepository = _IPbsResourceRepository;
            return Ok(new ApiOkResponse(await _IPbsRepository.ExcelUpload(_PbsParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("GetMachineTaxonomyForProjectPlanning")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetMachineTaxonomyForProjectPlanning([FromBody] PbsTreeStructureFilter PbsFilter)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _PbsParameters = new PbsParameters();

            _PbsParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsParameters.Lang = lang;
            _PbsParameters.TenantProvider = ItenantProvider;
            _PbsParameters.PbsTreeStructureFilter = PbsFilter;

            return Ok(
                new ApiOkResponse(await _IPbsRepository.GetMachineTaxonomyForProjectPlanning(_PbsParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CreatePbsScopeOfWork")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreatePbsScopeOfWork([FromBody] PbsScopeOfWork PbsSquareMeters)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _PbsParameters = new PbsParameters();

            _PbsParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsParameters.Lang = lang;
            _PbsParameters.TenantProvider = ItenantProvider;
            _PbsParameters.PbsSquareMeters = PbsSquareMeters;

            return Ok(
                new ApiOkResponse(await _IPbsRepository.CreatePbsScopeOfWork(_PbsParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetPbsLabour/{Id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetPbsLabour(string Id)
    {
        try
        {
            var lang = Request.Headers["lang"].FirstOrDefault();
            var _PbsParameters = new PbsParameters();

            _PbsParameters.Id = Id;
            _PbsParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsParameters.Lang = lang;
            _PbsParameters.TenantProvider = ItenantProvider;
            return Ok(new ApiOkResponse(await _IPbsRepository.GetPbsLabour(_PbsParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CreatePmol2")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreatePmol2([FromBody] CreatePmolDto createPmolDto)
    {
        try
        {
            var pmolList = new List<Pmol>();

            foreach (var i in createPmolDto.Cpc)
            {
                var oid = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
                var userId = UPrinceCustomerContext.ApplicationUser.First(u => u.OId == oid).OId;
                var lang = langCode(Request.Headers["lang"].FirstOrDefault());

                var connectionString = ConnectionString.MapConnectionString(Request.Headers["CU"].FirstOrDefault(),
                    Request.Headers["Project"].FirstOrDefault(), ItenantProvider);

                using (var connection = new SqlConnection(connectionString))
                {
                    var pbsLabour = connection
                        .Query<LabourForPbs>(
                            "SELECT * FROM dbo.PbsLabour WHERE PbsProductId = @ProductId AND CoperateProductCatalogId = @CpcId",
                            new
                                { i.ProductId, i.CpcId }).FirstOrDefault();

                    var borResourceCreateDto = new BorResourceCreateDto
                    {
                        Id = pbsLabour.CoperateProductCatalogId,
                        Required = pbsLabour.Quantity,
                        IsNew = true,
                        Environment = "local"
                    };

                    var labour = new List<BorResourceCreateDto>();

                    labour.Add(borResourceCreateDto);

                    var borResources = new BorResource
                    {
                        Labour = labour
                    };

                    var product = connection
                        .Query<PbsProduct>("SELECT * FROM dbo.PbsProduct WHERE Id = @ProductId;", new { i.ProductId })
                        .FirstOrDefault();

                    var pbsproduct = new BorProductDto
                    {
                        Id = product.Id,
                        ProductId = product.ProductId
                    };

                    var borDto = new BorDto
                    {
                        Id = Guid.NewGuid().ToString(),
                        BorStatusId = "d60aad0b-2e84-482b-ad25-618d80d49477",
                        BorTypeId = "88282458-0b40-poa3-b0f9-c2e40344c888",
                        Name = product.Name,
                        Product = pbsproduct,
                        BorResources = borResources,
                        CId = pbsLabour.CoperateProductCatalogId,
                        WeekPlan = true
                    };
                    var _borParameter = new BorParameter();
                    _borParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
                    _borParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
                    _borParameter.Lang = lang;
                    _borParameter.ContextAccessor = ContextAccessor;
                    _borParameter.TenantProvider = ItenantProvider;
                    _borParameter.BorDto = borDto;
                    _borParameter.IBorResourceRepository = _iBorResourceRepository;
                    _borParameter.ICoporateProductCatalogRepository = _iCoporateProductCatalogRepository;
                    var _CpcParameters = new CpcParameters();
                    _CpcParameters.Oid = userId;
                    _borParameter.CpcParameters = _CpcParameters;
                    var borItemId = await _iBorRepository.CreateBor(_borParameter);
                    _borParameter.Id = borItemId;
                    var bor = await _iBorRepository.GetBorById(_borParameter);

                    var pmolDto = new PmolCreateDto
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductId = product.ProductId,
                        Name = product.Name,
                        TypeId = "848e5e-622d-4783-95e6-4092004eb5eaff",
                        StatusId = "d60aad0b-2e84-482b-ad25-618d80d49477",
                        IsFinished = false,
                        PmolType = "regular",
                        Bor = bor
                    };

                    _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
                    _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
                    _pmolParameter.Lang = lang;
                    _pmolParameter.PmolDto = pmolDto;
                    _pmolParameter.ContextAccessor = ContextAccessor;
                    _pmolParameter.TenantProvider = ItenantProvider;
                    _pmolParameter.IPmolResourceRepository = _iPmolResourceRepository;
                    _pmolParameter.Configuration = _iConfiguration;
                    _pmolParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                        claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
                    var pmol = await _iPmolRepository.CreateHeader(_pmolParameter, false);
                    pmolList.Add(pmol);
                }
            }

            return Ok(new ApiOkResponse(pmolList));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CreatePmol")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreatePmol([FromBody] CreatePmolDto createPmolDto)
    {
        try
        {
            var pmolList = new List<Pmol>();

            if (!createPmolDto.IsRepeat)
            {
                createPmolDto.StartDate = DateTime.UtcNow.Date;
                createPmolDto.EndDate = DateTime.UtcNow.Date;
            }

            for (var k = createPmolDto.StartDate; k <= createPmolDto.EndDate; k = k?.AddDays(1))
                //if (k?.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday) continue;
                foreach (var i in createPmolDto.Cpc)
                {
                    var oid = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                        claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
                    var userId = UPrinceCustomerContext.ApplicationUser.First(u => u.OId == oid).OId;
                    var lang = langCode(Request.Headers["lang"].FirstOrDefault());

                    var connectionString = ConnectionString.MapConnectionString(Request.Headers["CU"].FirstOrDefault(),
                        Request.Headers["Project"].FirstOrDefault(), ItenantProvider);

                    using (var connection = new SqlConnection(connectionString))
                    {
                        var pbsResource = i.ResourceTypeId switch
                        {
                            //labours
                            "c46c3a26-39a5-42cc-b07s-89655304eh6" => connection
                                .Query<LabourForPbs>(
                                    "SELECT * FROM dbo.PbsLabour WHERE PbsProductId = @ProductId AND CoperateProductCatalogId = @CpcId",
                                    new { i.ProductId, i.CpcId })
                                .FirstOrDefault(),
                            //materials
                            "c46c3a26-39a5-42cc-n7k1-89655304eh6" => connection
                                .Query<LabourForPbs>(
                                    "SELECT * FROM dbo.PbsMaterial WHERE PbsProductId = @ProductId AND CoperateProductCatalogId = @CpcId",
                                    new { i.ProductId, i.CpcId })
                                .FirstOrDefault(),
                            //tools
                            "c46c3a26-39a5-42cc-n9wn-89655304eh6" => connection
                                .Query<LabourForPbs>(
                                    "SELECT * FROM dbo.PbsTools WHERE PbsProductId = @ProductId AND CoperateProductCatalogId = @CpcId",
                                    new { i.ProductId, i.CpcId })
                                .FirstOrDefault(),
                            //consumables
                            "c46c3a26-39a5-42cc-m06g-89655304eh6" => connection
                                .Query<LabourForPbs>(
                                    "SELECT * FROM dbo.PbsConsumable WHERE PbsProductId = @ProductId AND CoperateProductCatalogId = @CpcId",
                                    new { i.ProductId, i.CpcId })
                                .FirstOrDefault(),
                            _ => null
                        };

                        if (pbsResource != null)
                        {
                            var borResourceCreateDto = new BorResourceCreateDto
                            {
                                Id = pbsResource.CoperateProductCatalogId,
                                Required = pbsResource.Quantity,
                                IsNew = true,
                                Environment = "local"
                            };

                            var labour = new List<BorResourceCreateDto>();
                            var tools = new List<BorResourceCreateDto>();
                            var materials = new List<BorResourceCreateDto>();
                            var consumables = new List<BorResourceCreateDto>();


                            if (i.ResourceTypeId == "c46c3a26-39a5-42cc-b07s-89655304eh6") //labour
                                labour.Add(borResourceCreateDto);
                            else if (i.ResourceTypeId == "c46c3a26-39a5-42cc-n7k1-89655304eh6") //materials
                                materials.Add(borResourceCreateDto);
                            else if (i.ResourceTypeId == "c46c3a26-39a5-42cc-n9wn-89655304eh6") //tools
                                tools.Add(borResourceCreateDto);
                            else if (i.ResourceTypeId == "c46c3a26-39a5-42cc-m06g-89655304eh6") //consumables
                                consumables.Add(borResourceCreateDto);


                            var borResources = new BorResource
                            {
                                Labour = labour,
                                Consumable = consumables,
                                Tools = tools,
                                Materials = materials
                            };

                            var product = connection
                                .Query<PbsProduct>("SELECT * FROM dbo.PbsProduct WHERE Id = @ProductId;",
                                    new { i.ProductId }).FirstOrDefault();

                            var pbsproduct = new BorProductDto
                            {
                                Id = product.Id,
                                ProductId = product.ProductId
                            };

                            var borDto = new BorDto
                            {
                                Id = Guid.NewGuid().ToString(),
                                BorStatusId = "d60aad0b-2e84-482b-ad25-618d80d49477",
                                BorTypeId = "88282458-0b40-poa3-b0f9-c2e40344c888",
                                Name = product.Name,
                                Product = pbsproduct,
                                BorResources = borResources,
                                CId = pbsResource.CoperateProductCatalogId,
                                WeekPlan = true
                            };
                            var _borParameter = new BorParameter
                            {
                                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                                Lang = lang,
                                ContextAccessor = ContextAccessor,
                                TenantProvider = ItenantProvider,
                                BorDto = borDto,
                                IBorResourceRepository = _iBorResourceRepository,
                                ICoporateProductCatalogRepository = _iCoporateProductCatalogRepository
                            };
                            var _CpcParameters = new CpcParameters
                            {
                                Oid = userId
                            };
                            _borParameter.CpcParameters = _CpcParameters;
                            var borItemId = await _iBorRepository.CreateBor(_borParameter);
                            _borParameter.Id = borItemId;
                            var bor = await _iBorRepository.GetBorById(_borParameter);

                            DateTime? executeDate = null;
                            if (createPmolDto.IsRepeat) executeDate = k;

                            var pmolDto = new PmolCreateDto
                            {
                                Id = Guid.NewGuid().ToString(),
                                ProductId = product.ProductId,
                                Name = product.Name,
                                TypeId = "848e5e-622d-4783-95e6-4092004eb5eaff",
                                StatusId = "d60aad0b-2e84-482b-ad25-618d80d49477",
                                IsFinished = false,
                                PmolType = "regular",
                                Bor = bor,
                                ExecutionDate = executeDate,
                                ExecutionStartTime = "05:00",
                                ExecutionEndTime = "14:00"
                            };

                            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
                            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
                            _pmolParameter.Lang = lang;
                            _pmolParameter.PmolDto = pmolDto;


                            _pmolParameter.ContextAccessor = ContextAccessor;
                            _pmolParameter.TenantProvider = ItenantProvider;
                            _pmolParameter.IPmolResourceRepository = _iPmolResourceRepository;
                            _pmolParameter.Configuration = _iConfiguration;
                            _pmolParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(
                                    claim =>
                                        claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")
                                .Value;
                            var pmol = await _iPmolRepository.CreateHeader(_pmolParameter, false);
                            pmolList.Add(pmol);
                        }
                    }
                }

            return Ok(new ApiOkResponse(pmolList));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetAllPbsLabour")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetAllPbsLabour()
    {
        try
        {
            var lang = Request.Headers["lang"].FirstOrDefault();
            var _PbsParameters = new PbsParameters();

            _PbsParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsParameters.Lang = lang;
            _PbsParameters.TenantProvider = ItenantProvider;
            return Ok(new ApiOkResponse(await _IPbsRepository.GetAllPbsLabour(_PbsParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("GetAllPmolLabour")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetAllPmolLabour([FromBody] GetPmolLabourDto GetPmolLabourDto)
    {
        try
        {
            var lang = Request.Headers["lang"].FirstOrDefault();
            var _PbsParameters = new PbsParameters();

            _PbsParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsParameters.Lang = lang;
            _PbsParameters.GetPmolLabourDto = GetPmolLabourDto;
            _PbsParameters.TenantProvider = ItenantProvider;
            return Ok(new ApiOkResponse(await _IPbsRepository.GetAllPmolLabour(_PbsParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpGet("GetPbsRelations/{Id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetPbsRelations(string Id)
    {
        try
        {
            var lang = Request.Headers["lang"].FirstOrDefault();
            var _PbsParameters = new PbsParameters();

            _PbsParameters.Id = Id;
            _PbsParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsParameters.Lang = lang;
            _PbsParameters.TenantProvider = ItenantProvider;
            return Ok(new ApiOkResponse(await _IPbsRepository.GetPbsRelations(_PbsParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("GetCpcRelations")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetCpcRelations([FromBody] CpcRelationsDto cpcRelationsDto)
    {
        try
        {
            var lang = Request.Headers["lang"].FirstOrDefault();
            var _PbsParameters = new PbsParameters();

            _PbsParameters.CpcRelationsDto = cpcRelationsDto;
            _PbsParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsParameters.Lang = lang;
            _PbsParameters.TenantProvider = ItenantProvider;
            return Ok(new ApiOkResponse(await _IPbsRepository.GetCpcRelations(_PbsParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("AddPbsCbcResource")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> AddPbsCbcResource([FromBody] PbsCbcResources dto)
    {
        try
        {
            var lang = Request.Headers["lang"].FirstOrDefault();
            var _PbsParameters = new PbsParameters();

            _PbsParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsParameters.Lang = lang;
            _PbsParameters.PbsCbcResourcesDto = dto;
            _PbsParameters.TenantProvider = ItenantProvider;
            return Ok(new ApiOkResponse(await _IPbsRepository.AddPbsCbcResource(_PbsParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpDelete("DeletePbsCbcResource")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeletePbsCbcResource([FromBody] List<string> idList)
    {
        try
        {
            var lang = Request.Headers["lang"].FirstOrDefault();
            var _PbsParameters = new PbsParameters();

            _PbsParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsParameters.Lang = lang;
            _PbsParameters.IdList = idList;
            _PbsParameters.TenantProvider = ItenantProvider;
            return Ok(new ApiOkResponse(await _IPbsRepository.DeletePbsCbcResource(_PbsParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpGet("GetPbsCbcResourcesById/{Id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetPbsCbcResourcesById(string id)
    {
        try
        {
            var lang = Request.Headers["lang"].FirstOrDefault();
            var _PbsParameters = new PbsParameters();

            _PbsParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsParameters.Lang = lang;
            _PbsParameters.Id = id;
            _PbsParameters.TenantProvider = ItenantProvider;
            return Ok(new ApiOkResponse(await _IPbsRepository.GetPbsCbcResourcesById(_PbsParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("GetPbsCbcResourcesByIdForMyCal")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetPbsCbcResourcesByIdForMyCal([FromBody] MyCalCreatePmolDto dto)
    {
        try
        {
            var lang = Request.Headers["lang"].FirstOrDefault();
            var _PbsParameters = new PbsParameters
            {
                ContractingUnitSequenceId = dto.ContractingUnitId,
                ProjectSequenceId = dto.ProjectSequenceId,
                Lang = lang,
                MyCalCreatePmolDto = dto,
                TenantProvider = ItenantProvider,
                Id = dto.PbsId,
                Title = dto.Title
            };

            return Ok(new ApiOkResponse(await _IPbsRepository.GetPbsCbcResourcesById(_PbsParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpGet("GetPmolDeliverablesByPbsId/{Id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetPmolDeliverablesByPbsId(string id)
    {
        try
        {
            var lang = Request.Headers["lang"].FirstOrDefault();
            var _PbsParameters = new PbsParameters();

            _PbsParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsParameters.Lang = lang;
            _PbsParameters.Id = id;
            _PbsParameters.TenantProvider = ItenantProvider;
            return Ok(new ApiOkResponse(await _IPbsRepository.GetPmolDeliverablesByPbsId(_PbsParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpGet("GetPbsLotIdById/{Id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetPbsLotIdById(string id)
    {
        try
        {
            var lang = Request.Headers["lang"].FirstOrDefault();
            var _PbsParameters = new PbsParameters();

            _PbsParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsParameters.Lang = lang;
            _PbsParameters.Id = id;
            _PbsParameters.TenantProvider = ItenantProvider;
            return Ok(new ApiOkResponse(await _IPbsRepository.GetPbsLotIdById(_PbsParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
}