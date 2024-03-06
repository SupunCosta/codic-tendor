using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data;

public class ProjectTime
{
    public string Id { get; set; }
    public DateTime? ExpectedEndDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    [ForeignKey("CalendarTemplate")] public string CalendarTemplateId { get; set; }
    public virtual CalendarTemplate CalendarTemplate { get; set; }

    [ForeignKey("ProjectDefinition")] public string ProjectId { get; set; }

    public virtual ProjectDefinition ProjectDefinition { get; set; }
    public DateTime? TenderStartDate { get; set; }
    public DateTime? TenderEndDate { get; set; }


    //public string ChangeByUserId { get; set; }
    //public virtual Users ChangeByUser { get; set; }
    //public string Action { get; set; }
    //public int RevisionNumber { get; set; }
}

public class ProjectTimeCreateDto
{
    public DateTime? ExpectedEndDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string CalendarTemplateId { get; set; }
    public string ProjectId { get; set; }
    public DateTime? TenderStartDate { get; set; }
    public DateTime? TenderEndDate { get; set; }
}

public class ProjectTimeUpdateDto
{
    public string Id { get; set; }
    public DateTime? ExpectedEndDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string CalendarTemplateId { get; set; }
    [Required] public string ProjectId { get; set; }
    public DateTime? TenderStartDate { get; set; }
    public DateTime? TenderEndDate { get; set; }
}