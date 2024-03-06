using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data;

public class HistoryMetaData
{
    public string Id { get; set; }
    public string HistoryLog { get; set; }

    public string ChangedByUserId { get; set; }

    //public virtual ApplicationUser ChangedByUser { get; set; }
    public string Action { get; set; }
    public DateTime ChangedTime { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RevisionNumber { get; set; }
}