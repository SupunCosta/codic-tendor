using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.PMOL;

public class PmolInstructionLink
{
    public string Id { get; set; }
    public string Link { get; set; }
    [ForeignKey("PmolDocument")] public string PmolDocumentId { get; set; }
    public virtual PmolInstruction PmolDocument { get; set; }
}