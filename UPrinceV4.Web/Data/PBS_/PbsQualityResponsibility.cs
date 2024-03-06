using System.ComponentModel.DataAnnotations.Schema;
using UPrinceV4.Web.Data.CAB;

namespace UPrinceV4.Web.Data.PBS_;

public class PbsQualityResponsibility
{
    public string Id { get; set; }

    // [ForeignKey("CabPerson")]
    public string QualityProducerId { get; set; }

    [NotMapped] public CabPerson QualityProducer { get; set; }

    //[ForeignKey("CabPerson")]
    public string QualityReviewerId { get; set; }

    [NotMapped] public CabPerson QualityReviewer { get; set; }

    // [ForeignKey("CabPerson")]
    public string QualityApproverId { get; set; }
    [NotMapped] public CabPerson QualityApprover { get; set; }
}

public class PbsQualityResponsibilityCreateDto
{
    public string Id { get; set; }
    public string QualityProducerId { get; set; }
    public string QualityReviewerId { get; set; }
    public string QualityApproverId { get; set; }
}