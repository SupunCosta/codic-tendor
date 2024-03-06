namespace UPrinceV4.Web.Data.WF;

public class WFType
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string TypeId { get; set; }
    public string LanguageCode { get; set; }
    public int DisplayOrder { get; set; }
}

public class WFTypeDto
{
    public string Key { get; set; }
    public string Text { get; set; }
}