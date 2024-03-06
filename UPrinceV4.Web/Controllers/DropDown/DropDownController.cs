using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.PO;
using UPrinceV4.Web.Data.Stock;
using UPrinceV4.Web.Data.VisualPlan;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Controllers.DropDown;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class DropDownController : CommonConfigurationController
{
    private readonly UPrinceCustomerContex _customerContext;
    private readonly DDParameter _ddParameter;
    private readonly GraphServiceClient _graphServiceClient;
    private readonly IDropDownRepository _iDropDownRepository;
    private readonly ITenantProvider _tenantProvider;


    public DropDownController(ITenantProvider tenantProvider,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        UPrinceCustomerContex customerContext,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider,
        GraphServiceClient graphServiceClient, IDropDownRepository iDropDownRepository)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)
    {
        _iDropDownRepository = iDropDownRepository;
        _ddParameter = new DDParameter();
        _tenantProvider = tenantProvider;
        _graphServiceClient = graphServiceClient;
        _customerContext = customerContext;
    }

    [HttpPost("FilterDropDown")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> FilterDropDown([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            var result = await _iDropDownRepository.DropDownFilter(_ddParameter);
            var mApiOkResponse = new ApiOkResponse(result);


            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetProjectRoleByCode/{Code}")]
    public async Task<ActionResult> GetProjectRoleByCode(string Code, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Code = Code;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(await _iDropDownRepository.GetProjectRoleByCode(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("AddProjectRole")]
    public async Task<ActionResult> AddProjectRole(RoleDto DdDto, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.DdDto = DdDto;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(await _iDropDownRepository.AddProjectRole(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpDelete("DeleteProjectRole/{Code}")]
    public async Task<ActionResult> DeleteProjectRole(string Code, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Code = Code;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(await _iDropDownRepository.DeleteProjectRole(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetStockResourceByCode/{Code}")]
    public async Task<ActionResult> GetStockResourceByCode(string Code, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Code = Code;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(await _iDropDownRepository.GetStockResourceByCode(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("AddStockResource")]
    public async Task<ActionResult> AddStockResource(StockDropdownAddDto DdDto, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Dto = DdDto;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(await _iDropDownRepository.AddStockResource(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpDelete("DeleteStockResource/{Code}")]
    public async Task<ActionResult> DeleteStockResource(string Code, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Code = Code;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(await _iDropDownRepository.DeleteStockResource(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    [HttpGet("GetStockTypeByCode/{Code}")]
    public async Task<ActionResult> GetStockTypeByCode(string Code, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Code = Code;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(await _iDropDownRepository.GetStockTypeByCode(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("AddStockType")]
    public async Task<ActionResult> AddStockType(StockDropdownAddDto DdDto, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Dto = DdDto;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(await _iDropDownRepository.AddStockType(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpDelete("DeleteStockType/{Code}")]
    public async Task<ActionResult> DeleteStockType(string Code, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Code = Code;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(await _iDropDownRepository.DeleteStockType(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetStockStatusByCode/{Code}")]
    public async Task<ActionResult> GetStockStatusByCode(string Code, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Code = Code;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(await _iDropDownRepository.GetStockStatusByCode(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("AddStockStatus")]
    public async Task<ActionResult> AddStockStatus(StockDropdownAddDto DdDto, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Dto = DdDto;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(await _iDropDownRepository.AddStockStatus(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpDelete("DeleteStockStatus/{Code}")]
    public async Task<ActionResult> DeleteStockStatus(string Code, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Code = Code;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(await _iDropDownRepository.DeleteStockStatus(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetWFStatusByCode/{Code}")]
    public async Task<ActionResult> GetWFStatusByCode(string Code, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Code = Code;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(await _iDropDownRepository.GetWFStatusByCode(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("AddWFStatus")]
    public async Task<ActionResult> AddWFStatus(StockDropdownAddDto DdDto, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Dto = DdDto;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(await _iDropDownRepository.AddWFStatus(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpDelete("DeleteWFStatus/{Code}")]
    public async Task<ActionResult> DeleteWFStatus(string Code, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Code = Code;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(await _iDropDownRepository.DeleteWFStatus(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetWFTypeByCode/{Code}")]
    public async Task<ActionResult> GetWFTypeByCode(string Code, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Code = Code;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(await _iDropDownRepository.GetWFTypeByCode(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("AddWFType")]
    public async Task<ActionResult> AddWFType(StockDropdownAddDto DdDto, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Dto = DdDto;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(await _iDropDownRepository.AddWFType(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpDelete("DeleteWFType/{Code}")]
    public async Task<ActionResult> DeleteWFType(string Code, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Code = Code;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(await _iDropDownRepository.DeleteWFType(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetWHTypeByCode/{Code}")]
    public async Task<ActionResult> GetWHTypeByCode(string Code, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Code = Code;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(await _iDropDownRepository.GetWHTypeByCode(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("AddWHType")]
    public async Task<ActionResult> AddWHType(StockDropdownAddDto DdDto, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Dto = DdDto;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(await _iDropDownRepository.AddWHType(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpDelete("DeleteWHType/{Code}")]
    public async Task<ActionResult> DeleteWHType(string Code, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Code = Code;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(await _iDropDownRepository.DeleteWHType(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    [HttpGet("GetWHStatusByCode/{Code}")]
    public async Task<ActionResult> GetWHStatusByCode(string Code, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Code = Code;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(await _iDropDownRepository.GetWHStatusByCode(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("AddWHStatus")]
    public async Task<ActionResult> AddWHStatus(StockDropdownAddDto DdDto, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Dto = DdDto;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(await _iDropDownRepository.AddWHStatus(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpDelete("DeleteWHStatus/{Code}")]
    public async Task<ActionResult> DeleteWHStatus(string Code, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Code = Code;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(await _iDropDownRepository.DeleteWHStatus(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetWHTaxonomyLevelByCode/{Code}")]
    public async Task<ActionResult> GetWHTaxonomyLevelByCode(string Code, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Code = Code;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(await _iDropDownRepository.GetWHTaxonomyLevelByCode(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("AddWHTaxonomyLevel")]
    public async Task<ActionResult> AddWHTaxonomyLevel(StockDropdownAddDto DdDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Dto = DdDto;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(await _iDropDownRepository.AddWHTaxonomyLevel(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpDelete("DeleteWHTaxonomyLevel/{Code}")]
    public async Task<ActionResult> DeleteWHTaxonomyLevel(string Code, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Code = Code;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(await _iDropDownRepository.DeleteWHTaxonomyLevel(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetVPOrganisationShortcutPaneeByCode/{Code}")]
    public async Task<ActionResult> GetVPOrganisationShortcutPaneByCode(string Code,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Code = Code;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(
                await _iDropDownRepository.GetVPOrganisationShortcutPaneByCode(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("AddVPOrganisationShortcutPane")]
    public async Task<ActionResult> AddVPOrganisationShortcutPane(StockDropdownAddDto DdDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Dto = DdDto;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(await _iDropDownRepository.AddVPOrganisationShortcutPane(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpDelete("DeleteVPOrganisationShortcutPane/{Code}")]
    public async Task<ActionResult> DeleteVPOrganisationShortcutPane(string Code,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Code = Code;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(await _iDropDownRepository.DeleteVPOrganisationShortcutPane(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    [HttpGet("GetPOShortcutPaneByCode/{Code}")]
    public async Task<ActionResult> GetPOShortcutPaneByCode(string Code, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Code = Code;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(
                await _iDropDownRepository.GetVPOrganisationShortcutPaneByCode(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("AddPOShortcutPane")]
    public async Task<ActionResult> AddPOShortcutPane(StockDropdownAddDto DdDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Dto = DdDto;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(await _iDropDownRepository.AddVPOrganisationShortcutPane(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpDelete("DeletePOShortcutPane/{Code}")]
    public async Task<ActionResult> DeletePOShortcutPane(string Code, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Code = Code;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(await _iDropDownRepository.DeleteVPOrganisationShortcutPane(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    [HttpGet("GetOrganizationTaxonomyLevelByCode/{Code}")]
    public async Task<ActionResult> GetOrganizationTaxonomyLevelByCode(string Code,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Code = Code;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(
                new ApiOkResponse(await _iDropDownRepository.GetOrganizationTaxonomyLevelByCode(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("AddOrganizationTaxonomyLevel")]
    public async Task<ActionResult> AddOrganizationTaxonomyLevel(OrganizationTaxonomyLevelDropdown DdDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.LevelCreateDto = DdDto;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(await _iDropDownRepository.AddOrganizationTaxonomyLevel(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpDelete("DeleteOrganizationTaxonomyLevel/{Code}")]
    public async Task<ActionResult> DeleteOrganizationTaxonomyLevel(string Code,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Code = Code;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(await _iDropDownRepository.DeleteOrganizationTaxonomyLevel(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetCompetenciesTaxonomyLevelByCode/{Code}")]
    public async Task<ActionResult> GetCompetenciesTaxonomyLevelByCode(string Code,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Code = Code;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(
                new ApiOkResponse(await _iDropDownRepository.GetCompetenciesTaxonomyLevelByCode(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("AddCompetenciesTaxonomyLevel")]
    public async Task<ActionResult> AddCompetenciesTaxonomyLevel(OrganizationTaxonomyLevelDropdown DdDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.LevelCreateDto = DdDto;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(await _iDropDownRepository.AddCompetenciesTaxonomyLevel(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpDelete("DeleteCompetenciesTaxonomyLevel/{Code}")]
    public async Task<ActionResult> DeleteCompetenciesTaxonomyLevel(string Code,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Code = Code;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(await _iDropDownRepository.DeleteCompetenciesTaxonomyLevel(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetCertificationTaxonomyLevelByCode/{Code}")]
    public async Task<ActionResult> GetCertificationTaxonomyLevelByCode(string Code,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Code = Code;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(
                await _iDropDownRepository.GetCertificationTaxonomyLevelByCode(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("AddCertificationTaxonomyLevel")]
    public async Task<ActionResult> AddCertificationTaxonomyLevel(OrganizationTaxonomyLevelDropdown DdDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.LevelCreateDto = DdDto;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(await _iDropDownRepository.AddCertificationTaxonomyLevel(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpDelete("DeleteCertificationTaxonomyLevel/{Code}")]
    public async Task<ActionResult> DeleteCertificationTaxonomyLevel(string Code,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Code = Code;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(await _iDropDownRepository.DeleteCertificationTaxonomyLevel(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetPoTypeByCode/{Code}")]
    public async Task<ActionResult> GetPoTypeByCode(string Code,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Code = Code;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(
                await _iDropDownRepository.GetPoTypeByCode(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("AddPoType")]
    public async Task<ActionResult> AddPoType(CreatePOType DdDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.CreatePOType = DdDto;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(await _iDropDownRepository.AddPoType(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetPoStatusByCode/{Code}")]
    public async Task<ActionResult> GetPoStatusByCode(string Code,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.Code = Code;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(
                await _iDropDownRepository.GetPoStatusByCode(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("AddPoStatus")]
    public async Task<ActionResult> AddPoStatus(CreatePOStatus DdDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _ddParameter.ContractingUnitSequenceId = CU;
            _ddParameter.ProjectSequenceId = Project;
            _ddParameter.Lang = lang;
            _ddParameter.CreatePOStatus = DdDto;
            _ddParameter.ContextAccessor = ContextAccessor;
            _ddParameter.TenantProvider = _tenantProvider;
            return Ok(new ApiOkResponse(await _iDropDownRepository.AddPoStatus(_ddParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}