using System;

namespace UPrinceV4.Web.Data.WF;

public class WFFilter
{
    public string Title { get; set; }
    public DateTime? TargetDateTime { get; set; }
    public DateTime? ExecutedDateTime { get; set; }
    public DateTime? DateTime { get; set; }
    public string Requester { get; set; }
    public string Executer { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }
    public string Offset { get; set; }
    public int? Date { get; set; }
    public DateTime LocalDate { get; set; }
    public Sorter Sorter { get; set; }
}