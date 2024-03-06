using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UPrinceV4.Web.Data.CAB;
using UPrinceV4.Web.Repositories.Interfaces.PBS;

namespace UPrinceV4.Web.Data;

public class TimeClock
{
    public string Id { get; set; }

    public string UserId { get; set; }
    [NotMapped] public CabPersonCompany User { get; set; }
    public int Type { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }
    public string FromLocation { get; set; }
    public string ToLocation { get; set; }

    [ForeignKey("QRCode")] public string QRCodeId { get; set; }
    public virtual QRCode QRCode { get; set; }

    [ForeignKey("Location")] public string LocationId { get; set; }
    public virtual Location Location { get; set; }

    [ForeignKey("Shift")] public string ShiftId { get; set; }
    public virtual Shift Shift { get; set; }

    public string ProjectId { get; set; }
    public string PmolId { get; set; }
}

public class TimeClockHistory : HistoryMetaData
{
    public string DataJson { get; set; }
}

public class CreateTimeClockDto
{
    public int Type { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string FromLocation { get; set; }
    public string ToLocation { get; set; }
    public createLocationDto Location { get; set; }

    [Required] public string QRCodeId { get; set; }
    public DateTime LocalTime { get; set; }
    public string TimeZone { get; set; }
    public string ShiftId { get; set; }
    public string PmolId { get; set; }
    public bool IsBreak { get; set; }
    public bool IsForeman { get; set; }
    public IPbsResourceRepository PbsResourceRepository { get; set; }
    public bool IsShiftStart { get; set; }

}

public class UpdateTimeClockDto
{
    [Required] public string Id { get; set; }
    public int Type { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string FromLocation { get; set; }
    public string ToLocation { get; set; }
    public Location Location { get; set; }

    [Required] public string QRCodeId { get; set; }
    public DateTime LocalTime { get; set; }
    public string TimeZone { get; set; }
    public string ShiftId { get; set; }
}

public class TimeClocklListHistoryDto
{
    public string Id { get; set; }
    public string DataJson { get; set; }
    public string HistoryLog { get; set; }
    public string ChangedByUserId { get; set; }
    public string Action { get; set; }
    public DateTime ChangedTime { get; set; }
    public int RevisionNumber { get; set; }
}

public class PmolJobDone
{
    public string PmolId { get; set; }
    public string Message { get; set; }
    public string IsJobDone { get; set; }
}

public class PmolLabourEndTime
{
    public string PmolId { get; set; }
    public string LabourId { get; set; }
    public DateTime EndDateTime { get; set; }
}