namespace UPrinceV4.Web.Data.BOR;

public class BorType
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LanguageCode { get; set; }
    public string BorTypeId { get; set; }
    public int DisplayOrder { get; set; }
}

public class BorTypeDto
{
    public string key { get; set; }
    public string Text { get; set; }
}