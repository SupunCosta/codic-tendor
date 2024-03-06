namespace UPrinceV4.Web.Data.VisualPlaane;

public class CompetenciesTaxonomyLevel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LevelId { get; set; }
    public string LanguageCode { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsChildren { get; set; } = true;
}