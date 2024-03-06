using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces.PBS;

namespace UPrinceV4.Web.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PbsFilterController : CommonConfigurationController
{
    private readonly IPbsRepository _IPbsRepository;

    public PbsFilterController(ApplicationDbContext uPrinceCustomerContext,
        IPbsRepository iPbsRepository,
        IHttpContextAccessor contextAccessor, ApiResponse apiResponse, ApiBadRequestResponse apiBadRequestResponse,
        ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider) : base(uPrinceCustomerContext,
        contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse, iTenantProvider)
    {
        _IPbsRepository = iPbsRepository;
    }

    [HttpPost("PbsFilter")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetPbs([FromBody] PbsFilter filter)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _PbsParameters = new PbsParameters();
            _PbsParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsParameters.Lang = lang;
            _PbsParameters.TenantProvider = ItenantProvider;
            _PbsParameters.Filter = filter;
            var pbs = await _IPbsRepository.GetPbs(_PbsParameters);

            if (pbs.Any()) return Ok(new ApiOkResponse(pbs));
            var mApiResponse = new ApiOkResponse(null, "noavailableprojectbreakdownstructure")
            {
                Status = false
            };
            return Ok(mApiResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("PbsFilterPO")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetPbsPO([FromBody] PbsFilter filter,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _PbsParameters = new PbsParameters();
            _PbsParameters.ContractingUnitSequenceId = CU;
            _PbsParameters.ProjectSequenceId = Project;
            _PbsParameters.Lang = lang;
            _PbsParameters.TenantProvider = ItenantProvider;
            _PbsParameters.Filter = filter;
            var pbs = await _IPbsRepository.GetPbsPO(_PbsParameters);

            if (!pbs.Any())
            {
                var mApiResponse = new ApiOkResponse(null, "noavailableprojectbreakdownstructure");
                mApiResponse.Status = false;
                return Ok(mApiResponse);
            }

            return Ok(new ApiOkResponse(pbs));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("PbsFilterHasBor")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetPbsFilterHasBor([FromBody] PbsFilter filter)
    {
        try
        {
            //_logger.LogTrace("Started");
            // _logger.LogError("Filter" + JsonToStringConverter.getStringFromJson(filter));
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _PbsParameters = new PbsParameters();
            _PbsParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsParameters.Lang = lang;
            _PbsParameters.TenantProvider = ItenantProvider;
            _PbsParameters.Filter = filter;
            var result = await _IPbsRepository.GetPbsFilterHasBor(_PbsParameters);
            
            if (!result.Any())
            {
                var mApiResponse = new ApiOkResponse(null, "noavailableprojectbreakdownstructure");
                mApiResponse.Status = false;
                return Ok(mApiResponse);
            }

            return Ok(new ApiOkResponse(result));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("ProductResourcesById")]
    public async Task<ActionResult> ProductResourcesByIdById(List<string> idList,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _PbsParameters = new PbsParameters();
            _PbsParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            //  string message = ApiErrorMessages.GetErrorMessage(ItenantProvider, ErrorMessageKey.Ok, lang).Message;
            _PbsParameters.Lang = lang;
            _PbsParameters.TenantProvider = ItenantProvider;
            _PbsParameters.IdList = idList;
            var data = await _IPbsRepository.ProductResourcesByIdById(_PbsParameters);

            return Ok(new ApiOkResponse(data));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}