namespace UPrinceV4.Web.Data.PO;

public class PORequestType
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LanguageCode { get; set; }
    public string RequestTypeId { get; set; }
    public int DisplayOrder { get; set; }
}

public class PORequestTypeDto
{
    public string key { get; set; }
    public string Text { get; set; }
}
