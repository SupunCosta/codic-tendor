

using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.PBS_;

[Table("PbsInstructionLink")]
public class PbsInstructionLink
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Type { get; set; }
    public string Link { get; set; }
    public string PbsInstructionId { get; set; }

    [NotMapped] public string Value { get; set; }
}

public class PbsInstructionLinkDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Type { get; set; }
    public string Value { get; set; }
    public string PbsInstructionId { get; set; }
    public string Link { get; set; }
}