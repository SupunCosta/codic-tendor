namespace UPrinceV4.Web.Data.CIAW;

public class CiawStatus
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LanguageCode { get; set; }
    public string StatusId { get; set; }
    public string DisplayOrder { get; set; }
}

public class CiawStatusDto
{
    public string Key { get; set; }
    public string Text { get; set; }
}