using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.PMOL;

public class PmolRisk
{
    public string Id { get; set; }

    [ForeignKey("Pmol")] public string PmolId { get; set; }
    public virtual Pmol Pmol { get; set; }

    [ForeignKey("Risk")] public string RiskId { get; set; }
    public virtual Risk Risk { get; set; }
}