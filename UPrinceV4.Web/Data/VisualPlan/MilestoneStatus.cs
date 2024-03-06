namespace UPrinceV4.Web.Data.VisualPlan;

public class MilestoneStatus
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LanguageCode { get; set; }
    public string StatusId { get; set; }
    public int DisplayOrder { get; set; }
}

public class MilestoneStatusDto
{
    public string key { get; set; }
    public string Text { get; set; }
}