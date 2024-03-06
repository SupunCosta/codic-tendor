using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.CPC;

public class CpcImage
{
    public string Id { get; set; }

    [ForeignKey("CorporateProductCatalog")]
    public string CoperateProductCatalogId { get; set; }

    public virtual CorporateProductCatalog CorporateProductCatalog { get; set; }
    public string Image { get; set; }
    public bool IsDeleted { get; set; }
}

public class CpcImageCreateDto
{
    public string Id { get; set; }
    public string Image { get; set; }
}