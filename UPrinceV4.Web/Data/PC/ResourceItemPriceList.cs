namespace UPrinceV4.Web.Data.PC;

public class ResourceItemPriceList
{
    public string Id { get; set; }
    public string ResourceTypeId { get; set; }
    public string CpcId { get; set; }
    public double? Coefficient { get; set; }
    public double? FixedPrice { get; set; }
    public bool IsDeleted { get; set; }
}

public class ResourceItemPriceListReadDto
{
    public string Id { get; set; }
    public string ResourceTypeId { get; set; }
    public string CpcId { get; set; }
    public string ResourceNumber { get; set; }
    public double? Coefficient { get; set; }
    public double? FixedPrice { get; set; }
    public string Title { get; set; }
}