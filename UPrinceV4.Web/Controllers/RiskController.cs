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
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Repositories.Interfaces.CAB;
using UPrinceV4.Web.UserException;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class RiskController : CommonConfigurationController
{
    private readonly IPersonRepository _ipersonRepository;
    private readonly IRiskRepository _iRiskRepository;
    private readonly IRiskStatusRepository _iRiskStatusRepository;
    private readonly IRiskTypeRepository _iRiskTypeRepository;
    private readonly ILogger<RiskController> _logger;
    private readonly PersonRepositoryParameter _personRepositoryParameter;
    private readonly RiskRepositoryParameter _riskRepositoryParameter;
    private readonly RiskStatusRepositoryParameter _riskStatusRepositoryParameter;
    private readonly RiskTypeRepositoryParameter _riskTypeRepositoryParameter;

    public RiskController(ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor,
        ApiResponse apiResponse
        , ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider,
        IRiskStatusRepository iRiskStatusRepository, IRiskTypeRepository iRiskTypeRepository,
        IRiskRepository iRiskRepository, RiskRepositoryParameter riskRepositoryParameter,
        PersonRepositoryParameter personRepositoryParameter,
        RiskStatusRepositoryParameter riskStatusRepositoryParameter,
        RiskTypeRepositoryParameter riskTypeRepositoryParameter, IPersonRepository ipersonRepository
        , ILogger<RiskController> logger)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)
    {
        _iRiskStatusRepository = iRiskStatusRepository;
        _iRiskTypeRepository = iRiskTypeRepository;
        _iRiskRepository = iRiskRepository;
        _riskRepositoryParameter = riskRepositoryParameter;
        _riskStatusRepositoryParameter = riskStatusRepositoryParameter;
        _riskTypeRepositoryParameter = riskTypeRepositoryParameter;
        _personRepositoryParameter = personRepositoryParameter;
        _ipersonRepository = ipersonRepository;
        _logger = logger;
    }

    //[Microsoft.AspNetCore.Mvc.HttpPost("CreateRisk")]
    [HttpPost("CreateRisk")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateRisk([FromBody] RiskDto risk)
    {
        try
        {
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var user = UPrinceCustomerContext.ApplicationUser
                .FirstOrDefault(u => u.OId == objectIdentifier);

            var riskObj = new Risk
            {
                Id = risk.Id,
                PersonId = risk.CabPersonId,
                Name = risk.Name,
                RiskDetails = risk.RiskDetails,
                RiskStatusId = risk.RiskStatusId,
                RiskTypeId = risk.RiskTypeId
                /*ProjectDefinitionId = risk.ProjectId,*/
            };

            _riskRepositoryParameter.TenantProvider = ItenantProvider;
            _riskRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            _riskRepositoryParameter.Risk = riskObj;
            _riskRepositoryParameter.ChangedUser = user;
            _riskRepositoryParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _riskRepositoryParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();


            return Ok(new ApiOkResponse(await _iRiskRepository.AddRisk(_riskRepositoryParameter)));
        }
        catch (Exception ex)
        {
            ApiResponse.StatusCode = 400;
            ApiResponse.Status = false;
            ApiResponse.Message = ex.Message;
            ;
            return BadRequest(ApiResponse);
        }
    }

    //[Microsoft.AspNetCore.Mvc.HttpPost("CreateRiskType")]
    [HttpPost("CreateRiskType")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateRiskType([FromBody] string riskType)
    {
        try
        {
            var riskTypeObj = new RiskType();
            riskTypeObj.Type = riskType;

            _riskTypeRepositoryParameter.TenantProvider = ItenantProvider;
            _riskTypeRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            _riskTypeRepositoryParameter.RiskType = riskTypeObj;


            return Ok(new ApiOkResponse(await _iRiskTypeRepository.AddRiskType(_riskTypeRepositoryParameter)));
        }
        catch (Exception ex)
        {
            ApiResponse.StatusCode = 400;
            ApiResponse.Status = false;
            ApiResponse.Message = ex.Message;
            ;
            return BadRequest(ApiResponse);
        }
    }

    //[Microsoft.AspNetCore.Mvc.HttpPost("CreateRiskStatus")]
    [HttpPost("CreateRiskStatus")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateRiskStatus([FromBody] string riskStatus)
    {
        try
        {
            var riskStatusObj = new RiskStatus
            {
                Status = riskStatus
            };

            _riskStatusRepositoryParameter.TenantProvider = ItenantProvider;
            _riskStatusRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            _riskStatusRepositoryParameter.RiskStatus = riskStatusObj;


            return Ok(new ApiOkResponse(
                await _iRiskStatusRepository.AddRiskStatus(_riskStatusRepositoryParameter)));
        }
        catch (Exception ex)
        {
            ApiResponse.StatusCode = 400;
            ApiResponse.Status = false;
            ApiResponse.Message = ex.Message;
            ;
            return BadRequest(ApiResponse);
        }
    }

    //[Microsoft.AspNetCore.Mvc.HttpGet("ReadRiskList")]
    [HttpGet("ReadRiskList")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetRiskList()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            // var message = ApiErrorMessages.GetErrorMessage(ItenantProvider, ErrorMessageKey.Ok, lang).Message;
            _riskRepositoryParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _riskRepositoryParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _riskRepositoryParameter.TenantProvider = ItenantProvider;
            _riskRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            _riskRepositoryParameter.Lang = lang;
            _riskRepositoryParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _riskRepositoryParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();

            var riskList = await _iRiskRepository.GetRiskList(_riskRepositoryParameter);

            if (riskList == null || !riskList.Any())
            {
                ApiOkResponse.Result = riskList;
                ApiOkResponse.Message = "noAvailableRisk";
                return Ok(ApiOkResponse);
            }

            return Ok(new ApiOkResponse(riskList));
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

    [HttpGet("ReadRisk/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetRiskById(string id)
    {
        try
        {
            _riskRepositoryParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _riskRepositoryParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            _riskRepositoryParameter.TenantProvider = ItenantProvider;
            _riskRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            _riskRepositoryParameter.RiskId = id;
            _riskRepositoryParameter.Lang = lang;
            _riskRepositoryParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _riskRepositoryParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();

            var risk = await _iRiskRepository.GetRiskById(_riskRepositoryParameter);

            if (risk != null) return Ok(new ApiOkResponse(risk));
            ApiOkResponse.Result = null;
            ApiOkResponse.Message = "noAvailableRiskForTheId";
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpDelete("DeleteRisk")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult DeleteRisk([FromBody] List<string> idList)
    {
        try
        {
            _riskRepositoryParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _riskRepositoryParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            _riskRepositoryParameter.TenantProvider = ItenantProvider;
            _riskRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            _riskRepositoryParameter.IdList = idList;
            _iRiskRepository.DeleteRisk(_riskRepositoryParameter);
            ApiOkResponse.Message = "Ok";
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadRiskDropdowns")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetRiskDropdowns()
    {
        try
        {
            _riskRepositoryParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _riskRepositoryParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            _riskRepositoryParameter.TenantProvider = ItenantProvider;
            _riskRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            _riskRepositoryParameter.Lang = lang;

            _riskStatusRepositoryParameter.TenantProvider = ItenantProvider;
            _riskStatusRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            _riskStatusRepositoryParameter.Lang = lang;
            _riskRepositoryParameter.RiskStatusRepositoryParameter = _riskStatusRepositoryParameter;

            _riskTypeRepositoryParameter.TenantProvider = ItenantProvider;
            _riskTypeRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            _riskTypeRepositoryParameter.Lang = lang;
            _riskRepositoryParameter.RiskTypeRepositoryParameter = _riskTypeRepositoryParameter;

            _personRepositoryParameter.TenantProvider = ItenantProvider;
            _personRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            _riskRepositoryParameter.PersonRepositoryParameter = _personRepositoryParameter;

            _riskRepositoryParameter.IPersonRepository = _ipersonRepository;
            _riskRepositoryParameter.IRiskStatusRepository = _iRiskStatusRepository;
            _riskRepositoryParameter.IRiskTypeRepository = _iRiskTypeRepository;


            return Ok(new ApiOkResponse(await _iRiskRepository.GetRiskDropdownData(_riskRepositoryParameter)));
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

    //[Microsoft.AspNetCore.Mvc.HttpPost("RiskFilter")]
    [HttpPost("RiskFilter")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> RiskFilter([FromBody] RiskFilterModel riskFilter)
    {
        var lang = langCode(Request.Headers["lang"].FirstOrDefault());
        try
        {
            _riskRepositoryParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _riskRepositoryParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _riskRepositoryParameter.TenantProvider = ItenantProvider;
            _riskRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            _riskRepositoryParameter.Lang = lang;
            _riskRepositoryParameter.RiskFilterModel = riskFilter;

            var riskList = await _iRiskRepository.Filter(_riskRepositoryParameter);
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = false;
            ApiResponse.Message = "noAvailableRisk";
            ApiOkResponse.Result = riskList;
            ApiOkResponse.Message = "ok";
            return Ok(ApiOkResponse);
        }
        catch (EmptyListException)
        {
            var message = ApiErrorMessages
                .GetErrorMessage(ItenantProvider, ErrorMessageKey.NoAvailableRisk, lang)
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