using System;

namespace UPrinceV4.Web.Data.PMOL;

public class PmolLabourTime
{
    public string Id { get; set; }
    public string LabourId { get; set; }
    public DateTime? StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }
    public bool IsBreak { get; set; } = false;
    public string Type { get; set; }
}

public class LabourTime
{
    public DateTime StartDateTimeRoundNearest { get; set; }
    public DateTime EndDateTimeRoundNearest { get; set; }
    public int dif { get; set; }
}

public class PmolLabourTimeRead
{
    public string Id { get; set; }
    public string LabourId { get; set; }
    public DateTime? StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }
    public bool IsBreak { get; set; } = false;
}

public class PmolLabourTimeIsLabour
{
    public string PmolId { get; set; }
    public string CabPersonId { get; set; }
}