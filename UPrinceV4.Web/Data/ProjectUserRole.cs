using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data;

public class ProjectUserRole
{
    public string Id { get; set; }
    [ForeignKey("ProjectDefinition")] public string ProjectDefinitionId { get; set; }

    public virtual ProjectDefinition ProjectDefinition { get; set; }

    //[ForeignKey("UserRole")]
    public string UsrRoleId { get; set; }

    //public virtual UserRole UserRole { get; set; }
    public string Status { get; set; }
}