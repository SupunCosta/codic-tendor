namespace UPrinceV4.Web.Data.HR;

public class WorkSchedule
{
    public string Id { get; set; }
    public string Day { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public string DisplayOrder { get; set; }
    public string HRHeaderId { get; set; }
}

public class GetWorkScheduleDto
{
    public string Id { get; set; }
    public string Day { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public string DisplayOrder { get; set; }
    public string HRHeaderId { get; set; }
}