namespace UPrinceV4.Web.Data.Stock;

public class StockStatus
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string StatusId { get; set; }
    public string LanguageCode { get; set; }
    public int DisplayOrder { get; set; }
}

public class StockStatusDto
{
    public string Key { get; set; }
    public string Text { get; set; }
}