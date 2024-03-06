namespace UPrinceV4.Web.Data.VisualPlan;

public class MilestoneType
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LanguageCode { get; set; }
    public string TypeId { get; set; }
    public int DisplayOrder { get; set; }
}

public class MilestoneTypeDto
{
    public string key { get; set; }
    public string Text { get; set; }
}