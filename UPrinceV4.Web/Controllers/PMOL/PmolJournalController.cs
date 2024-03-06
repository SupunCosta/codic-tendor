using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.PMOL;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces.PMOL;

namespace UPrinceV4.Web.Controllers.PMOL;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class PmolJournalController : CommonConfigurationController
{
    private readonly IPmolJournalRepository _iPmolRepository;
    private readonly ITenantProvider _TenantProvider;

    public PmolJournalController(ITenantProvider tenantProvider, IPmolJournalRepository iPmolRepository,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse,
        ITenantProvider iTenantProvider
    )
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)

    {
        _iPmolRepository = iPmolRepository;
        _TenantProvider = tenantProvider;
    }

    [HttpPost("UploadJournalDocuments")]
    public async Task<ActionResult<string>> UploadJournalDocuments([FromForm] IFormCollection image)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }
            

            var client = new FileClient();
            var url = client.PersistPhotoInNewFolder(image.Files.FirstOrDefault()?.FileName, _TenantProvider
                , image.Files.FirstOrDefault(), "Journal Documents");

            var response = new ApiOkResponse(url);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CreateJournal")]
    public async Task<ActionResult> CreateJournal([FromBody] PmolJournalCreateDto pmolDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            PmolJournalParameter _pmolParameter = new PmolJournalParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                PmolDto = pmolDto,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider
            };
            return Ok(new ApiOkResponse(await _iPmolRepository.CreateJournal(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadJournal/{id}")]
    public async Task<ActionResult> ReadJournal(string id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            PmolJournalParameter _pmolParameter = new PmolJournalParameter();
            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameter.Lang = lang;
            _pmolParameter.Id = id;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = ItenantProvider;

            return Ok(new ApiOkResponse(await _iPmolRepository.ReadJournal(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadJournalDoneWork/{id}")]
    public async Task<ActionResult> ReadJournalDoneWork(string id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            PmolJournalParameter _pmolParameter = new PmolJournalParameter();
            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameter.Lang = lang;
            _pmolParameter.Id = id;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = ItenantProvider;
            return Ok(new ApiOkResponse(await _iPmolRepository.ReadJournalDoneWork(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadJournalEncounteredProblem/{id}")]
    public async Task<ActionResult> ReadJournalEncounteredProblem(string id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            PmolJournalParameter _pmolParameter = new PmolJournalParameter();
            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameter.Lang = lang;
            _pmolParameter.Id = id;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = ItenantProvider;

            return Ok(new ApiOkResponse(await _iPmolRepository.ReadJournalEncounteredProblem(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadJournalLessonsLearned/{id}")]
    public async Task<ActionResult> ReadJournalLessonsLearned(string id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            PmolJournalParameter _pmolParameter = new PmolJournalParameter();
            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameter.Lang = lang;
            _pmolParameter.Id = id;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = ItenantProvider;

            return Ok(new ApiOkResponse(await _iPmolRepository.ReadJournalLessonsLearned(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadJournalReportedThings/{id}")]
    public async Task<ActionResult> ReadJournalReportedThings(string id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            PmolJournalParameter _pmolParameter = new PmolJournalParameter();
            //string message = ApiErrorMessages.GetErrorMessage(ItenantProvider, ErrorMessageKey.Ok, lang).Message;
            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameter.Lang = lang;
            _pmolParameter.Id = id;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = ItenantProvider;
            return Ok(new ApiOkResponse(await _iPmolRepository.ReadJournalReportedThings(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("UploadJournalDocumentsForMobile")]
    public async Task<ActionResult<string>> UploadJournalDocumentsForMobile([FromForm] IFormCollection image)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }
            PmolJournalParameter _pmolParameter = new PmolJournalParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                formData = image,
                TenantProvider = ItenantProvider
            };
            var uu = await _iPmolRepository.UploadJournalPictureForMobile(_pmolParameter);
            //return Ok(new ApiOkResponse(_iPmolRepository.UploadJournalPictureForMobile(_pmolParameter)));
            return Ok(new ApiOkResponse(uu));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
}