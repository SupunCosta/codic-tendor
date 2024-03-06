using System;

namespace UPrinceV4.Web.Data.PS;

public class PsFilter
{
    public string Title { get; set; }
    public string Status { get; set; }
    public string Project { get; set; }
    public double? TotalAmount { get; set; }
    public DateTime? Date { get; set; }
    public Sorter Sorter { get; set; }
    public string ProjectSequenceCode { get; set; }
}