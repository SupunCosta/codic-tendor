using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories;
using UPrinceV4.Web.UserException;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Controllers;


[Authorize]
[Route("api/[controller]")]
[ApiController]
public class QualityController : CommonConfigurationController
{
    private readonly IQualityRepository _iQualityRepository;
    private readonly QualityRepositoryParameter _qualityRepositoryParameter;

    public QualityController(ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor,
        ApiResponse apiResponse, ApiBadRequestResponse apiBadRequestResponse
        , ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider, IQualityRepository iQualityRepository,
        QualityRepositoryParameter qualityRepositoryParameter
        , ILogger<QualityController> logger)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)
    {
        _iQualityRepository = iQualityRepository;
        _qualityRepositoryParameter = qualityRepositoryParameter;
    }

    //[Microsoft.AspNetCore.Mvc.HttpPost("CreateQuality")]
    [HttpPost("CreateQuality")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateQuality([FromBody] QualityDto quality)
    {
        try
        {
            _qualityRepositoryParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _qualityRepositoryParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var user = UPrinceCustomerContext.ApplicationUser
                .FirstOrDefault(u => u.OId == objectIdentifier);
            var qualityObj = new Quality
            {
                Id = quality.Id,
                Name = quality.Name,
                Criteria = quality.Criteria,
                Method = quality.Method,
                Skills = quality.Skills,
                Tolerance = quality.Tolerance,
                ProjectDefinitionId = quality.ProjectId
            };

            _qualityRepositoryParameter.TenantProvider = ItenantProvider;
            _qualityRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            _qualityRepositoryParameter.Quality = qualityObj;
            _qualityRepositoryParameter.ChangedUser = user;
            return Ok(new ApiOkResponse(await _iQualityRepository.AddQuality(_qualityRepositoryParameter)));
        }
        catch (Exception ex)
        {
            ApiResponse.StatusCode = 400;
            ApiResponse.Status = false;
            ApiResponse.Message = ex.Message;
            return BadRequest(ApiResponse);
        }
    }


    //[Microsoft.AspNetCore.Mvc.HttpGet("ReadQualityList")]
    [HttpGet("ReadQualityList")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetQualityList()
    {
        try
        {
            _qualityRepositoryParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _qualityRepositoryParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            _qualityRepositoryParameter.TenantProvider = ItenantProvider;
            _qualityRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            _qualityRepositoryParameter.Lang = lang;

            var qualityList = await _iQualityRepository.GetQualityList(_qualityRepositoryParameter);

            if (qualityList == null || !qualityList.Any())
            {
                ApiOkResponse.Result = null;
                ApiOkResponse.Message = "noAvailableQuality";
                return Ok(ApiOkResponse);
            }

            return Ok(new ApiOkResponse(qualityList));
        }
        catch (EmptyListException ex)
        {
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = false;
            ApiResponse.Message = ex.Message;
            return Ok(ApiResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    //[Microsoft.AspNetCore.Mvc.HttpGet("ReadQuality/{id}")]
    [HttpGet("ReadQuality")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetQualityById(string id)
    {
        try
        {
            _qualityRepositoryParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _qualityRepositoryParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            _qualityRepositoryParameter.TenantProvider = ItenantProvider;
            _qualityRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            _qualityRepositoryParameter.QualityId = id;
            _qualityRepositoryParameter.Lang = lang;

            var quality = await _iQualityRepository.GetQualityById(_qualityRepositoryParameter);

            if (quality != null) return Ok(new ApiOkResponse(quality));
            ApiOkResponse.Result = null;
            ApiOkResponse.Message = "noAvailableQualityForTheId";
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpDelete("DeleteQuality")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult DeleteQuality([FromBody] List<string> idList)
    {
        try
        {
            _qualityRepositoryParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _qualityRepositoryParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            _qualityRepositoryParameter.TenantProvider = ItenantProvider;
            _qualityRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            _qualityRepositoryParameter.IdList = idList;
            _iQualityRepository.DeleteQuality(_qualityRepositoryParameter);
            ApiOkResponse.Message = "Ok";
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    //[Microsoft.AspNetCore.Mvc.HttpPost("QualityFilter")]
    [HttpPost("QualityFilter")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> QualityFilter(
        [FromBody] QualityFilterModel qualityFilter)
    {
        _qualityRepositoryParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
        _qualityRepositoryParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
        var lang = langCode(Request.Headers["lang"].FirstOrDefault());
        try
        {
            _qualityRepositoryParameter.TenantProvider = ItenantProvider;
            _qualityRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            _qualityRepositoryParameter.Lang = lang;
            _qualityRepositoryParameter.QualityFilterModel = qualityFilter;

            var riskList = await _iQualityRepository.Filter(_qualityRepositoryParameter);
            var result = riskList;
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = false;
            ApiResponse.Message = ApiErrorMessages
                .GetErrorMessage(ItenantProvider, ErrorMessageKey.NoAvailableQuality, lang).Message;
            ApiOkResponse.Result = result;
            return Ok(ApiOkResponse);
        }
        catch (EmptyListException)
        {
            var message = ApiErrorMessages
                .GetErrorMessage(ItenantProvider, ErrorMessageKey.NoAvailableQuality, lang)
                .Message;
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = false;
            ApiResponse.Message = message;
            return Ok(ApiResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}