using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.CAB;
using UPrinceV4.Web.Data.CIAW;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Controllers.CIAW;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class CiawController : CommonConfigurationController
{
    private readonly ICiawRepository _iciawRepository;
    private readonly ITenantProvider _tenantProvider;


    public CiawController(ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor,
        ApiResponse apiResponse, ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse,
        ITenantProvider iTenantProvider, ICiawRepository iciawRepository,
        IConfiguration iConfiguration) : base(uPrinceCustomerContext, contextAccessor, apiResponse,
        apiBadRequestResponse, apiOkResponse, iTenantProvider)
    {
        _iciawRepository = iciawRepository;
        _tenantProvider = iTenantProvider;
        _iConfiguration = iConfiguration;
    }

    private IConfiguration _iConfiguration { get; }

    [HttpPost("CreateCiaw")]
    public async Task<ActionResult> CreateCiaw([FromBody] CiawCreateDto CiawCreateDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _ciawParameter = new CiawParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                CiawCreateDto = CiawCreateDto,
                ContextAccessor = ContextAccessor,
                TenantProvider = _tenantProvider,
                Configuration = _iConfiguration,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };

            var ciaw = await _iciawRepository.Create(_ciawParameter);
            return Ok(new ApiOkResponse(ciaw));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("FilterCiaw")]
    public async Task<ActionResult> FilterCiaw([FromBody] CiawFilter CiawFilter,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _ciawParameter = new CiawParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                CiawFilter = CiawFilter,
                ContextAccessor = ContextAccessor,
                TenantProvider = _tenantProvider,
                Configuration = _iConfiguration,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };

            var ciaw = await _iciawRepository.FilterCiaw(_ciawParameter);
            return Ok(new ApiOkResponse(ciaw));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpGet("CiawDropDownData")]
    public async Task<ActionResult> CiawDropDownData([FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _ciawParameter = new CiawParameter
            {
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = _tenantProvider,
                Configuration = _iConfiguration,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };

            var ciaw = await _iciawRepository.CiawDropDownData(_ciawParameter);
            return Ok(new ApiOkResponse(ciaw));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpGet("CiawCancelDropDownData")]
    public async Task<ActionResult> CiawCancelDropDownData([FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _ciawParameter = new CiawParameter
            {
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = _tenantProvider,
                Configuration = _iConfiguration,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };

            var ciaw = await _iciawRepository.CiawCancelDropDownData(_ciawParameter);
            return Ok(new ApiOkResponse(ciaw));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpGet("CiawGetById/{CiawId}")]
    public async Task<ActionResult> CiawGetById(string CiawId, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _ciawParameter = new CiawParameter
            {
                Lang = lang,
                Id = CiawId,
                ContractingUnitSequenceId = CU,
                ContextAccessor = ContextAccessor,
                TenantProvider = _tenantProvider,
                Configuration = _iConfiguration,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };

            var ciaw = await _iciawRepository.CiawGetById(_ciawParameter);
            return Ok(new ApiOkResponse(ciaw));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("ProjectCiawSiteCreate")]
    public async Task<ActionResult> ProjectCiawSiteCreate([FromBody] ProjectCiawSite ProjectCiawSite,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _ciawParameter = new CiawParameter
            {
                Lang = lang,
                ProjectCiawSite = ProjectCiawSite,
                ContextAccessor = ContextAccessor,
                TenantProvider = _tenantProvider,
                Configuration = _iConfiguration,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };

            var ciaw = await _iciawRepository.ProjectCiawSiteCreate(_ciawParameter);
            return Ok(new ApiOkResponse(ciaw));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("ProjectCiawSiteGet/{Id}")]
    public async Task<ActionResult> ProjectCiawSiteGet(string Id, [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _ciawParameter = new CiawParameter
            {
                Lang = lang,
                Id = Id,
                ContextAccessor = ContextAccessor,
                TenantProvider = _tenantProvider,
                Configuration = _iConfiguration,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };

            var ciaw = await _iciawRepository.ProjectCiawSiteGet(_ciawParameter);
            return Ok(new ApiOkResponse(ciaw));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("FilterNationality")]
    public async Task<ActionResult> FilterNationality([FromBody] NationalityFilter NationalityFilter,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _ciawParameter = new CiawParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                NationalityFilter = NationalityFilter,
                ContextAccessor = ContextAccessor,
                TenantProvider = _tenantProvider,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };

            var ciaw = await _iciawRepository.FilterNationality(_ciawParameter);
            return Ok(new ApiOkResponse(ciaw));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("CiawSendRequest")]
    public async Task<ActionResult> CiawSendRequest([FromBody] CiawSendRequest CiawSendRequest,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _ciawParameter = new CiawParameter
            {
                Lang = lang,
                ContractingUnitSequenceId = CU,
                CiawSendRequest = CiawSendRequest,
                ContextAccessor = ContextAccessor,
                TenantProvider = _tenantProvider,
                Configuration = _iConfiguration,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };

            var ciaw = await _iciawRepository.CiawSendRequest(_ciawParameter);
            return Ok(new ApiOkResponse(ciaw));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    // [HttpPost("CiawSendSingleRequest")]
    // public async Task<ActionResult> CiawSendSingleRequest([FromBody] CiawSendRequest CiawSendRequest, [FromHeader(Name = "lang")] string langX)
    // {
    //     try
    //     {
    //         var lang = langCode(langX);
    //         _ciawParameter.Lang = lang;
    //         _ciawParameter.CiawSendRequest = CiawSendRequest;
    //         _ciawParameter.ContextAccessor = ContextAccessor;
    //         _ciawParameter.TenantProvider = _tenantProvider;
    //         _ciawParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
    //             claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
    //         var ciaw = await _iciawRepository.CiawSendSingleRequest(_ciawParameter);
    //         return Ok(new ApiOkResponse(ciaw));
    //     }
    //     catch (Exception ex)
    //     {
    //         return BadRequest(new ApiResponse(400, false, ex.StackTrace));
    //     }
    // }
    [HttpPost("CiawCancelPresences")]
    public async Task<ActionResult> CiawCancelPresences([FromBody] CiawCancleRequest CiawCancleRequest,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _ciawParameter = new CiawParameter
            {
                Lang = lang,
                ContractingUnitSequenceId = CU,
                CiawCancleRequest = CiawCancleRequest,
                ContextAccessor = ContextAccessor,
                TenantProvider = _tenantProvider,
                Configuration = _iConfiguration,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };

            var ciaw = await _iciawRepository.CiawCancelPresences(_ciawParameter);
            return Ok(new ApiOkResponse(ciaw));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpGet("SendCiawWarningEmail/{Id}")]
    public async Task<ActionResult> SendCiawWarningEmail(string Id,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _ciawParameter = new CiawParameter
            {
                Lang = lang,
                Id = Id,
                ContractingUnitSequenceId = CU,
                ContextAccessor = ContextAccessor,
                TenantProvider = _tenantProvider,
                Configuration = _iConfiguration,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };

            var ciaw = await _iciawRepository.SendCiawWarningEmail(_ciawParameter);
            return Ok(new ApiOkResponse(ciaw));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }
}