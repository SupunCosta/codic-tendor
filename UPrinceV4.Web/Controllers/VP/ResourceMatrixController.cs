using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.VisualPlan;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Controllers.VP;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class ResourceMatrixController : CommonConfigurationController
{
    private readonly ResourceMatrixParameter _resourceMatrixParameter;
    private readonly IResourceMatrixRepository _iResourceMatrixRepository;
    
    public ResourceMatrixController(ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse, ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider,ResourceMatrixParameter resourceMatrixParameter,IResourceMatrixRepository iResourceMatrixRepository) : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse, iTenantProvider)
    {
        _resourceMatrixParameter = resourceMatrixParameter;
        _iResourceMatrixRepository = iResourceMatrixRepository;
    }
    
    [HttpPost("GetResourceMatrix")]
    public async Task<ActionResult> GetResourceMatrix([FromBody] GetResourceMatrixDto getResourceMatrixDto,[FromHeader(Name = "isMyEnv")] bool myEnv,[FromHeader(Name = "isCu")] bool isCu,[FromHeader(Name = "CU")] string cu, [FromHeader(Name = "Project")] string Project)
    {
        try
        {
            var resourceMatrixParameter = new ResourceMatrixParameter();
            resourceMatrixParameter.Lang = langCode(Request.Headers["lang"].FirstOrDefault());
            resourceMatrixParameter.ContextAccessor = ContextAccessor;
            resourceMatrixParameter.TenantProvider = ItenantProvider;
            resourceMatrixParameter.IsMyEnv = myEnv;
            resourceMatrixParameter.IsCu = isCu;
            resourceMatrixParameter.ContractingUnitSequenceId = cu;
            resourceMatrixParameter.ProjectSequenceId = Project;
            resourceMatrixParameter.getResourceMatrixDto = getResourceMatrixDto;
            
            resourceMatrixParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            if (getResourceMatrixDto.Type == "week")
            {
                if (getResourceMatrixDto.Filter.ViewMode == "week")
                {
                    var data = await _iResourceMatrixRepository.GetResourceMatrixFromPmol(resourceMatrixParameter);
                    return Ok(new ApiOkResponse(data));
                }
                if (getResourceMatrixDto.Filter.ViewMode == "quarter")
                {
                    var data = await _iResourceMatrixRepository.GetResourceMatrixFromPmolQuarter(resourceMatrixParameter);
                    return Ok(new ApiOkResponse(data));
                }
                else
                {
                    var data = await _iResourceMatrixRepository.GetResourceMatrixFromPmolMonth(resourceMatrixParameter);
                    return Ok(new ApiOkResponse(data));
                }
            }

            else
            {
                if (getResourceMatrixDto.Filter.ViewMode == "week")
                {
                    var data = await _iResourceMatrixRepository.GetResourceMatrixFromPbs(resourceMatrixParameter);
                    return Ok(new ApiOkResponse(data));
                }
                if (getResourceMatrixDto.Filter.ViewMode == "quarter")
                {
                    var data = await _iResourceMatrixRepository.GetResourceMatrixFromPbsQuarter(resourceMatrixParameter);
                    return Ok(new ApiOkResponse(data));
                }
                
                else
                {
                    var data = await _iResourceMatrixRepository.GetResourceMatrixFromPbsMonth(resourceMatrixParameter);
                    return Ok(new ApiOkResponse(data));
                }
            }

           
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("GetLabourMatrix")]
    public async Task<ActionResult> GetLabourMatrix([FromBody] PbsDate pbsDate,[FromHeader(Name = "isCu")] bool isCu,[FromHeader(Name = "CU")] string cu, [FromHeader(Name = "Project")] string Project)
    {
        try
        {
            _resourceMatrixParameter.Lang = langCode(Request.Headers["lang"].FirstOrDefault());
            
            _resourceMatrixParameter.ContextAccessor = ContextAccessor;
            _resourceMatrixParameter.TenantProvider = ItenantProvider;
            _resourceMatrixParameter.IsCu = isCu;
            _resourceMatrixParameter.ContractingUnitSequenceId = cu;
            _resourceMatrixParameter.ProjectSequenceId = Project;
            _resourceMatrixParameter.PbsDate = pbsDate;
            _resourceMatrixParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            if (pbsDate.Type == "Day")
            {
                var data = await _iResourceMatrixRepository.GetLabourMatrixByDate(_resourceMatrixParameter);
                return Ok(new ApiOkResponse(data));
            }

            if (pbsDate.Type == "Week")
            {
                var data = await _iResourceMatrixRepository.GetLabourMatrixByWeek(_resourceMatrixParameter);
                return Ok(new ApiOkResponse(data));
            }
            else
            {
                var data = await _iResourceMatrixRepository.GetLabourMatrixByMonth(_resourceMatrixParameter);
                return Ok(new ApiOkResponse(data));
            }
            
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("GetOrganizationMatrix")]
    public async Task<ActionResult> GetOrganizationMatrix([FromBody] PbsDate pbsDate,[FromHeader(Name = "isCu")] bool isCu,[FromHeader(Name = "CU")] string cu, [FromHeader(Name = "Project")] string Project)
    {
        try
        {
            _resourceMatrixParameter.Lang = langCode(Request.Headers["lang"].FirstOrDefault());
            
            _resourceMatrixParameter.ContextAccessor = ContextAccessor;
            _resourceMatrixParameter.TenantProvider = ItenantProvider;
            _resourceMatrixParameter.IsCu = isCu;
            _resourceMatrixParameter.ContractingUnitSequenceId = cu;
            _resourceMatrixParameter.ProjectSequenceId = Project;
            _resourceMatrixParameter.PbsDate = pbsDate;
            _resourceMatrixParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            if (pbsDate.Type == "Day")
            {
                var data = await _iResourceMatrixRepository.GetOrganizationMatrixDay(_resourceMatrixParameter);
                return Ok(new ApiOkResponse(data));
            }
            if (pbsDate.Type == "Week")
            {
                var data = await _iResourceMatrixRepository.GetOrganizationMatrixWeek(_resourceMatrixParameter);
                return Ok(new ApiOkResponse(data));
            }
            else
            {
                var data = await _iResourceMatrixRepository.GetOrganizationMatrixMonth(_resourceMatrixParameter);
                return Ok(new ApiOkResponse(data));
            }


        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}