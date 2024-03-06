using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.PBS_;

[Table("Instructions")]

public class Instructions : BaseEntity
{
    public string InstructionsDetails { get; set; }

    // 100 = technical, 200 = environmental, 300 = safety, 400 = health
    public string InstructionType { get; set; }
    public string PbsInstructionFamilyId { get; set; }

    [NotMapped] public IList<PbsInstructionLink> PbsInstructionLink { get; set; }

    [NotMapped] public PbsInstructionFamily PbsInstructionFamily { get; set; }
}

public class CreateInstructionDto
{
    public string Id { get; set; }
    public string PbsProductId { get; set; }
    public string InstructionsId { get; set; }
}