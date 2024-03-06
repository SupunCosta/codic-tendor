using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.BOR;

public class BorRequiredLabour
{
    public string Id { get; set; }
    public DateTime Date { get; set; }
    public double Quantity { get; set; }
    [ForeignKey("BorLabour")] public string BorLabourId { get; set; }
    public virtual BorLabour BorLabour { get; set; }
    public string CpcId { get; set; }
}