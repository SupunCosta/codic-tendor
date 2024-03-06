using System;

namespace UPrinceV4.Web.Data.PdfToExcel;

public class PublishedContractorsPdfData
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
    public string Key1 { get; set; }
    public string Value1 { get; set; }
    public string Key2 { get; set; }
    public string Value2 { get; set; }
    public string Key3 { get; set; }
    public string Value3 { get; set; }
    public string Key4 { get; set; }
    public string Value4 { get; set; }
    public string Key5 { get; set; }
    public string Value5 { get; set; }
    public bool IsExclude { get; set; }
    public string MeasurementCode { get; set; }
    public string RealArticleNo { get; set; }

}