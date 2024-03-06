using System;

namespace UPrinceV4.Web.Data.VisualPlan;

public class PmolToolsPool
{
    public string Id { get; set; }
    public string PmolId { get; set; }
    public string CPCId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class GetPmolToolsPool
{
    public string Id { get; set; }
    public string PmolId { get; set; }
    public string CPCId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}