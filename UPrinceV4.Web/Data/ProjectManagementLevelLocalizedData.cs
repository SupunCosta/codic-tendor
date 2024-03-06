using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data;

public class ProjectManagementLevelLocalizedData
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string LanguageCode { get; set; }

    [ForeignKey("ProjectManagementLevel")] public string ProjectManagementLevelId { get; set; }
}