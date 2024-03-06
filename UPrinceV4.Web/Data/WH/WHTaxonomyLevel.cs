namespace UPrinceV4.Web.Data.WH;

public class WHTaxonomyLevel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LevelId { get; set; }
    public string LanguageCode { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsChildren { get; set; } = true;
}

public class WHTaxonomyLevelDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LanguageCode { get; set; }
    public int DisplayOrder { get; set; }
}