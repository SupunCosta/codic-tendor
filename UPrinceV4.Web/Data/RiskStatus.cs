namespace UPrinceV4.Web.Data;

public class RiskStatus
{
    public string Id { get; set; }
    public string Status { get; set; }
    public string LanguageCode { get; set; }
    public string RiskStatusId { get; set; }
}

public class RiskStatusDto
{
    public string Id { get; set; }
    public string Status { get; set; }
}

public class RiskStatusDapperDto
{
    public string Key { get; set; }
    public string Text { get; set; }
}