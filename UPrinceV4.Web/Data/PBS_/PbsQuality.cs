using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.PBS_;

public class PbsQuality
{
    public string Id { get; set; }

    [ForeignKey("PbsProduct")] public string PbsProductId { get; set; }
    public virtual PbsProduct PbsProduct { get; set; }

    [ForeignKey("Quality")] public string QualityId { get; set; }
    public virtual Quality Quality { get; set; }
}

public class PbsQualityCreateDto
{
    [Required] public string PbsProductId { get; set; }
    [Required] public string QualityId { get; set; }
}