namespace UPrinceV4.Web.Data.VisualPlan;

public class OrganizationTaxonomyLevel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LevelId { get; set; }
    public string LanguageCode { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsChildren { get; set; } = true;
}

public class OrganizationTaxonomyLevelDropdown
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LevelId { get; set; }
    public string LanguageCode { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsChildren { get; set; } = true;
    public string Label { get; set; }
}