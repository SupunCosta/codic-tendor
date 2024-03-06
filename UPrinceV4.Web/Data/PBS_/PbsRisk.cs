using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.PBS_;

public class PbsRisk
{
    public string Id { get; set; }

    [ForeignKey("PbsProduct")] public string PbsProductId { get; set; }
    public virtual PbsProduct PbsProduct { get; set; }

    [ForeignKey("Risk")] public string RiskId { get; set; }
    public virtual Risk Risk { get; set; }
}

public class PbsRiskCreateDto
{
    [Required] public string PbsProductId { get; set; }
    [Required] public string RiskId { get; set; }
}