using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UPrinceV4.Web.Data.CAB;

namespace UPrinceV4.Web.Data.CPC;

public class CpcVendor
{
    [Key] public string Id { get; set; }

    //public string CompanyName { get; set; }
    public string ResourceNumber { get; set; }
    public string ResourceTitle { get; set; }
    public string PurchasingUnit { get; set; }
    public double? ResourcePrice { get; set; }
    public string ResourceLeadTime { get; set; }
    public double? MinOrderQuantity { get; set; }
    public double? MaxOrderQuantity { get; set; }
    public double? RoundingValue { get; set; }

    [ForeignKey("CorporateProductCatalog")]
    public string CoperateProductCatalogId { get; set; }

    public virtual CorporateProductCatalog CorporateProductCatalog { get; set; }

    // [ForeignKey("CabCompany")]
    public string CompanyId { get; set; }

    // public virtual CabCompany Company { get; set; }
    [NotMapped] public CompanyDto Company { get; set; }
    public bool IsDeleted { get; set; }
    public bool PreferredParty { get; set; }
}

public class CpcVendorCreateDto
{
    public string Id { get; set; }
    public string CompanyId { get; set; }
    public string CompanyName { get; set; }
    public string ResourceNumber { get; set; }
    public string ResourceTitle { get; set; }
    public string PurchasingUnit { get; set; }
    public double? ResourcePrice { get; set; }
    public string ResourceLeadTime { get; set; }
    public double? MinOrderQuantity { get; set; }
    public double? MaxOrderQuantity { get; set; }
    public double? RoundingValue { get; set; }
    public bool PreferredParty { get; set; }
    public string CoperateProductCatalogId { get; set; }
}