using System;

namespace UPrinceV4.Web.Data.VisualPlan;

public class VpWH
{
    public string Id { get; set; }
    public string PoId { get; set; }
    public string ProjectSequenceCode { get; set; }
    public string CPCId { get; set; }
    public string ParentId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class GetVpWHDto
{
    public string Id { get; set; }
    public string PoId { get; set; }
    public string ProjectSequenceCode { get; set; }
    public string CPCId { get; set; }
    public string ParentId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Title { get; set; }
    public string Type { get; set; }
}