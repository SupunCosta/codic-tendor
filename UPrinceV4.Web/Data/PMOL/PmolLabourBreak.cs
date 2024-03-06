using System;

namespace UPrinceV4.Web.Data.PMOL;

public class PmolLabourBreak
{
    public string Id { get; set; }
    public string LabourId { get; set; }
    public DateTime? StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }
    public bool IsBreak { get; set; }
}

public class LabourBreakCal
{
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public bool IsBreak { get; set; }
}

public class PmolLabourBreakRead
{
    public string Id { get; set; }
    public string LabourId { get; set; }
    public DateTime? StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }
    public bool IsBreak { get; set; }
}