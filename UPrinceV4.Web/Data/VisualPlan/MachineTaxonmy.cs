namespace UPrinceV4.Web.Data.VisualPlan;

public class MachineTaxonmy
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string MilestoneId { get; set; }
    public string ParentId { get; set; }
    public string MachineTaxonomyLevelId { get; set; }
}

public class MachineTaxonmyDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string MilestoneId { get; set; }
    public string ParentId { get; set; }
    public string MachineTaxonomyLevelId { get; set; }
}