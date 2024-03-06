namespace UPrinceV4.Web.Data.Stock;

public class StockFilter
{
    public string Status { get; set; }
    public string Name { get; set; }
    public string ResourceType { get; set; }
    public string Type { get; set; }
    public string Title { get; set; }
    public string WharehouseTaxonomy { get; set; }
    public string WareHouse { get; set; }
    public string Count { get; set; }
    public Sorter Sorter { get; set; }
}