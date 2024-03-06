using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.PMOL;

public class PmolTeamRole
{
    public string Id { get; set; }
    public string CabPersonId { get; set; }
    public string RoleId { get; set; }
    [ForeignKey("PmolPlannedWorkLabour")] public string PmolLabourId { get; set; }
    public virtual PmolPlannedWorkLabour PmolLabour { get; set; }
    public double? RequiredQuantity { get; set; }
    public double? ConsumedQuantity { get; set; }
    public string Type { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsJobDone { get; set; }
    public string Message { get; set; }
}

public class PmolTeamRoleCreateDto
{
    public string Id { get; set; }
    public string CabPersonId { get; set; }
    public string RoleId { get; set; }
    public string PmolLabourId { get; set; }
    public double? RequiredQuantity { get; set; }
    public double? ConsumedQuantity { get; set; }
    public string Type { get; set; }
}

public class PmolTeamRoleReadDto
{
    public string Id { get; set; }
    public string CabPersonId { get; set; }
    public string RoleId { get; set; }
    public double? RequiredQuantity { get; set; }
    public double? ConsumedQuantity { get; set; }
    public string RoleName { get; set; }
    public string CabPerson { get; set; }
    public string PmolLabourId { get; set; }
    public DateTime? StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }
}

public class PmolTeamRoleDelete
{
    public string Id { get; set; }
    public string CabPersonId { get; set; }
    public string PmolId { get; set; }
    public DateTime? ExecutionDate { get; set; }
    public string RoleId { get; set; }

}

public class RemovePersonFromPmol
{
    public string CabPersonId { get; set; }
    public string PmolId { get; set; }
    public string CuConnectionString { get; set; }
    public string ProjectConnectionString { get; set; }
    public DateTime ExecutionDate { get; set; }

}