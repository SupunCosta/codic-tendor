using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.CPC;

public class CpcResourceTypeLocalizedData
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string LanguageCode { get; set; }

    [ForeignKey("CpcResourceType")] public string CpcResourceTypeId { get; set; }
}

public class GetCpcResourceTypeLocalizedDataByCode
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string LanguageCode { get; set; }
    public string CpcResourceTypeId { get; set; }
    public string Name { get; set; }
}

public class UpdateCpcResourceTypeLocalizedDataDto
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string LanguageCode { get; set; }
}

public class AddCpcResourceTypeLocalizedDataDto
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string LanguageCode { get; set; }
    public string CpcResourceTypeId { get; set; }
    public string Name { get; set; }
}