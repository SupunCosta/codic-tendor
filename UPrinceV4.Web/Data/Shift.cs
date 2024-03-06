using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Contrib.Extensions;
using UPrinceV4.Web.Data.CAB;

namespace UPrinceV4.Web.Data;

public class Shift
{
    public string Id { get; set; }

    // [ForeignKey("ApplicationUser")]
    public string UserId { get; set; }
    [NotMapped] public CabPersonCompany User { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }
    public string Status { get; set; }
    public int WorkflowStateId { get; set; }
    public virtual WorkflowState WorkflowState { get; set; }
    public IList<TimeClock> TimeClocks { get; set; }
}

public class ShiftDto
{
    [Key] public string Id { get; set; }

    public string UserId { get; set; }

    public DateTime StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }
    public string Status { get; set; }
    public int WorkflowStateId { get; set; }
}

public class ShiftListDto
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string User { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }
    public string Status { get; set; }
}

public class ShiftUser
{
    public string Oid { get; set; }
    public string Name { get; set; }
}