using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.Contractor;
using UPrinceV4.Web.Data.PdfToExcel;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Controllers.PdfToExcel;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PdfToExcelController : CommonConfigurationController
{
    private readonly GraphServiceClient _graphServiceClient;
    private readonly PdfToExcelParameter _pdfToExcelParameter;
    private readonly IPdfToExcelRepository _PdfToExcelRepository;
    private readonly ITenantProvider _TenantProvider;
    private readonly IConfiguration _iConfiguration;




    public PdfToExcelController(ITenantProvider tenantProvider,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse,
        ITenantProvider iTenantProvider
        , IPdfToExcelRepository iPdfToExcelRepository,
        GraphServiceClient graphServiceClient,IConfiguration iConfiguration)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)
    {
        _TenantProvider = tenantProvider;
        _PdfToExcelRepository = iPdfToExcelRepository;
        _pdfToExcelParameter = new PdfToExcelParameter();
        _graphServiceClient = graphServiceClient;
        _iConfiguration = iConfiguration;
    }

    
    [HttpPost("PdfToExcel")]
    public async Task<ActionResult> TestVpPo([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromForm] IFormCollection pdf, [FromForm] string pdfType)
    {
        try
        {
            var lang = langCode(langX);
            _pdfToExcelParameter.ContractingUnitSequenceId = CU;
            _pdfToExcelParameter.ProjectSequenceId = Project;
            _pdfToExcelParameter.Lang = lang;
            _pdfToExcelParameter.PdfType = pdfType;
            _pdfToExcelParameter.ContextAccessor = ContextAccessor;
            _pdfToExcelParameter.TenantProvider = _TenantProvider;
            var client = new FileClient();
            var url = client.PersistPhotoInNewFolder(pdf.Files.FirstOrDefault()?.FileName, _TenantProvider
                , pdf.Files.FirstOrDefault(), "PDFDocuments");
            _pdfToExcelParameter.URL = url;
            return Ok(new ApiOkResponse(await _PdfToExcelRepository.PdfToExcel(_pdfToExcelParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("PdfToExcelTest")]
    public async Task<ActionResult> PdfToExcelTest([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromForm] IFormCollection pdf, [FromForm] string LotId,
        [FromForm] string ContractorId)
    {
        try
        {
            _pdfToExcelParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var file = pdf.Files.FirstOrDefault();

            var lang = langCode(langX);
            _pdfToExcelParameter.ContractingUnitSequenceId = CU;
            _pdfToExcelParameter.ProjectSequenceId = Project;
            _pdfToExcelParameter.Lang = lang;
            _pdfToExcelParameter.ContextAccessor = ContextAccessor;
            _pdfToExcelParameter.TenantProvider = _TenantProvider;
            _pdfToExcelParameter.lotId = LotId;
            _pdfToExcelParameter.ContractorId = ContractorId;
            var client = new FileClient();
            var url = client.PersistPhotoInNewFolder(pdf.Files.FirstOrDefault()?.FileName, _TenantProvider
                , pdf.Files.FirstOrDefault(), "BOR Documents");
            _pdfToExcelParameter.URL = url;
            _pdfToExcelParameter.File = file;
            _pdfToExcelParameter.GraphServiceClient = _graphServiceClient;
            return Ok(new ApiOkResponse(await _PdfToExcelRepository.PdfToExcelTest(_pdfToExcelParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetContractorPdfData/{LotId}")]
    public async Task<ActionResult> GetContractorPdfData([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, string LotId)
    {
        try
        {
            _pdfToExcelParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var lang = langCode(langX);
            _pdfToExcelParameter.ContractingUnitSequenceId = CU;
            _pdfToExcelParameter.ProjectSequenceId = Project;
            _pdfToExcelParameter.Lang = lang;
            _pdfToExcelParameter.ContextAccessor = ContextAccessor;
            _pdfToExcelParameter.TenantProvider = _TenantProvider;
            _pdfToExcelParameter.Id = LotId;
            _pdfToExcelParameter.Configuration = _iConfiguration;
            return Ok(new ApiOkResponse(await _PdfToExcelRepository.GetContractorPdfData(_pdfToExcelParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetContractorPdfDataFilterContractor/{LotId}")]
    public async Task<ActionResult> GetContractorPdfDataFilterContractor([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, string LotId)
    {
        try
        {
            _pdfToExcelParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var lang = langCode(langX);
            _pdfToExcelParameter.ContractingUnitSequenceId = CU;
            _pdfToExcelParameter.ProjectSequenceId = Project;
            _pdfToExcelParameter.Lang = lang;
            _pdfToExcelParameter.ContextAccessor = ContextAccessor;
            _pdfToExcelParameter.TenantProvider = _TenantProvider;
            _pdfToExcelParameter.Id = LotId;
            return Ok(new ApiOkResponse(
                await _PdfToExcelRepository.GetContractorPdfDataFilterContractor(_pdfToExcelParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("GetContractorPdfRowData")]
    public async Task<ActionResult> GetContractorPdfRowData([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromBody] GetContractorPdfRowData rowData)
    {
        try
        {
            var lang = langCode(langX);
            _pdfToExcelParameter.ContractingUnitSequenceId = CU;
            _pdfToExcelParameter.ProjectSequenceId = Project;
            _pdfToExcelParameter.Lang = lang;
            _pdfToExcelParameter.ContextAccessor = ContextAccessor;
            _pdfToExcelParameter.TenantProvider = _TenantProvider;
            _pdfToExcelParameter.lotId = rowData.LotId;
            _pdfToExcelParameter.ContractorId = rowData.ContractorId;

            return Ok(new ApiOkResponse(await _PdfToExcelRepository.GetContractorPdfRowData(_pdfToExcelParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ContractorPdfErrorLogGetByLotId/{LotId}")]
    public async Task<ActionResult> ContractorPdfErrorLogGetByLotId([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, string LotId)
    {
        try
        {
            _pdfToExcelParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var lang = langCode(langX);
            _pdfToExcelParameter.ContractingUnitSequenceId = CU;
            _pdfToExcelParameter.ProjectSequenceId = Project;
            _pdfToExcelParameter.Lang = lang;
            _pdfToExcelParameter.ContextAccessor = ContextAccessor;
            _pdfToExcelParameter.TenantProvider = _TenantProvider;
            _pdfToExcelParameter.Id = LotId;
            _pdfToExcelParameter.Configuration = _iConfiguration;
            return Ok(new ApiOkResponse(
                await _PdfToExcelRepository.ContractorPdfErrorLogGetByLotId(_pdfToExcelParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ContractorPdfErrorLogGetByLotIdFilterContractor/{LotId}")]
    public async Task<ActionResult> ContractorPdfErrorLogGetByLotIdFilterContractor([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, string LotId)
    {
        try
        {
            _pdfToExcelParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var lang = langCode(langX);
            _pdfToExcelParameter.ContractingUnitSequenceId = CU;
            _pdfToExcelParameter.ProjectSequenceId = Project;
            _pdfToExcelParameter.Lang = lang;
            _pdfToExcelParameter.ContextAccessor = ContextAccessor;
            _pdfToExcelParameter.TenantProvider = _TenantProvider;
            _pdfToExcelParameter.Id = LotId; 
            _pdfToExcelParameter.Configuration = _iConfiguration;
            return Ok(new ApiOkResponse(
                await _PdfToExcelRepository.ContractorPdfErrorLogGetByLotIdFilterContractor(_pdfToExcelParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("SaveContractorPdfData")]
    public async Task<ActionResult> SaveContractorPdfData([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromBody] SaveCBCExcelLotDataDto pdfData)
    {
        try
        {
            _pdfToExcelParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var lang = langCode(langX);
            _pdfToExcelParameter.ContractingUnitSequenceId = CU;
            _pdfToExcelParameter.ProjectSequenceId = Project;
            _pdfToExcelParameter.Lang = lang;
            _pdfToExcelParameter.ContextAccessor = ContextAccessor;
            _pdfToExcelParameter.TenantProvider = _TenantProvider;
            _pdfToExcelParameter.lotId = pdfData.LotId;
            _pdfToExcelParameter.ExcelLotDataDtoTest = pdfData;
            _pdfToExcelParameter.Configuration = _iConfiguration;

            return Ok(new ApiOkResponse(await _PdfToExcelRepository.SaveContractorPdfData(_pdfToExcelParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("SaveUploadedPdfData")]
    public async Task<ActionResult> SaveUploadedPdfData([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromBody] SaveUploadedPdfData pdfData)
    {
        try
        {
            _pdfToExcelParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var lang = langCode(langX);
            _pdfToExcelParameter.ContractingUnitSequenceId = CU;
            _pdfToExcelParameter.ProjectSequenceId = Project;
            _pdfToExcelParameter.Lang = lang;
            _pdfToExcelParameter.ContextAccessor = ContextAccessor;
            _pdfToExcelParameter.TenantProvider = _TenantProvider;
            _pdfToExcelParameter.lotId = pdfData.ContractId;
            _pdfToExcelParameter.pdfData = pdfData;

            return Ok(new ApiOkResponse(await _PdfToExcelRepository.SaveUploadedPdfData(_pdfToExcelParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("ContractorLotExcelUpload")]
    public async Task<ActionResult> ContractorLotExcelUpload([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromBody] SaveUploadedPdfData pdfData)
    {
        try
        {
            _pdfToExcelParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var lang = langCode(langX);
            _pdfToExcelParameter.ContractingUnitSequenceId = CU;
            _pdfToExcelParameter.ProjectSequenceId = Project;
            _pdfToExcelParameter.Lang = lang;
            _pdfToExcelParameter.ContextAccessor = ContextAccessor;
            _pdfToExcelParameter.TenantProvider = _TenantProvider;
            _pdfToExcelParameter.lotId = pdfData.ContractId;
            _pdfToExcelParameter.pdfData = pdfData;

            return Ok(new ApiOkResponse(await _PdfToExcelRepository.ContractorLotExcelUpload(_pdfToExcelParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}