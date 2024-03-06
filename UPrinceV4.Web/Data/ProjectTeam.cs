using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UPrinceV4.Web.Data.CAB;

namespace UPrinceV4.Web.Data;

public class ProjectTeam
{
    public string Id { get; set; }

    [ForeignKey("CabCompany")] public string ContractingUnitId { get; set; }
    public virtual CabCompany ContractingUnit { get; set; }

    [ForeignKey("ProjectDefinition")] public string ProjectId { get; set; }
    public virtual ProjectDefinition Project { get; set; }
}

public class ProjectTeamUpdateDto
{
    [Required] public string Id { get; set; }
    public string ContractingUnitId { get; set; }
    public string ContractingUnitName { get; set; }
    public IEnumerable<ProjectTeamRoleReadDto> TeamRoleList { get; set; }
}

public class ProjectTeamCreateDto
{
    public string Id { get; set; }
    public string ContractingUnitId { get; set; }
    public string ProjectId { get; set; }
    public List<ProjectTeamRoleCreateDto> TeamRoleList { get; set; }
}