using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace UPrinceV4.Web.Data.PBS_;

[Table("PbsInstruction")]
public class PbsInstruction : BaseEntity
{
    public string InstructionsDetails { get; set; }

    // 100 = technical, 200 = environmental, 300 = safety, 400 = health
    public string InstructionType { get; set; }

    [ForeignKey("PbsProduct")]
    [JsonIgnore]
    public string PbsProductId { get; set; }

    public virtual PbsProduct PbsProduct { get; set; }
    [JsonIgnore] public string PbsInstructionFamilyId { get; set; }

    [NotMapped] public virtual PbsInstructionFamily PbsInstructionFamily { get; set; }

    [NotMapped] public IList<PbsInstructionLink> PbsInstructionLink { get; set; }

    public string InstructionsId { get; set; }
}

public class PbsInstructionDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string InstructionType { get; set; }
    public string InstructionsDetails { get; set; }
    public string PbsProductId { get; set; }
    public string PbsInstructionFamilyId { get; set; }
    public IList<PbsInstructionLinkDto> PbsInstructionLink { get; set; }
    public string InstructionsId { get; set; }
}

public class PbsInstructionLoadDto : BaseEntity
{
    public string InstructionsDetails { get; set; }

    // 100 = technical, 200 = environmental, 300 = safety, 400 = health
    public string InstructionType { get; set; }
    public string PbsProductId { get; set; }
    public virtual PbsProduct PbsProduct { get; set; }
    public string PbsInstructionFamilyId { get; set; }
    public string PbsInstructionFamilyName { get; set; }
    public virtual PbsInstructionFamilyLoadDto PbsInstructionFamily { get; set; }
    public List<PbsInstructionLinkDto> PbsInstructionLink { get; set; }
    public string InstructionsId { get; set; }

    public string PbsInstructionId { get; set; }
}

public class PbsInstructionLoadAllDto : BaseEntity
{
    public IList<PbsInstructionLoadDto> Technical { get; set; }

    public IList<PbsInstructionLoadDto> Environmental { get; set; }

    public IList<PbsInstructionLoadDto> Safety { get; set; }

    public IList<PbsInstructionLoadDto> Health { get; set; }
}

public class PbsInstructionLoadAllPmolDto
{
    public IList<PbsInstructionLoadDto> Technical { get; set; }

    public IList<PbsInstructionLoadDto> Environmental { get; set; }

    public IList<PbsInstructionLoadDto> Safety { get; set; }

    public IList<PbsInstructionLoadDto> Health { get; set; }
}

public class PbsInstructionFilter
{
    public string Title { get; set; }
    public string InstructionType { get; set; }
    public string PbsInstructionFamilyId { get; set; }
    public string PbsProductId { get; set; }

    public Sorter Sorter { get; set; }
}

public class Sorter
{
    public string Attribute { get; set; }
    public string Order { get; set; }
}