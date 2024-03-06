using System;

namespace UPrinceV4.Web.Data.PMOL;

public class PmolFilter
{
    public string Title { get; set; }
    public string TypeId { get; set; }
    public string StatusId { get; set; }
    public string Foreman { get; set; }
    public DateTime? ExecutionDate { get; set; }
    public string Offset { get; set; }
    public DateTime LocalDate { get; set; }
    public int? Date { get; set; }
    public int? Status { get; set; }
    public Sorter Sorter { get; set; }
}

public class GmtMonthDto
{
    public DateTime FirstDay { get; set; }
    public DateTime LastDay { get; set; }
}