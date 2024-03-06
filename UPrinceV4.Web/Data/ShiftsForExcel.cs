using System;
using System.Collections.Generic;

namespace UPrinceV4.Web.Data;

public class ShiftsForExcel
{
    public string ShiftId { get; set; }
    public string Employee { get; set; }
    public string Employer { get; set; }
    public DateTime ShiftStartDateTime { get; set; }
    public DateTime ShiftEndDateTime { get; set; }
    public DateTime ActivityStartTime { get; set; }
    public DateTime ActivityEndTime { get; set; }
    public TimeSpan TotalTime { get; set; }
    public string ActivityType { get; set; }
    public string ProjectTitle { get; set; }
    public string Location { get; set; }
    public string VehicleNumber { get; set; }
    public string Role { get; set; }
}

public class ExcelReadDto
{
    public string Id { get; set; }
    public string ShiftId { get; set; }
    public string UserId { get; set; }
    public DateTime? ShiftEndDateTime { get; set; }
    public DateTime? ShiftStartDateTime { get; set; }
    public string Status { get; set; }
    public string WorkflowStateId { get; set; }
    public string Type { get; set; }
    public DateTime? ActivityStartTime { get; set; }
    public DateTime? ActivityEndTime { get; set; }
    public string Location { get; set; }
    public string TravellerType { get; set; }
    public string ActivityType { get; set; }
    public string VehicleNo { get; set; }
    public TimeSpan TotalTime { get; set; }
    public string ProjectTitle { get; set; }
    public string Employee { get; set; }
    public string Employer { get; set; }
    public string ProjectId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Distance { get; set; }
    public DateTime? StartDateTimeRoundNearest { get; set; }
    public DateTime? EndDateTimeRoundNearest { get; set; }
    public string TotalTimeRoundNearest { get; set; }
    public DateTime TotalTimeRoundNearestDateFormat { get; set; }
}

public class CabInfoDto
{
    public string OId { get; set; }

    public string Employee { get; set; }
    public string Employer { get; set; }
}

public class Summary
{
    public int lengthInMeters { get; set; }
    public int travelTimeInSeconds { get; set; }
    public int trafficDelayInSeconds { get; set; }
    public DateTime departureTime { get; set; }
    public DateTime arrivalTime { get; set; }
}

public class Summary2
{
    public int lengthInMeters { get; set; }
    public int travelTimeInSeconds { get; set; }
    public int trafficDelayInSeconds { get; set; }
    public DateTime departureTime { get; set; }
    public DateTime arrivalTime { get; set; }
}

public class Point
{
    public double latitude { get; set; }
    public double longitude { get; set; }
}

public class Leg
{
    public Summary2 summary { get; set; }
    public List<Point> points { get; set; }
}

public class Section
{
    public int startPointIndex { get; set; }
    public int endPointIndex { get; set; }
    public string sectionType { get; set; }
    public string travelMode { get; set; }
}

public class Route
{
    public Summary summary { get; set; }
    public List<Leg> legs { get; set; }
    public List<Section> sections { get; set; }
}

public class Response
{
    public string formatVersion { get; set; }
    public List<Route> routes { get; set; }
}

public class BatchItem
{
    public int statusCode { get; set; }
    public Response response { get; set; }
}

public class Summary3
{
    public int successfulRequests { get; set; }
    public int totalRequests { get; set; }
}

public class Root
{
    public List<BatchItem> batchItems { get; set; }
    public Summary3 summary { get; set; }
}