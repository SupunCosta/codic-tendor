using System;
using System.ComponentModel.DataAnnotations.Schema;
using UPrinceV4.Web.Data.PS;

namespace UPrinceV4.Web.Data.INV;

public class Invoice
{
    public string Id { get; set; }
    public string InvoiceId { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public string InvoiceStatusId { get; set; }
    [ForeignKey("PsHeader")] public string PsId { get; set; }
    public virtual PsHeader Ps { get; set; }
    public DateTime? WorkPeriodFrom { get; set; }
    public DateTime? WorkPeriodTo { get; set; }
    public DateTime? Date { get; set; }
    public string PurchaseOrderNumber { get; set; }
    public double? TotalIncludingTax { get; set; }
    public double? TotalExcludingTax { get; set; }
    public string ProjectSequenceCode { get; set; }
    public string ProductTitle { get; set; }
}

public class InvoiceFilterDto
{
    public string Id { get; set; }
    public string InvoiceId { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public string InvoiceStatusId { get; set; }
    public string InvoiceStatus { get; set; }
    public string PsId { get; set; }
    public DateTime? WorkPeriodFrom { get; set; }
    public DateTime? WorkPeriodTo { get; set; }
    public DateTime? Date { get; set; }
    public string PurchaseOrderNumber { get; set; }
    public double? TotalIncludingTax { get; set; }
    public double? TotalExcludingTax { get; set; }
    public string ProductTitle { get; set; }
    public string ProjectSequenceCode { get; set; }
}

public class InvoicedDto
{
    public string BorId { get; set; }
    public string SoldQuantity { get; set; }
    public string CpcResourceNumber { get; set; }
    public string CPCId { get; set; }
}