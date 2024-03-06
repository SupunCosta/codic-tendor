using System;

namespace UPrinceV4.Web.Data.VisualPlan;

public class AbsenceHeader
{
    public string Id { get; set; }
    public string Person { get; set; }
    public string LeaveType { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public bool AllDay { get; set; }
}

public class AbsenceHeaderCreateDto
{
    public string Id { get; set; }
    public string Person { get; set; }
    public string LeaveType { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public bool AllDay { get; set; }
}

public class AbsenceHeaderGetDto
{
    public string Id { get; set; }
    public string Person { get; set; }
    public string PersonName { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public bool AllDay { get; set; }
    public AbsenceLeaveTypeDto LeaveType { get; set; }
}