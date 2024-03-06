using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.CPC;

public class CpcResourceNickname
{
    public string Id { get; set; }
    public string NickName { get; set; }
    public string Language { get; set; }
    public string LocaleCode { get; set; }

    [ForeignKey("CorporateProductCatalog")]
    public string CoperateProductCatalogId { get; set; }

    public virtual CorporateProductCatalog CorporateProductCatalog { get; set; }
    public bool IsDeleted { get; set; }
}

public class CpcResourceNicknameCreateDto
{
    public string Id { get; set; }
    public string NickName { get; set; }
    public string Language { get; set; }
    public string LocaleCode { get; set; }
    public string CoperateProductCatalogId { get; set; }
}