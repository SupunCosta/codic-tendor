using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data;

public class ProjectTimeHistory
{
    public string Id { get; set; }
    public DateTime? ExpectedEndDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string CalendarTemplateId { get; set; }
    [NotMapped] public virtual CalendarTemplate CalendarTemplate { get; set; }
    public string ProjectId { get; set; }
    [NotMapped] public virtual ProjectDefinition ProjectDefinition { get; set; }
    public string ChangeByUserId { get; set; }
    [NotMapped] public virtual ApplicationUser ChangeByUser { get; set; }
    public string Action { get; set; }
    public int RevisionNumber { get; set; }
    [Key] public DateTime SysStartTime { get; set; }
    public DateTime SysEndTime { get; set; }
}