using System;

namespace UPrinceV4.Web.Data.VisualPlan;

public class OrganizationTeamPmol
{
    public string Id { get; set; }
    public string OrganizationTeamId { get; set; }
    public string PmolId { get; set; }
    public DateTime? ExecutionDate { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public string Project { get; set; }
    public string ContractingUnit { get; set; }
}
