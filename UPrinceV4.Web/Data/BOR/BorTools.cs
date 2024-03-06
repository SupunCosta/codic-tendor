using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.BOR;

public class BorTools
{
    public string Id { get; set; }

    [ForeignKey("BorProduct")] public string BorProductId { get; set; }

    public virtual Bor BorProduct { get; set; }

    // [ForeignKey("CorporateProductCatalog")]
    public string CorporateProductCatalogId { get; set; }

    // public virtual CorporateProductCatalog CorporateProductCatalog { get; set; }
    public DateTime Date { get; set; }
    public double Required { get; set; }
    public double Purchased { get; set; }
    public double DeliveryRequested { get; set; }
    public double Warf { get; set; }
    public double Consumed { get; set; }
    public double Invoiced { get; set; }
    public string Source { get; set; }
    public double TotalRequired { get; set; }
    public DateTime? ActualDeliveryDate { get; set; } = null;
    public DateTime? ExpectedDeliveryDate { get; set; } = null;
    public DateTime? RequestedDeliveryDate { get; set; } = null;
    public double Returned { get; set; }
}