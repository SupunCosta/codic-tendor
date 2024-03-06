using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.WH;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Controllers.WH;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class WareHouseController : CommonConfigurationController
{
    private readonly IWareHouseRepository _iWareHouseRepository;
    private readonly ITenantProvider _TenantProvider;
    private readonly WHParameter _whParameter;

    public WareHouseController(ITenantProvider tenantProvider,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider
        , IWareHouseRepository iWareHouseRepository)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)

    {
        _iWareHouseRepository = iWareHouseRepository;
        _whParameter = new WHParameter();
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

            _whParameter.ContractingUnitSequenceId = CU;
            _whParameter.ProjectSequenceId = Project;
            _whParameter.Lang = lang;
            _whParameter.ContextAccessor = ContextAccessor;
            _whParameter.TenantProvider = _TenantProvider;
            return Ok(new ApiOkResponse(await _iWareHouseRepository.GetShortcutpaneData(_whParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("FilterWH")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetWHList([FromBody] WHFilter filter,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _whParameter.ContractingUnitSequenceId = CU;
            _whParameter.ProjectSequenceId = Project;
            _whParameter.Lang = lang;
            _whParameter.Filter = filter;
            _whParameter.ContextAccessor = ContextAccessor;
            _whParameter.TenantProvider = _TenantProvider;
            var result = await _iWareHouseRepository.GetWHList(_whParameter);
            var mApiOkResponse = new ApiOkResponse(result);


            if (!result.Any())
            {
                var mApiResponse = new ApiOkResponse(null, "noAvailableWareHouse");
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
    public async Task<ActionResult> CreateHeader([FromBody] WHCreateDto WHDto,
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

            _whParameter.ContractingUnitSequenceId = CU;
            _whParameter.ProjectSequenceId = Project;
            _whParameter.Lang = lang;
            _whParameter.WHDto = WHDto;
            _whParameter.ContextAccessor = ContextAccessor;
            _whParameter.TenantProvider = ItenantProvider;
            _whParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var s = await _iWareHouseRepository.CreateHeader(_whParameter);
            return Ok(new ApiOkResponse(s));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CreateWHTaxonomy")]
    public async Task<ActionResult> CreateWHTaxonomy(
        [FromBody] WHTaxonomyCreateDto WHTaxonomyDto, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
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

            _whParameter.ContractingUnitSequenceId = CU;
            _whParameter.ProjectSequenceId = Project;
            _whParameter.Lang = lang;
            _whParameter.WHTaxonomyDto = WHTaxonomyDto;
            _whParameter.ContextAccessor = ContextAccessor;
            _whParameter.TenantProvider = ItenantProvider;
            _whParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var s = await _iWareHouseRepository.CreateWHTaxonomy(_whParameter);
            return Ok(new ApiOkResponse(s));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetWHDropdown")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetWHDropdown([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _whParameter.ContractingUnitSequenceId = CU;
            _whParameter.ProjectSequenceId = Project;
            _whParameter.Lang = lang;

            _whParameter.TenantProvider = ItenantProvider;
            return Ok(new ApiOkResponse(await _iWareHouseRepository.GetWHDropdown(_whParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    [HttpGet("GetById/{SequenceId}")]
    public async Task<ActionResult> GetWHById(string SequenceId, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _whParameter.ContractingUnitSequenceId = CU;
            _whParameter.ProjectSequenceId = Project;
            _whParameter.Lang = lang;
            _whParameter.Id = SequenceId;
            _whParameter.ContextAccessor = ContextAccessor;
            _whParameter.TenantProvider = ItenantProvider;
            return Ok(new ApiOkResponse(await _iWareHouseRepository.GetWHById(_whParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("GetWHTaxonomyList")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetWHTaxonomyList(
        [FromBody] WHTaxonomyFilterDto taxonomyfilter, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _whParameter.ContractingUnitSequenceId = CU;
            _whParameter.ProjectSequenceId = Project;
            _whParameter.Lang = lang;
            _whParameter.WHTaxonomyFilter = taxonomyfilter;
            _whParameter.ContextAccessor = ContextAccessor;
            _whParameter.TenantProvider = _TenantProvider;
            var result = await _iWareHouseRepository.GetWHTaxonomyList(_whParameter);
            var mApiOkResponse = new ApiOkResponse(result);


            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    [HttpGet("GetWHRockCpcById/{Id}")]
    public async Task<ActionResult> GetWHRockCpcById(string Id, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _whParameter.ContractingUnitSequenceId = CU;
            _whParameter.ProjectSequenceId = Project;
            _whParameter.Lang = lang;
            _whParameter.Id = Id;
            _whParameter.ContextAccessor = ContextAccessor;
            _whParameter.TenantProvider = ItenantProvider;
            //return Ok(new ApiOkResponse(await _iWareHouseRepository.GetWHRockCpcById(_whParameter)));

            var result = await _iWareHouseRepository.GetWHRockCpcById(_whParameter);


            if (result == null)
            {
                var mApiResponse = new ApiOkResponse(null, "nostock")
                {
                    Status = false
                };
                return Ok(mApiResponse);
            }

            var mApiOkResponse = new ApiOkResponse(result);

            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("SaveWHRockCpc")]
    public async Task<ActionResult> SaveWHRockCpc([FromBody] WHRockCpcDto WHRockCpcDto,
        [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _whParameter.ContractingUnitSequenceId = CU;
            _whParameter.ProjectSequenceId = Project;
            _whParameter.WHRockCpcDto = WHRockCpcDto;
            _whParameter.Lang = lang;
            _whParameter.ContextAccessor = ContextAccessor;
            _whParameter.TenantProvider = ItenantProvider;
            _whParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            return Ok(new ApiOkResponse(await _iWareHouseRepository.SaveWHRockCpc(_whParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("GetWHRockCpcList")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetWHRockCpcList(
        [FromBody] WHRockCpcFilter WHRockCpcFilter, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _whParameter.ContractingUnitSequenceId = CU;
            _whParameter.ProjectSequenceId = Project;
            _whParameter.Lang = lang;
            _whParameter.WHRockCpcFilter = WHRockCpcFilter;
            _whParameter.ContextAccessor = ContextAccessor;
            _whParameter.TenantProvider = _TenantProvider;
            _whParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var result = await _iWareHouseRepository.GetWHRockCpcList(_whParameter);
            var mApiOkResponse = new ApiOkResponse(result);


            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("UploadImageForMobile")]
    public async Task<ActionResult<string>> UploadImageForMobile([FromForm] IFormCollection image)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            _whParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _whParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _whParameter.Lang = Request.Headers["lang"].FirstOrDefault();
            _whParameter.ContextAccessor = ContextAccessor;
            _whParameter.TenantProvider = ItenantProvider;
            _whParameter.formData = image;
            var id = await _iWareHouseRepository.UploadImageForMobile(_whParameter);
            return Ok(new ApiOkResponse(id, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}