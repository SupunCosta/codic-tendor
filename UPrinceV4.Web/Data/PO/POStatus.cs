namespace UPrinceV4.Web.Data.PO;

public class POStatus
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LanguageCode { get; set; }
    public string StatusId { get; set; }
    public int DisplayOrder { get; set; }
}

public class POStatusDto
{
    public string Key { get; set; }
    public string Text { get; set; }
}

public class GetPOStatusDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LanguageCode { get; set; }
    public string StatusId { get; set; }
    public int DisplayOrder { get; set; }
}

public class CreatePOStatus
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LanguageCode { get; set; }
    public string StatusId { get; set; }
    public int DisplayOrder { get; set; }
}