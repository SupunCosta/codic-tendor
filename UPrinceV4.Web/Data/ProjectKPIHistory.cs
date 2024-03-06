using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data;

public class ProjectKPIHistory
{
    public string Id { get; set; }
    public string CustomLabelOne { get; set; }
    public int CustomPropOne { get; set; }
    public string CustomLabelTwo { get; set; }
    public int CustomPropTwo { get; set; }
    public string CustomLabelThree { get; set; }
    public int CustomPropThree { get; set; }
    public string ProjectId { get; set; }
    [NotMapped] public virtual ProjectDefinition ProjectDefinition { get; set; }
    public string ChangeByUserId { get; set; }
    [NotMapped] public virtual ApplicationUser ChangeByUser { get; set; }
    public string Action { get; set; }
    public int RevisionNumber { get; set; }
    [Key] public DateTime SysStartTime { get; set; }
    public DateTime SysEndTime { get; set; }
}