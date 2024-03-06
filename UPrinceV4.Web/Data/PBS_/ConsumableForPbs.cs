using System.ComponentModel.DataAnnotations.Schema;
using UPrinceV4.Web.Data.CPC;

namespace UPrinceV4.Web.Data.PBS_;

public class ConsumableForPbs
{
    public string Id { get; set; }

    [ForeignKey("PbsProduct")] public string PbsProductId { get; set; }
    public virtual PbsProduct PbsProduct { get; set; }

    // [ForeignKey("CorporateProductCatalog")]
    public string CoperateProductCatalogId { get; set; }
    [NotMapped] public CorporateProductCatalog CorporateProductCatalog { get; set; }
    public double Quantity { get; set; }
}

public class ConsumableForPbsCreateDto
{
    public string ResourceId { get; set; }
    public string PbsProductId { get; set; }
    public string CoperateProductCatalogId { get; set; }
    public double Quantity { get; set; }
    public string Environment { get; set; }
}