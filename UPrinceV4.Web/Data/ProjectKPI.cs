using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data;

public class ProjectKPI
{
    public string Id { get; set; }
    public string CustomLabelOne { get; set; }
    public double? CustomPropOne { get; set; }
    public string CustomLabelTwo { get; set; }
    public double? CustomPropTwo { get; set; }
    public string CustomLabelThree { get; set; }
    public double? CustomPropThree { get; set; }

    [ForeignKey("ProjectDefinition")] public string ProjectId { get; set; }

    public virtual ProjectDefinition ProjectDefinition { get; set; }
    //public string ChangeByUserId { get; set; }
    //public virtual Users ChangeByUser { get; set; }
    //public string Action { get; set; }
    //public int RevisionNumber { get; set; }
}

public class ProjectKPIUpdateDto
{
    public string Id { get; set; }
    public string CustomLabelOne { get; set; }
    public double? CustomPropOne { get; set; }
    public string CustomLabelTwo { get; set; }
    public double? CustomPropTwo { get; set; }
    public string CustomLabelThree { get; set; }
    public double? CustomPropThree { get; set; }
    public string ProjectId { get; set; }
}

public class ProjectKPICreateDto
{
    public string CustomLabelOne { get; set; }
    public double? CustomPropOne { get; set; }
    public string CustomLabelTwo { get; set; }
    public double? CustomPropTwo { get; set; }
    public string CustomLabelThree { get; set; }
    public double? CustomPropThree { get; set; }
}