namespace UPrinceV4.Web.Data.PL;

public class PLListsItems
{
    public string Id { get; set; }
    public string ListId { get; set; }
    public string ItemId { get; set; }

    public PLItem PLItem { get; set; }
    public PLPriceList PLPriceList { get; set; }
}