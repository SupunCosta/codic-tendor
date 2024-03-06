namespace UPrinceV4.Web.Data.PO;

public class POType
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LanguageCode { get; set; }
    public string TypeId { get; set; }
    public int DisplayOrder { get; set; }
}

public class POTypeDto
{
    public string key { get; set; }
    public string Text { get; set; }
}

public class GetPOTypeDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LanguageCode { get; set; }
    public string TypeId { get; set; }
    public int DisplayOrder { get; set; }
}

public class CreatePOType
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LanguageCode { get; set; }
    public string TypeId { get; set; }
    public int DisplayOrder { get; set; }
}