namespace UPrinceV4.Web.Data.VisualPlan;

public class CorporateSheduleTime
{
    public string Id { get; set; }
    public string Day { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public string DisplayOrder { get; set; }
    public string CorporateSheduleId { get; set; }
}

public class CorporateSheduleTimeDto
{
    public string Id { get; set; }
    public string Day { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public string DisplayOrder { get; set; }
    public string CorporateSheduleId { get; set; }
}