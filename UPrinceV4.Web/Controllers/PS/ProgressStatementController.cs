using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.PS;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces.PMOL;
using UPrinceV4.Web.Repositories.Interfaces.PS;

namespace UPrinceV4.Web.Controllers.PS;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class ProgressStatementController : CommonConfigurationController
{
    private readonly IPsRepository _iPsRepository;
    private readonly ILogger<ProgressStatementController> _logger;
    private readonly PmolParameter _pmolParameter;
    private readonly PsParameter _psParameter;

    public ProgressStatementController(ApplicationDbContext uPrinceCustomerContext, IPsRepository iPsRepository,
        PsParameter psParameter, IHttpContextAccessor contextAccessor, PmolParameter PmolParameter,
        ApiResponse apiResponse, ApiBadRequestResponse apiBadRequestResponse,
        ILogger<ProgressStatementController> logger, ApiOkResponse apiOkResponse,
        ITenantProvider iTenantProvider) : base(uPrinceCustomerContext, contextAccessor, apiResponse,
        apiBadRequestResponse,
        apiOkResponse, iTenantProvider)
    {
        _iPsRepository = iPsRepository;
        _psParameter = psParameter;
        _pmolParameter = PmolParameter;
        _logger = logger;
    }


    [HttpPost("GetPsDropdownData")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetPsDropdownData([FromBody] PsHeaderDropdowndto mPsHeaderDropdowndto)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = mPsHeaderDropdowndto.ProjectSequenceCode;
            _pmolParameter.Lang = lang;
            _pmolParameter.TenantProvider = ItenantProvider;
            var dropDownData = await _iPsRepository.GetDropdownData(_pmolParameter);

            return Ok(new ApiOkResponse(dropDownData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadTitle/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetTitleById(string id)
    {
        // _logger.LogError("GetTitleById " + id);
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());


            _psParameter.ApplicationDbContext = UPrinceCustomerContext;
            _psParameter.PsId = id;
            _psParameter.Lang = lang;
            _psParameter.TenantProvider = ItenantProvider;
            var psTitle = await _iPsRepository.GetPsTitleById(_psParameter);
            return Ok(new ApiOkResponse(psTitle));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CreatePsHeader")]
    public async Task<ActionResult> CreatePsHeader([FromBody] PsHeaderCreateDto dto)
    {
        try
        {
            //_logger.LogTrace("Started");
            // _logger.LogError("CreatePsHeader" + JsonToStringConverter.getStringFromJson(dto));
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var parameter = new PsParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                PsHeaderCreateDto = dto,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                ApplicationDbContext = UPrinceCustomerContext,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };
            return Ok(new ApiOkResponse(await _iPsRepository.CreatePsHeader(parameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("PsFilter")]
    public async Task<ActionResult> PsFilter([FromBody] PsFilter dto)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var parameter = new PsParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                Filter = dto,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider
            };
            var data = await _iPsRepository.PsFilter(parameter);


            if (!data.Any())
            {
                var mApiResponse = new ApiOkResponse(null, "noProgressavailable")
                {
                    Status = false
                };
                return Ok(mApiResponse);
            }

            var mApiOkResponse = new ApiOkResponse(data);
            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("Read/{id}")]
    public async Task<ActionResult> Read(string id)
    {
        // _logger.LogTrace("Started");
        // _logger.LogError("Read" + id);
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var parameter = new PsParameter();
            parameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            parameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            parameter.ApplicationDbContext = UPrinceCustomerContext;
            parameter.PsId = id;
            parameter.Lang = lang;
            parameter.TenantProvider = ItenantProvider;
            var data = await _iPsRepository.ReadPsById(parameter);
            return Ok(new ApiOkResponse(data));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadExcel/{id}")]
    public async Task<ActionResult> ReadExcel(string id)
    {
        // _logger.LogTrace("Started");
        // _logger.LogError("Read" + id);
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var parameter = new PsParameter();
            parameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            parameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            parameter.ApplicationDbContext = UPrinceCustomerContext;
            parameter.PsId = id;
            parameter.Lang = lang;
            parameter.TenantProvider = ItenantProvider;
            var data = await _iPsRepository.ReadExcel(parameter);
            return Ok(new ApiOkResponse(data));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CreatePsResource")]
    public async Task<ActionResult> CreatePsResource([FromBody] PsResourceCreateDto dto)
    {
        try
        {
            // _logger.LogTrace("Started");
            // _logger.LogError("CreatePsResource" + JsonToStringConverter.getStringFromJson(dto));
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var parameter = new PsParameter();
            parameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            parameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            parameter.Lang = lang;
            parameter.PsResourceCreateDto = dto;
            parameter.ContextAccessor = ContextAccessor;
            parameter.TenantProvider = ItenantProvider;
            parameter.ApplicationDbContext = UPrinceCustomerContext;
            parameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var id = await _iPsRepository.CreatePsResource(parameter);
            return Ok(new ApiOkResponse(id));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPut("ApprovePs/{id}")]
    public async Task<ActionResult> ApprovePs(string id)
    {
        //_logger.LogTrace("Started");
        //_logger.LogError("ApprovePs " + id);
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var parameter = new PsParameter();
            parameter.ApplicationDbContext = UPrinceCustomerContext;
            parameter.PsId = id;
            parameter.Lang = lang;
            parameter.TenantProvider = ItenantProvider;
            parameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            parameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            parameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var data = await _iPsRepository.ApprovePs(parameter);
            return Ok(new ApiOkResponse(data));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPut("CUApprovePs/{id}")]
    public async Task<ActionResult> CUApprovePs(string id)
    {
        //_logger.LogTrace("Started");
        //_logger.LogError("ApprovePs " + id);
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var parameter = new PsParameter();
            parameter.ApplicationDbContext = UPrinceCustomerContext;
            parameter.PsId = id;
            parameter.Lang = lang;
            parameter.TenantProvider = ItenantProvider;
            parameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            parameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            var data = await _iPsRepository.CUApprovePs(parameter);
            return Ok(new ApiOkResponse(data));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}