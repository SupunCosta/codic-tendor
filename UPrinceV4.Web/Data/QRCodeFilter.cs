using System;

namespace UPrinceV4.Web.Data;

public class QRCodeFilter
{
    public int? Type { get; set; }
    public string ProjectId { get; set; }
    public string VehicleNo { get; set; }
    public string Location { get; set; }
    public string OffSet { get; set; }
    public DateTime? Date { get; set; }
    public Sorter Sorter { get; set; }
}