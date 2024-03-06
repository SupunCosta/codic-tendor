using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data;

public class QrHistoryLog
{
    public string Id { get; set; }
    public string HistoryLog { get; set; }
    [ForeignKey("QRCode")] public string QRCodeId { get; set; }
    public virtual QRCode QRCode { get; set; }
    public string ChangedByUserId { get; set; }
    public string Action { get; set; }
    public DateTime ChangedTime { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RevisionNumber { get; set; }
}