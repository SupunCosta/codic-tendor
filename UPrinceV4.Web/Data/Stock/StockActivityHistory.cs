using System;

namespace UPrinceV4.Web.Data.Stock;

public class StockActivityHistory
{
    public string Id { get; set; }
    public string WorkFlowId { get; set; }
    public DateTime DateTime { get; set; }
    public string ActivityTypeId { get; set; }
    public string Quantity { get; set; }
    public string MOUId { get; set; }
    public string Price { get; set; }
    public string WorkFlowTitle { get; set; }
    public string WareHouseWorker { get; set; }
    public string StockHeaderId { get; set; }
}