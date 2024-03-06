namespace UPrinceV4.Web.Data.WBS;

public class WbsTaxonomyLevel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LevelId { get; set; }
    public string LanguageCode { get; set; }
    public string DisplayOrder { get; set; }
}

public class WbsTaxonomyLevelDto
{
    public string Key { get; set; }
    public string Text { get; set; }
}