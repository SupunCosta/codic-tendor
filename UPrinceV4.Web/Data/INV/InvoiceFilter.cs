using System;

namespace UPrinceV4.Web.Data.INV;

public class InvoiceFilter
{
    public string Title { get; set; }
    public string InvoiceStatusId { get; set; }
    public DateTime? Date { get; set; }
    public double? TotalIncludingTax { get; set; }
    public double? TotalExcludingTax { get; set; }
    public Sorter Sorter { get; set; }
    public string ProjectSequenceCode { get; set; }
}