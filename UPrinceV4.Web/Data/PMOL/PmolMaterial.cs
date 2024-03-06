using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.PMOL;

public class PmolPlannedWorkMaterial
{
    public string Id { get; set; }

    // [ForeignKey("CorporateProductCatalog")]
    public string CoperateProductCatalogId { get; set; }

    // public virtual CorporateProductCatalog CorporateProductCatalog { get; set; }
    public double RequiredQuantity { get; set; }
    public double ConsumedQuantity { get; set; }
    public string CpcBasicUnitofMeasureId { get; set; }
    [ForeignKey("Pmol")] public string PmolId { get; set; }
    public virtual Pmol Pmol { get; set; }
    public string Type { get; set; }
    public bool IsDeleted { get; set; }
}