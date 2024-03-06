using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.RFQ;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Repositories.Interfaces.BOR;
using UPrinceV4.Web.Repositories.Interfaces.PBS;
using UPrinceV4.Web.Repositories.Interfaces.PMOL;
using UPrinceV4.Web.Repositories.Interfaces.PO;
using UPrinceV4.Web.Repositories.Interfaces.RFQ;

namespace UPrinceV4.Web.Controllers.RFQ;

[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class RFQController : CommonConfigurationController
{
    private readonly IBorRepository _iBorRepository;
    private readonly IPbsResourceRepository _iPbsResourceRepository;
    private readonly IPmolRepository _iPmolRepository;
    private readonly IPORepository _iPORepository;
    private readonly IProjectDefinitionRepository _iProjectDefinitionRepository;
    private readonly IRFQRepository _iRFQRepository;
    private readonly IShiftRepository _iShiftRepository;
    private readonly RFQParameter _rfqParameter;
    private readonly ITenantProvider _TenantProvider;


    public RFQController(ITenantProvider tenantProvider,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider,
        IRFQRepository iRFQRepository, IPmolRepository iPmolRepository, IConfiguration iConfiguration,
        IPORepository iPoRepository,
        IPbsResourceRepository iPbsResourceRepository, IBorRepository iBorRepository,
        IProjectDefinitionRepository iProjectDefinitionRepository,
        IShiftRepository iShiftRepository)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)

    {
        _rfqParameter = new RFQParameter();
        _TenantProvider = tenantProvider;
        _iPmolRepository = iPmolRepository;
        _iConfiguration = iConfiguration;
        _iPbsResourceRepository = iPbsResourceRepository;
        _iBorRepository = iBorRepository;
        _iProjectDefinitionRepository = iProjectDefinitionRepository;
        _iShiftRepository = iShiftRepository;
        _iRFQRepository = iRFQRepository;
        _iPORepository = iPoRepository;
    }

    private IConfiguration _iConfiguration { get; }

    [Authorize]
    [HttpPost("SendRfqEmail")]
    public async Task<ActionResult> SendRfqEmail([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project, [FromHeader(Name = "lang")] string langX,
        [FromBody] List<string> IdList)
    {
        try
        {
            var lang = langCode(langX);
            _rfqParameter.ContractingUnitSequenceId = CU;
            _rfqParameter.ProjectSequenceId = Project;
            _rfqParameter.Lang = lang;
            _rfqParameter.ContextAccessor = ContextAccessor;
            _rfqParameter.TenantProvider = _TenantProvider;
            _rfqParameter.IdList = IdList;
            _rfqParameter.Configuration = _iConfiguration;

            var result = await _iRFQRepository.SendRfqEmail(_rfqParameter);

            return Ok(new ApiOkResponse(result));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    //[Authorize]
    [HttpPost("AcceptRfqEmail")]
    public async Task<ActionResult> AcceptRfqEmail([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project, [FromHeader(Name = "lang")] string langX,
        [FromForm] string rfqAccept, [FromForm] IFormCollection pdf)
    {
        try
        {
            var myObj = JsonConvert.DeserializeObject<RfqAccept>(rfqAccept);

            var file = pdf.Files.FirstOrDefault();
            _rfqParameter.File = file;
            var lang = langCode(langX);
            _rfqParameter.ContractingUnitSequenceId = CU;
            _rfqParameter.ProjectSequenceId = Project;
            _rfqParameter.Lang = lang;
            _rfqParameter.ContextAccessor = ContextAccessor;
            _rfqParameter.TenantProvider = _TenantProvider;
            _rfqParameter.RfqAccept = myObj;
            _rfqParameter.Configuration = _iConfiguration;

            var result = await _iRFQRepository.AcceptRfqEmail(_rfqParameter);

            return Ok(new ApiOkResponse(result));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    //[Authorize]
    [HttpPost("UploadRfqSignature")]
    public async Task<ActionResult<string>> UploadRfqSignature([FromForm] IFormCollection image,
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
            var url = client.PersistPhotoInNewFolder(image.Files.FirstOrDefault()?.FileName, _TenantProvider
                , image.Files.FirstOrDefault(), "Rfq Signatures");


            var response = new ApiOkResponse(url);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    //[Authorize]
    [HttpGet("ReadRfqById/{SequenceId}")]
    public async Task<ActionResult> ReadRfqById(string SequenceId, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _poParameter = new POParameter();
            _poParameter.ContractingUnitSequenceId = CU;
            _poParameter.ProjectSequenceId = Project;
            _poParameter.Lang = lang;
            _poParameter.Id = SequenceId;
            _poParameter.ContextAccessor = ContextAccessor;
            _poParameter.TenantProvider = ItenantProvider;
            _poParameter.Configuration = _iConfiguration;
            return Ok(new ApiOkResponse(await _iPORepository.GetPOById(_poParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}