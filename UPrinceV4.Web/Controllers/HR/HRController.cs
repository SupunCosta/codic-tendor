using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.HR;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Repositories.Interfaces.HR;
using UPrinceV4.Web.Repositories.Interfaces.PMOL;
using UPrinceV4.Web.UserException;

namespace UPrinceV4.Web.Controllers.HR;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class HRController : CommonConfigurationController
{
    private readonly IHRRepository _iHRRepository;
    private readonly IPmolRepository _iPmolRepository;
    private readonly IVPRepository _iVPRepository;
    private readonly ITenantProvider _TenantProvider;


    public HRController(ITenantProvider tenantProvider,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider
        , IHRRepository iHRRepository, IVPRepository iVPRepository, IPmolRepository iPmolRepository)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)

    {
        _iHRRepository = iHRRepository;
        _TenantProvider = tenantProvider;
        _iVPRepository = iVPRepository;
        _iPmolRepository = iPmolRepository;
    }

    [HttpPost("CreateHR")]
    public async Task<ActionResult> CreateHR([FromBody] CreateHRDto CreateHRDto,
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
            var hrParameter = new HRParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                CreateHR = CreateHRDto,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };
            return Ok(new ApiOkResponse(await _iHRRepository.CreateHR(hrParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetHRById/{SequenceId}")]
    public async Task<ActionResult> GetHRById(string SequenceId, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            var hrParameter = new HRParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = _TenantProvider,
                Id = SequenceId
            };
            return Ok(new ApiOkResponse(await _iHRRepository.GetHRById(hrParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("FilterHRList")]
    public async Task<ActionResult> FilterHRList([FromBody] FilterHR Filter,
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
            var hrParameter = new HRParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                Filter = Filter,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };

            var s = await _iHRRepository.FilterHRList(hrParameter);
            return Ok(new ApiOkResponse(s));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetTaxonomyIdByPersonId/{PersonId}")]
    public async Task<ActionResult> GetTaxonomyIdByPersonId(string PersonId, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var hrParameter = new HRParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                Id = PersonId,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider
            };

            return Ok(new ApiOkResponse(await _iHRRepository.GetTaxonomyIdByPersonId(hrParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetHRRoles")]
    public async Task<ActionResult> GetHRRoles([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var hrParameter = new HRParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider
            };

            return Ok(new ApiOkResponse(await _iHRRepository.GetHRRoles(hrParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetLabourHistory/{PersonId}")]
    public async Task<ActionResult> GetLabourHistory(string PersonId, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var hrParameter = new HRParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                Id = PersonId,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                IVPRepository = _iVPRepository,
                IPmolRepository = _iPmolRepository
            };

            return Ok(new ApiOkResponse(await _iHRRepository.GetLabourHistory(hrParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("HRPersonFilter")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> HRPersonFilter(
        [FromBody] CabPersonFilterModel cabPersonFilter, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        var lang = langCode(Request.Headers["lang"].FirstOrDefault());
        try
        {
            var hrParameter = new HRParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                CabPersonFilter = cabPersonFilter
            };
            var personList = await _iHRRepository.HRPersonFilter(hrParameter);

            var mApiOkResponse = new ApiOkResponse(personList)
            {
                Status = true,
                Result = personList,
                Message = "ok"
            };

            if (!personList.Any())
            {
                var mApiResponse = new ApiOkResponse(null, "noavailableperson")
                {
                    Status = false
                };
                return Ok(mApiResponse);
            }

            return Ok(mApiOkResponse);
        }
        catch (EmptyListException)
        {
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = false;
            ApiResponse.Message = "noPeopleInYourSearchCriteria";
            return Ok(ApiResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpDelete("delete/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> RemoveHr(string Id, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var hrParameter = new HRParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = langX,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                Id = Id
            };
            return Ok(new ApiOkResponse(await _iHRRepository.RemoveHr(hrParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpGet("GetLabourPmolPr/{PersonId}")]
    public async Task<ActionResult> GetLabourPmolPr(string PersonId, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var hrParameter = new HRParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                Id = PersonId,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                IVPRepository = _iVPRepository,
                IPmolRepository = _iPmolRepository
            };

            return Ok(new ApiOkResponse(await _iHRRepository.GetLabourPmolPr(hrParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("CreateHrContractorList")]
    public async Task<ActionResult> CreateHrContractorList([FromBody]HRContractorList dto,
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
            var hrParameter = new HRParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                HRContractorListDto = dto,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };

            var s = await _iHRRepository.CreateHrContractorList(hrParameter);
            return Ok(new ApiOkResponse(s));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpGet("GetHrContractorList/{id}")]
    public async Task<ActionResult> GetHrContractorList(string id,
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
            var hrParameter = new HRParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                Id = id,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };

            var s = await _iHRRepository.GetHrContractorList(hrParameter);
            return Ok(new ApiOkResponse(s));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("DeleteHrContractorList")]
    public async Task<ActionResult> DeleteHrContractorList([FromBody]List<string> idList,
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
            var hrParameter = new HRParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                IdList = idList,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };

            var s = await _iHRRepository.DeleteHrContractorList(hrParameter);
            return Ok(new ApiOkResponse(s));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpGet("GetHrContractTypes")]
    public async Task<ActionResult> GetHrContractorList(
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
            var hrParameter = new HRParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };

            var s = await _iHRRepository.GetHrContractTypes(hrParameter);
            return Ok(new ApiOkResponse(s));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("HrContractorFileUpload")]
    public async Task<ActionResult<string>> HrContractorFileUpload([FromForm] IFormCollection file,
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

            
            var client = new FileClient();
            var url = client.PersistLotUpload(file.Files.FirstOrDefault()?.FileName, _TenantProvider
                , file.Files.FirstOrDefault(), "HrContractor");

            var response = new ApiOkResponse(url);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}