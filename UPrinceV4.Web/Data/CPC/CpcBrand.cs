namespace UPrinceV4.Web.Data.CPC;

public class CpcBrand
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LocaleCode { get; set; }
    public string LanguageCode { get; set; }
    public string CpcBrandId { get; set; }
}

public class GetCpcBrandByCode
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Label { get; set; }
    public string CpcBrandId { get; set; }
    public string LanguageCode { get; set; }
}

public class UpdateCpcBrandDto
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string Name { get; set; }
    public string LanguageCode { get; set; }
}

public class CpcBrandDto
{
    public string Key { get; set; }
    public string Text { get; set; }
}