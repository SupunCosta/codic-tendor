namespace UPrinceV4.Web.Data.CPC;

public class CpcMaterialLocalizedData
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string LanguageCode { get; set; }

    public string MaterialId { get; set; }
}

public class GetCpcMaterialLocalizedDataByCode
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string LanguageCode { get; set; }
    public string CpcMaterialId { get; set; }
    public string Name { get; set; }
}

public class UpdateCpcMaterialLocalizedDataDto
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string LanguageCode { get; set; }
}

public class AddCpcMaterialLocalizedDataDto
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string LanguageCode { get; set; }
    public string CpcMaterialId { get; set; }
    public string Name { get; set; }
}