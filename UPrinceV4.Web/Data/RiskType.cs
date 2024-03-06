namespace UPrinceV4.Web.Data;

public class RiskType
{
    public string Id { get; set; }
    public string Type { get; set; }
    public string LanguageCode { get; set; }
    public string RiskTypeId { get; set; }
}

public class RiskTypeDto
{
    public string Id { get; set; }
    public string Type { get; set; }
}

public class RiskTypeDapperDto
{
    public string Key { get; set; }
    public string Text { get; set; }
}