using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data;

public class ProjectDefinitionHistory
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ProjectTypeId { get; set; }
    public virtual ProjectType ProjectType { get; set; }
    public string ProjectManagementLevelId { get; set; }
    public virtual ProjectManagementLevel ProjectManagementLevel { get; set; }
    public string ProjectToleranceStateId { get; set; }
    public virtual ProjectToleranceState ProjectToleranceState { get; set; }
    public string ProjectTemplateId { get; set; }
    public virtual ProjectTemplate ProjectTemplate { get; set; }
    [NotMapped] public virtual List<ProjectKPIHistory> ProjectKPIs { get; set; }
    public string SequenceCode { get; set; }
    [NotMapped] public virtual List<ProjectFinanceHistory> ProjectFinances { get; set; }
    [NotMapped] public virtual List<ProjectTimeHistory> ProjectTimes { get; set; }
    public string ChangeByUserId { get; set; }
    public virtual ApplicationUser ChangeByUser { get; set; }
    public string Action { get; set; }
    public int RevisionNumber { get; set; }
    [Key] public DateTime SysStartTime { get; set; }
    public DateTime SysEndTime { get; set; }
}