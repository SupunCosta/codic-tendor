using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.PMOL;

public class PmolBreak
{
    public string Id { get; set; }

    [ForeignKey("Pmol")] public string PmolId { get; set; }
    public virtual Pmol Pmol { get; set; }
    public DateTime? StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }
    public bool IsBreak { get; set; }
}

public class PmolBreakCal
{
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public bool IsBreak { get; set; }
}

public class PmolBreakDto
{
    public bool IsBreak { get; set; }
}