namespace UPrinceV4.Web.Data.Stock;

public class StockActiveType
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string TypeId { get; set; }
    public string LanguageCode { get; set; }
    public int DisplayOrder { get; set; }
}

public class StockActiveTypeDto
{
    public string Key { get; set; }
    public string Text { get; set; }
}