using System;
using System.Collections.Generic;
using UPrinceV4.Web.Data.CAB;

namespace UPrinceV4.Web.Data;

public class ShiftDetails
{
    public DateTime Date { get; set; }
    public DateTime? EndDateTime { get; set; }
    public ApplicationUser Person { get; set; }
    public CabPersonCompany CabPerson { get; set; }
    public TimeSpan TotalTime { get; set; }
    public string Status { get; set; }
    public List<TimeClockDetails> TimeRegistrations { get; set; }
    public WorkflowState WorkflowState { get; set; }
    public bool IsShiftEnded { get; set; }
}

public class TimeClockDetails
{
    public DateTime StartDateTime { get; set; }
    public TimeSpan ElapedTime { get; set; }
    public TimeClockActivityType ActivityType { get; set; }

    public string TimeClockId { get; set; }
}