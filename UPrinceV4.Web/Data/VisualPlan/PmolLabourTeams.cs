using System;

namespace UPrinceV4.Web.Data.VisualPlan;

public class PmolLabourTeams
{
    public string Id { get; set; }
    public string PmolId { get; set; }
    public string PersonId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class GetPmolLabourTeams
{
    public string Id { get; set; }
    public string PmolId { get; set; }
    public string PersonId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}