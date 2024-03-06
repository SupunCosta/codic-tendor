using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.BOR;

public class BorRequiredConsumable
{
    public string Id { get; set; }
    public DateTime Date { get; set; }
    public double Quantity { get; set; }
    [ForeignKey("BorMaterial")] public string BorConsumableId { get; set; }
    public virtual BorConsumable BorConsumable { get; set; }
    public string CpcId { get; set; }
}

public class BorRequiredResource
{
    public string Id { get; set; }
    public DateTime Date { get; set; }
    public double Quantity { get; set; }
    [ForeignKey("BorMaterial")] public string BorConsumableId { get; set; }
    public virtual BorConsumable BorConsumable { get; set; }
    public string CpcId { get; set; }
}