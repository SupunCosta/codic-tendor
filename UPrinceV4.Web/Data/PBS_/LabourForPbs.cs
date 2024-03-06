using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UPrinceV4.Web.Data.CPC;

namespace UPrinceV4.Web.Data.PBS_;

public class LabourForPbs
{
    public string Id { get; set; }

    [ForeignKey("PbsProduct")] public string PbsProductId { get; set; }
    public virtual PbsProduct PbsProduct { get; set; }

    //[ForeignKey("CorporateProductCatalog")]
    public string CoperateProductCatalogId { get; set; }
    [NotMapped] public CorporateProductCatalog CorporateProductCatalog { get; set; }
    public double Quantity { get; set; }
}

public class LabourForPbsCreateDto
{
    public string ResourceId { get; set; }
    [Required] public string PbsProductId { get; set; }
    [Required] public string CoperateProductCatalogId { get; set; }
    public double Quantity { get; set; }
    public string Environment { get; set; }
}

public class GetLabourForPbsDto
{
    public string Id { get; set; }
    public string PbsProductId { get; set; }
    public string PbsTitle { get; set; }
    public string CorporateProductCatalogId { get; set; }
    public double Quantity { get; set; }
    public string Title { get; set; }
    public string ResourceNumber { get; set; }
    public string BorId { get; set; }
    public string BorTitle { get; set; }
    public double InventoryPrice { get; set; }
    public string Mou { get; set; }
    public DateTime? Date { get; set; } = null;
    public double Required { get; set; }
    public double Purchased { get; set; }
    public double Consumed { get; set; }
    public double Invoiced { get; set; }
    public string ResourceFamily { get; set; }
    public double returned { get; set; }
}