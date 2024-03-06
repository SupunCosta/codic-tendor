using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.VisualPlaane;
using UPrinceV4.Web.Data.VisualPlan;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Repositories.Interfaces.CAB;
using UPrinceV4.Web.Repositories.Interfaces.PMOL;

namespace UPrinceV4.Web.Controllers.VP;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class OrganizationSettingsController : CommonConfigurationController
{
    private readonly ICompanyRepository _iCompanyRepository;
    private readonly IOrganizationSettingsRepository _iOrganizationSettingsRepository;
    private readonly IPersonRepository _iPersonRepository;
    //private readonly OSParameter _osParameter;
    private readonly ITenantProvider _TenantProvider;


    public OrganizationSettingsController(ITenantProvider tenantProvider,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApplicationDbContext dbContext,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, PmolParameter PmolParameter,
        ILogger<OrganizationSettingsController> logger, ITenantProvider iTenantProvider
        , IOrganizationSettingsRepository iOrganizationSettingsRepository)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)

    {
        _iOrganizationSettingsRepository = iOrganizationSettingsRepository;
       // _osParameter = new OSParameter();
        _TenantProvider = tenantProvider;
    }

    [HttpPost("CreateOrganizationCompetencies")]
    public async Task<ActionResult> CreateOrganizationCompetencies(
        [FromBody] OrganizationCompetenciesCreate organizationCompetenciesCreate,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var osParameter = new OSParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = _TenantProvider,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                OrganizationCompetenciesCreate = organizationCompetenciesCreate
            };
            var result = await _iOrganizationSettingsRepository.CreateOrganizationCompetencies(osParameter);
            return Ok(new ApiOkResponse(result, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetOrganizationCompetenceList")]
    public async Task<ActionResult> GetOrganizationCompetenceList([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)

    {
        try
        {
            var lang = langCode(langX);

            var _osParameter = new OSParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = _TenantProvider
            };

            var data = await _iOrganizationSettingsRepository.GetOrganizationCompetenceList(_osParameter);


            return Ok(new ApiOkResponse(data));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("OrganizationCompetenceGetById/{Id}")]
    public async Task<ActionResult> OrganizationCompetenceGetById(string Id, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            var _osParameter = new OSParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = _TenantProvider,
                Id = Id
            };

            var data = await _iOrganizationSettingsRepository.OrganizationCompetenceGetById(_osParameter);


            return Ok(new ApiOkResponse(data));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetOrganizationCompetenceDropdown")]
    
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetOrganizationCompetenceDropdown([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            var _osParameter = new OSParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                TenantProvider = ItenantProvider
            };

            return Ok(new ApiOkResponse(
                await _iOrganizationSettingsRepository.GetOrganizationCompetenceDropdown(_osParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CreateCompetenceTaxonomy")]
    public async Task<ActionResult> CreateCompetenceTaxonomy(
        [FromBody] CompetenciesTaxonomy CompetenceTaxonomyDto,
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

            var _osParameter = new OSParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                CompetenciesTaxonomyDto = CompetenceTaxonomyDto,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };

            var s = await _iOrganizationSettingsRepository.CreateCompetenceTaxonomy(_osParameter);
            return Ok(new ApiOkResponse(s));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("GetCompetenceTaxonomyList")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetCompetenceTaxonomyList(
        [FromBody] CompetenciesTaxonomyFilter taxonomyfilter,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            var _osParameter = new OSParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                TaxonomyFilter = taxonomyfilter,
                ContextAccessor = ContextAccessor,
                TenantProvider = _TenantProvider
            };
            
            var mApiOkResponse = new ApiOkResponse( await _iOrganizationSettingsRepository.GetCompetenceTaxonomyList(_osParameter));


            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetCompetenceTaxonomyLevels")]
    
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetCompetenceTaxonomyLevels([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            var _osParameter = new OSParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                TenantProvider = ItenantProvider
            };

            return Ok(new ApiOkResponse(
                await _iOrganizationSettingsRepository.GetCompetenceTaxonomyLevels(_osParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CreateOrganizationCertification")]
    public async Task<ActionResult> CreateOrganizationCertification(
        [FromBody] OrganizationCertificationCreate organizationCertificationCreate,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            var _osParameter = new OSParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = _TenantProvider,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                OrganizationCertificationCreate = organizationCertificationCreate
            };

            var result = await _iOrganizationSettingsRepository.CreateOrganizationCertification(_osParameter);
            return Ok(new ApiOkResponse(result, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    [HttpGet("GetOrganizationCertificationList")]
    public async Task<ActionResult> GetOrganizationCertificationList([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)

    {
        try
        {
            var lang = langCode(langX);

            var _osParameter = new OSParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = _TenantProvider
            };

            var data = await _iOrganizationSettingsRepository.GetOrganizationCertificationList(_osParameter);


            return Ok(new ApiOkResponse(data));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("OrganizationCertificationGetById/{Id}")]
    public async Task<ActionResult> OrganizationCertificationGetById(string Id, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            var _osParameter = new OSParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = _TenantProvider,
                Id = Id
            };

            var data = await _iOrganizationSettingsRepository.OrganizationCertificationGetById(_osParameter);


            return Ok(new ApiOkResponse(data));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetOrganizationCertificationDropdown")]
    
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetOrganizationCertificationDropdown([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            var _osParameter = new OSParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                TenantProvider = ItenantProvider
            };

            return Ok(new ApiOkResponse(
                await _iOrganizationSettingsRepository.GetOrganizationCertificationDropdown(_osParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CreateCertificationTaxonomy")]
    public async Task<ActionResult> CreateCertificationTaxonomy(
        [FromBody] CertificationTaxonomy CertificationTaxonomyDto,
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

            var _osParameter = new OSParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                CertificationTaxonomyDto = CertificationTaxonomyDto,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };

            var s = await _iOrganizationSettingsRepository.CreateCertificationTaxonomy(_osParameter);
            return Ok(new ApiOkResponse(s));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("GetCertificationTaxonomyList")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetCertificationTaxonomyList(
        [FromBody] CompetenciesTaxonomyFilter taxonomyfilter,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            var _osParameter = new OSParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                TaxonomyFilter = taxonomyfilter,
                ContextAccessor = ContextAccessor,
                TenantProvider = _TenantProvider
            };

            var result = await _iOrganizationSettingsRepository.GetCertificationTaxonomyList(_osParameter);
            var mApiOkResponse = new ApiOkResponse(result);


            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CreateCorporateShedule")]
    public async Task<ActionResult> CreateCorporateShedule(
        [FromBody] CorporateSheduleDto CSDto, [FromHeader(Name = "CU")] string CU,
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

            var _osParameter = new OSParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                CSDto = CSDto,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };

            var s = await _iOrganizationSettingsRepository.CreateCorporateShedule(_osParameter);
            return Ok(new ApiOkResponse(s));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CreateOrganization")]
    public async Task<ActionResult> CreateOrganization(
        [FromBody] OrganizationCreate OrganizationCreate,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            var _osParameter = new OSParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = _TenantProvider,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value,
                OrganizationCreate = OrganizationCreate
            };

            var result = await _iOrganizationSettingsRepository.CreateOrganization(_osParameter);
            return Ok(new ApiOkResponse(result, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetOrganizationList")]
    public async Task<ActionResult> GetOrganizationList([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)

    {
        try
        {
            var lang = langCode(langX);

            var _osParameter = new OSParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = _TenantProvider
            };

            var data = await _iOrganizationSettingsRepository.GetOrganizationList(_osParameter);


            return Ok(new ApiOkResponse(data));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("OrganizationGetById/{Id}")]
    public async Task<ActionResult> OrganizationGetById(string Id, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            var _osParameter = new OSParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = _TenantProvider,
                Id = Id
            };

            var data = await _iOrganizationSettingsRepository.OrganizationGetById(_osParameter);


            return Ok(new ApiOkResponse(data));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CreateOrganizationTaxonomy")]
    public async Task<ActionResult> CreateOrganizationTaxonomy(
        [FromBody] OrganizationTaxonomyDto OrganizationTaxonomyDto,
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

            var _osParameter = new OSParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                OrganizationTaxonomyDto = OrganizationTaxonomyDto,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };

            var s = await _iOrganizationSettingsRepository.CreateOrganizationTaxonomy(_osParameter);
            return Ok(new ApiOkResponse(s));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("GetOrganizationTaxonomyList")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetOrganizationTaxonomyList(
        [FromBody] OrganizationTaxonomyFilter taxonomyfilter,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            var _osParameter = new OSParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                Filter = taxonomyfilter,
                ContextAccessor = ContextAccessor,
                TenantProvider = _TenantProvider
            };

            var result = await _iOrganizationSettingsRepository.GetOrganizationTaxonomyList(_osParameter);
            var mApiOkResponse = new ApiOkResponse(result);


            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("GetOrganizationTaxonomyListForProjectPlan")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetOrganizationTaxonomyListForProjectPlan(
        [FromBody] OrganizationTaxonomyFilter taxonomyfilter,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            var _osParameter = new OSParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                Filter = taxonomyfilter,
                ContextAccessor = ContextAccessor,
                TenantProvider = _TenantProvider
            };

            var result = await _iOrganizationSettingsRepository.GetOrganizationTaxonomyListForProjectPlan(_osParameter);
            var mApiOkResponse = new ApiOkResponse(result);


            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("GetOrganizationTaxonomyListForMyCalender")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetOrganizationTaxonomyListForMyCalender(
        [FromBody] OrganizationTaxonomyFilter taxonomyfilter,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            var _osParameter = new OSParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                Filter = taxonomyfilter,
                ContextAccessor = ContextAccessor,
                TenantProvider = _TenantProvider
            };

            var result = await _iOrganizationSettingsRepository.GetOrganizationTaxonomyListForMyCalender(_osParameter);
            var mApiOkResponse = new ApiOkResponse(result);


            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetOrganizationTaxonomyLevel")]
    
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetOrganizationTaxonomyLevel([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            var _osParameter = new OSParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                TenantProvider = ItenantProvider
            };

            return Ok(new ApiOkResponse(
                await _iOrganizationSettingsRepository.GetOrganizationTaxonomyLevel(_osParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpDelete("DeleteOrganizationTaxonomyNode/{Id}")]
    public async Task<ActionResult> DeleteOrganizationTaxonomyNode(string Id, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            var _osParameter = new OSParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                Id = Id,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };

            var s = await _iOrganizationSettingsRepository.DeleteOrganizationTaxonomyNode(_osParameter);
            return Ok(new ApiOkResponse(s));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("GetCorporateSheduleList")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetCorporateSheduleList([FromBody] CorporateSheduleList CSList,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            var _osParameter = new OSParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                CSList = CSList,
                TenantProvider = ItenantProvider
            };

            return Ok(new ApiOkResponse(
                await _iOrganizationSettingsRepository.GetCorporateSheduleList(_osParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("CorporateSheduleGetById/{SequenceId}")]
    
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CorporateSheduleGetById(string SequenceId, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            var _osParameter = new OSParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                Id = SequenceId,
                TenantProvider = ItenantProvider
            };

            return Ok(new ApiOkResponse(
                await _iOrganizationSettingsRepository.CorporateSheduleGetById(_osParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetOrganizationDropdown")]
    
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetOrganizationDropdown([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            var _osParameter = new OSParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                TenantProvider = ItenantProvider
            };

            return Ok(new ApiOkResponse(
                await _iOrganizationSettingsRepository.GetOrganizationDropdown(_osParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("PersonFilter")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> PersonFilter(string SequenceId,
        [FromBody] OrganizationCabPersonFilterModel organizationCabPersonFilter, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            var _osParameter = new OSParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                organizationCabPersonFilter = organizationCabPersonFilter,
                TenantProvider = ItenantProvider
            };

            return Ok(new ApiOkResponse(
                await _iOrganizationSettingsRepository.Filter(_osParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("PersonFilterForBu")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> PersonFilterForBu(string SequenceId,
        [FromBody] OrganizationCabPersonFilterDto organizationCabPersonFilterDto,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            var _osParameter = new OSParameter
            {
                Lang = lang,
                OrganizationCabPersonFilterDto = organizationCabPersonFilterDto,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };

            _osParameter.TenantProvider = ItenantProvider;
            return Ok(new ApiOkResponse(
                await _iOrganizationSettingsRepository.PersonFilterForBu(_osParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("PersonFilterForBuTeam")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> PersonFilterForBuTeam(string SequenceId,
        [FromBody] OrganizationCabPersonFilterDto organizationCabPersonFilterDto,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            var _osParameter = new OSParameter
            {
                Lang = lang,
                OrganizationCabPersonFilterDto = organizationCabPersonFilterDto,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };

            _osParameter.TenantProvider = ItenantProvider;
            return Ok(new ApiOkResponse(
                await _iOrganizationSettingsRepository.PersonFilterForBuTeam(_osParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("OrganizationTaxonomySetDefaultBu")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> OrganizationTaxonomySetDefaultBu(
        [FromBody] OrganizationTaxonomyBu OrganizationTaxonomyBu,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            var _osParameter = new OSParameter
            {
                Lang = lang,
                OrganizationTaxonomyBu = OrganizationTaxonomyBu,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };

            _osParameter.TenantProvider = ItenantProvider;
            return Ok(new ApiOkResponse(
                await _iOrganizationSettingsRepository.OrganizationTaxonomySetDefaultBu(_osParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}