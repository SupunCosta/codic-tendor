namespace UPrinceV4.Web.Data.ProjectClassification;

public class ProjectClassificationBuisnessUnit
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string TypeId { get; set; }
    public string LanguageCode { get; set; }
    public int DisplayOrder { get; set; }
    public string Label { get; set; }
}

public class ProjectClassificationTypeDto
{
    public string Key { get; set; }
    public string Text { get; set; }
}