using System;

namespace UPrinceV4.Web.Data.Contractor;

public class ContractorPsPublished
{
    public string Id { get; set; }
    public string ArticleNumber { get; set; }
    public string Title { get; set; }
    public string MeasurementCode { get; set; }
    public string QuantityQuotation { get; set; }
    public string UnitPrice { get; set; }
    public string QuantityConsumed { get; set; }
    public string Total { get; set; }
    public string LotId { get; set; }
    public string CompanyId { get; set; }
    public string PsSequenceId { get; set; }
    public string PsOrderNumber { get; set; }
    public bool IsApproved { get; set; } = false;
    public DateTime? ApprovedDate { get; set; }
}