using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.CAB;

public class CabHistoryLog
{
    public string Id { get; set; }
    public string HistoryLog { get; set; }
    [ForeignKey("ApplicationUser")] public string ChangedByUserId { get; set; }
    public virtual ApplicationUser ChangedByUser { get; set; }
    public string Action { get; set; }
    public DateTime ChangedTime { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RevisionNumber { get; set; }

    [ForeignKey("CabPerson")] public string PersonId { get; set; }
    public virtual CabPerson Person { get; set; }
    [ForeignKey("CabCompany")] public string CompanyId { get; set; }
    public virtual CabCompany Company { get; set; }
}

public class CabHistoryLogDto
{
    public string CreatedByUser { get; set; }
    public DateTime? CreatedDateTime { get; set; }
    public string UpdatedByUser { get; set; }
    public DateTime? UpdatedDateTime { get; set; }
    public int RevisionNumber { get; set; }
}