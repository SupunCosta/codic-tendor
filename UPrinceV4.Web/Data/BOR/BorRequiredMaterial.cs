using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.BOR;

public class BorRequiredMaterial
{
    public string Id { get; set; }
    public DateTime Date { get; set; }
    public double Quantity { get; set; }
    [ForeignKey("BorMaterial")] public string BorMaterialId { get; set; }
    public virtual BorMaterial BorMaterial { get; set; }
    public string CpcId { get; set; }
}