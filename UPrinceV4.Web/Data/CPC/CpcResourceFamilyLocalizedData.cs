namespace UPrinceV4.Web.Data.CPC;

public class CpcResourceFamilyLocalizedData
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string LanguageCode { get; set; }

    //[ForeignKey("CpcResourceFamily")]
    public string CpcResourceFamilyId { get; set; }
    public string ParentId { get; set; }
}