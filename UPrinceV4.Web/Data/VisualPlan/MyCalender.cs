using System;

namespace UPrinceV4.Web.Data.VisualPlan;

public class MyCalender
{
    
}

public class MyCalenderGetTeamDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string BuId { get; set; }
    public string Type { get; set; }
    public string Id { get; set; }
}

public class MyCalenderPmolData
{
    public string Id { get; set; }
    public string OrganizationTeamId { get; set; }
    public string PmolId { get; set; }
    public DateTime? ExecutionDate { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public string Project { get; set; }
    public string ContractingUnit { get; set; }
    public string PersonId { get; set; }
}