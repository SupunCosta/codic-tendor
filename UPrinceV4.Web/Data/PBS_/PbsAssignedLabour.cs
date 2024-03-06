using System;
using System.Collections.Generic;

namespace UPrinceV4.Web.Data.PBS_;

public class PbsAssignedLabour
{
    public string Id { get; set; }
    public string PbsLabourId { get; set; }
    public string PbsProduct { get; set; }
    public string CabPersonId { get; set; }
    public bool IsDeleted { get; set; } = false;
    public string CpcId { get; set; } 
    public string Project { get; set; } 
    public DateTime StartDate { get; set; } 
    public DateTime EndDate { get; set; }
    public double AssignedHoursPerDay { get; set; }
    public string Week { get; set; }
    public string ProjectManager { get; set; }
    public string DayOfWeek { get; set; }
    public DateTime Date { get; set; }
}

public class PbsAssignedLabourDto
{
    public string Id { get; set; }
    public string PbsLabourId { get; set; }
    public List<string> CabPersonId { get; set; }
    public string CpcId { get; set; }
    public string Cu { get; set; }
    public string Project { get; set; }
    public string PbsProduct { get; set; }
    public string TeamId { get; set; }
}

public class PbsAssignedLabourDeleteDto
{
    public string CabPersonId { get; set; }
    public string CpcId { get; set; }
    public string Project { get; set; }
}