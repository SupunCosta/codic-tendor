using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data.Contractor;
using UPrinceV4.Web.Data.PdfToExcel;
using UPrinceV4.Web.Data.VisualPlaane;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IPdfToExcelRepository
{
    public Task<AnalyzeResult> PdfToExcel(PdfToExcelParameter PdfToExcelParameter);
    public Task<ContractorPdfDataResults> PdfToExcelTest(PdfToExcelParameter pdfToExcelParameter);
    public Task<List<GetContractorPdfData>> GetContractorPdfData(PdfToExcelParameter pdfToExcelParameter);
    public Task<string> GetContractorPdfRowData(PdfToExcelParameter PdfToExcelParameter);
    public Task<List<ContractorPdfErrorLog>> ContractorPdfErrorLogGetByLotId(PdfToExcelParameter PdfToExcelParameter);
    public Task<string> SaveContractorPdfData(PdfToExcelParameter PdfToExcelParameter);

    public Task<List<ContractorPdfErrorLog>> ContractorPdfErrorLogGetByLotIdFilterContractor(
        PdfToExcelParameter PdfToExcelParameter);

    public Task<List<GetContractorPdfData>> GetContractorPdfDataFilterContractor(
        PdfToExcelParameter pdfToExcelParameter);

    public Task<string> SaveUploadedPdfData(PdfToExcelParameter PdfToExcelParameter);
    public Task<string> ContractorLotExcelUpload(PdfToExcelParameter PdfToExcelParameter);
}

public class PdfToExcelParameter
{
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public string Title { get; set; }
    public string Id { get; set; }
    public string UserId { get; set; }
    public List<string> idList { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    
    public TestVp TestVp { get; set; }
    public string PdfType { get; set; }
    public string URL { get; set; }

    public AnalyzeResult AnalyzeResult { get; set; }
    public string lotId { get; set; }
    public string ContractorId { get; set; }
    public GraphServiceClient GraphServiceClient { get; set; }
    public IFormFile File { get; set; }
    public SaveCBCExcelLotDataDto ExcelLotDataDtoTest { get; set; }
    public SaveUploadedPdfData pdfData { get; set; }
    public IConfiguration Configuration { get; set; }
}