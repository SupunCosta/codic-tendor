using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.Mecops;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Controllers.Mecop;

[Route("api/[controller]")]
[ApiController]
public class MecopController : CommonConfigurationController
{
    private readonly IMecopRepository _mecopRepository;
    private readonly ITenantProvider _tenantProvider;
    
    public MecopController(ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse, ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider,IMecopRepository mecopRepository,ITenantProvider tenantProvider) 
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse, iTenantProvider)
    {
        _mecopRepository = mecopRepository;
        _tenantProvider = tenantProvider;
    }
    
    [HttpGet("GetMecopData")]
    public async Task<ActionResult> GetMecopData([FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _mecopParameter = new MecopParameter
            {
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = _tenantProvider
            };

            var result = await _mecopRepository.GetMecopData(_mecopParameter);
            return Ok(new ApiOkResponse(result));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpGet("GetMecopForExel")]
    public async Task<ActionResult> GetMecopForExel([FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _mecopParameter = new MecopParameter
            {
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = _tenantProvider
            };

            var result = await _mecopRepository.GetMecopForExelNew(_mecopParameter);
            return Ok(new ApiOkResponse(result));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpGet("GetMecopMetaDataForExel")]
    public async Task<ActionResult> GetMecopMetaDataForExel([FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _mecopParameter = new MecopParameter
            {
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = _tenantProvider
            };

            var result = await _mecopRepository.GetMecopMetaDataForExel(_mecopParameter);
            return Ok(new ApiOkResponse(result));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("MecopStatusUpdate")]
    public async Task<ActionResult> MecopStatusUpdate([FromHeader(Name = "lang")] string langX, [FromBody] MecopStatusUpdateDto mecopStatusUpdateDto)
    {
        try
        {
            var lang = langCode(langX);
            var _mecopParameter = new MecopParameter
            {
                Lang = lang,
                MecopStatusUpdateDto = mecopStatusUpdateDto,
                ContextAccessor = ContextAccessor,
                TenantProvider = _tenantProvider
            };

            var result = await _mecopRepository.MecopStatusUpdate(_mecopParameter);
            return Ok(new ApiOkResponse(result));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}