using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data;

public class ProjectStateLocalizedData
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string LanguageCode { get; set; }

    [ForeignKey("ProjectState")] public string ProjectStateId { get; set; }
    public int DisplayOrder { get; set; }
}