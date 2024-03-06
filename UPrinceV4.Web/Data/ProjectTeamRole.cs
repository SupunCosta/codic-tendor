using System.ComponentModel.DataAnnotations.Schema;
using UPrinceV4.Web.Data.CAB;

namespace UPrinceV4.Web.Data;

public class ProjectTeamRole
{
    public string Id { get; set; }

    [ForeignKey("ProjectTeam")] public string ProjectTeamId { get; set; }
    public virtual ProjectTeam ProjectTeam { get; set; }

    [ForeignKey("CabCompany")] public string CabPersonId { get; set; }
    public virtual CabPerson CabPerson { get; set; }

    public string RoleId { get; set; }

    //public virtual Roles Role { get; set; }
    public string status { get; set; }
    public bool IsAccessGranted { get; set; }

    public string Message { get; set; }
}

public class ProjectTeamRoleCreateDto
{
    public string Id { get; set; }
    public string CabPersonId { get; set; }
    public string RoleId { get; set; }
    public string status { get; set; }
    public string Message { get; set; }
    public string Email { get; set; }
    public bool IsAccessGranted { get; set; }
}

public class ProjectTeamRoleReadDto
{
    public string Id { get; set; }
    public string CabPersonName { get; set; }
    public string RoleName { get; set; }
    public string status { get; set; }
    public string CabPersonId { get; set; }
    public string RoleId { get; set; }
    public bool IsAccessGranted { get; set; }
    public string Email { get; set; }
    public string CompanyName { get; set; }
    public string Message { get; set; }
}