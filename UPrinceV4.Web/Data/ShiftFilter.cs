using System;

namespace UPrinceV4.Web.Data;

public class ShiftFilter
{
    public int? StartDateTime { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string UserName { get; set; }
    public int? StatusId { get; set; }
    public string OffSet { get; set; }
    public DateTime LocalDate { get; set; }
    public Sorter Sorter { get; set; }
}