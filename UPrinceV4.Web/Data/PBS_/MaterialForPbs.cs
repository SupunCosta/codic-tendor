using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UPrinceV4.Web.Data.CPC;

namespace UPrinceV4.Web.Data.PBS_;

public class MaterialForPbs
{
    public string Id { get; set; }

    [ForeignKey("PbsProduct")] public string PbsProductId { get; set; }
    public virtual PbsProduct PbsProduct { get; set; }

    //  [ForeignKey("CorporateProductCatalog")]
    public string CoperateProductCatalogId { get; set; }
    [NotMapped] public CorporateProductCatalog CorporateProductCatalog { get; set; }
    public double Quantity { get; set; }
}

public class MaterialForPbsCreateDto
{
    public string ResourceId { get; set; }
    [Required] public string PbsProductId { get; set; }
    [Required] public string CoperateProductCatalogId { get; set; }
    public double Quantity { get; set; }
    public string Environment { get; set; }
}