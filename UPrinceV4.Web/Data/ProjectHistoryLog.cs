using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data;

public class ProjectHistoryLog
{
    public string Id { get; set; }

    [ForeignKey("ProjectState")] public string ProjectStateId { get; set; }
    public virtual ProjectState ProjectState { get; set; }
    public DateTime CreatedDateTime { get; set; }

    [ForeignKey("ApplicationUser")] public string CreatedByUserId { get; set; }
    public virtual ApplicationUser CreatedByUser { get; set; }
    public DateTime ModifiedDateTime { get; set; }

    [ForeignKey("ApplicationUser")] public string ModifiedByUserId { get; set; }
    public virtual ApplicationUser ModifiedByUser { get; set; }
    public int RevisionNumber { get; set; }

    [ForeignKey("ProjectDefinition")] public string ProjectId { get; set; }
    public virtual ProjectDefinition ProjectDefinition { get; set; }
}

public class ProjectHistoryLogCreateDto
{
    public string ProjectStateId { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public string CreatedByUserId { get; set; }
    public DateTime ModifiedDateTime { get; set; }
    public string ModifiedByUserId { get; set; }
    public int RevisionNumber { get; set; }
    public string ProjectId { get; set; }
}

public class ProjectHistoryLogUpdateDto
{
    [Required] public string Id { get; set; }
    public string ProjectStateId { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public string CreatedByUserId { get; set; }
    public DateTime ModifiedDateTime { get; set; }
    public string ModifiedByUserId { get; set; }
    public int RevisionNumber { get; set; }
    public string ProjectId { get; set; }
}