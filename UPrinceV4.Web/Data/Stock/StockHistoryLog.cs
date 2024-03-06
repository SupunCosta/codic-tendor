using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.Stock;

public class StockHistoryLog
{
    public string Id { get; set; }
    public StockHeader StockHeader { get; set; }
    [ForeignKey("StockHeader")] public string StockId { get; set; }
    public string CreatedBy { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}