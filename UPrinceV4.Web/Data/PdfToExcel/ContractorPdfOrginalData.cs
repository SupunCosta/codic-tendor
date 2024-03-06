using System;

namespace UPrinceV4.Web.Data.PdfToExcel;

public class ContractorPdfOrginalData
{
    public string Id { get; set; }
    public string ArticleNo { get; set; }
    public string CompanyId { get; set; }
    public string Title { get; set; }
    public string Unit { get; set; }
    public string VH { get; set; }
    public string Quantity { get; set; }
    public float? UnitPrice { get; set; }
    public float? TotalPrice { get; set; }
    public string LotId { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string PageRowColumn { get; set; }
    public string PageRow { get; set; }
    public string LotRowNumber { get; set; }
}