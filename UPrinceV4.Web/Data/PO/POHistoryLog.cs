using System;

namespace UPrinceV4.Web.Data.PO;

public class POHistoryLog
{
    public string Id { get; set; }
    public string HistoryLog { get; set; }
    public string ChangedByUserId { get; set; }
    public string Action { get; set; }
    public DateTime ChangedTime { get; set; }
    public string PoId { get; set; }
}