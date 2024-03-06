using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.PMOL;

public class PmolQuality
{
    public string Id { get; set; }

    [ForeignKey("Pmol")] public string PmolId { get; set; }
    public virtual Pmol Pmol { get; set; }

    [ForeignKey("Quality")] public string QualityId { get; set; }
    public virtual Quality Quality { get; set; }
}