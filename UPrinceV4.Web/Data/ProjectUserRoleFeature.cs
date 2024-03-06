using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data;

public class ProjectUserRoleFeature
{
    public string Id { get; set; }
    [ForeignKey("ProjectUserRole")] public string ProjectUserRoleId { get; set; }
    public virtual ProjectUserRole ProjectUserRole { get; set; }
    [ForeignKey("ProjectFeature")] public string ProjectFeatureId { get; set; }
    public virtual ProjectFeature ProjectFeature { get; set; }
}