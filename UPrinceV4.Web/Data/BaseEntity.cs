using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data;

public class BaseEntity : MetaData
{
    public string Id { get; set; }
    public string SequenceCode { get; set; }
    [NotMapped] public string Title => SequenceCode + " - " + Name;
    [NotMapped] public string HeaderTitle => SequenceCode + " - " + Name;
    public string Name { get; set; }
    [NotMapped] public string InstructionTypeName { get; set; }
    [NotMapped] public string InstructionFamilyName { get; set; }
}