using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.INV;

public class InvoiceHistoryLog
{
    public string Id { get; set; }
    public string HistoryLog { get; set; }
    [ForeignKey("Invoice")] public string InvoiceId { get; set; }
    public virtual Invoice Invoice { get; set; }
    public string ChangedByUserId { get; set; }
    public string Action { get; set; }
    public DateTime ChangedTime { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RevisionNumber { get; set; }
}