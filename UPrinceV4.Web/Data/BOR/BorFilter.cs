namespace UPrinceV4.Web.Data.BOR;

public class BorFilter
{
    public string BorTitle { get; set; }
    public string Product { get; set; }
    public string LocationParent { get; set; }
    public string UtilityParent { get; set; }
    public Sorter Sorter { get; set; }
}

public class BorResourceFilter
{
    public string ResourceTypeId { get; set; }
    public string ResourceTitle { get; set; }
    public double? Required { get; set; }
    public double? Purchased { get; set; }
    public double? DeliveryRequested { get; set; }
    public double? Warf { get; set; }
    public double? Invoiced { get; set; }
    public Sorter Sorter { get; set; }
}