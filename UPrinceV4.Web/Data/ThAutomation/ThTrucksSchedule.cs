using System;

namespace UPrinceV4.Web.Data.ThAutomation;

public class ThTrucksSchedule
{
    public string Id { get; set; }
    public string ProductTruckId { get; set; }
    public string Title { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string Type { get; set; }
    public int? TurnNumber { get; set; }
    public int? LoadingNumber { get; set; }
    public int? TruckOrder { get; set; }
}

public class GetThTrucksSchedule
{
    public string Id { get; set; }
    public string ProductTruckId { get; set; }
    public string Title { get; set; }
    public string STime { get; set; }
    public string ETime { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string Type { get; set; }
    public string CpcId { get; set; }
    public string Truck { get; set; }
    public double? Size { get; set; }
    public int? TurnNumber { get; set; }
    public int? LoadingNumber { get; set; }
    public int? TruckOrder { get; set; }
    public string Capacity { get; set; }
    public string ProjectSequenceCode { get; set; }
    public bool IsTruck { get; set; }
    public bool IsError { get; set; }
    public string ErrorMessage { get; set; }


}

public class RemoveThProductDto
{
    public int? TurnNumber { get; set; }
    public string ProductTruckId { get; set; }
    public string ProjectSequenceCode { get; set; }
}