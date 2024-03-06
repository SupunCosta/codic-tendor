using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.BOR;

public class BorRequiredTools
{
    public string Id { get; set; }
    public DateTime Date { get; set; }
    public double Quantity { get; set; }
    [ForeignKey("BorMaterial")] public string BorToolsId { get; set; }
    public virtual BorTools BorTools { get; set; }
    public string CpcId { get; set; }
}