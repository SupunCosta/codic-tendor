namespace UPrinceV4.Web.Data.WF;

public class WFTask
{
    public string Id { get; set; }
    public string WorkFlowId { get; set; }
    public string Source { get; set; }
    public string CPCItemId { get; set; }
    public string Quantity { get; set; }
    public string MOUId { get; set; }
    public string PickedQuantity { get; set; }
    public string Destination { get; set; }

    public string Comment { get; set; }
    public string StockAvailability { get; set; }
    public string UnitPrice { get; set; }
}