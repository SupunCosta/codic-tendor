using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data;

public class ProjectDefinitionHistoryLog
{
    public string Id { get; set; }
    public string HistoryLog { get; set; }
    public string ProjectDefinitionId { get; set; }
    public string ChangedByUserId { get; set; }
    public virtual ApplicationUser ChangedByUser { get; set; }
    public string Action { get; set; }
    public DateTime ChangedTime { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RevisionNumber { get; set; }
}

public class ProjectDefinitionHistoryLogDto
{
    public string CreatedByUser { get; set; }
    public DateTime? CreatedDateTime { get; set; }
    public string UpdatedByUser { get; set; }
    public DateTime? UpdatedDateTime { get; set; }
    public int RevisionNumber { get; set; }
}

public class ProjectDefinitionHistoryLogDapperDto
{
    public string Oid { get; set; }
    public string User { get; set; }
    public DateTime? DateTime { get; set; }
    public int RevisionNumber { get; set; }
}