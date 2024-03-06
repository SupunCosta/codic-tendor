namespace UPrinceV4.Web.Data;

public class ProjectConfiguration
{
    public string Id { get; set; }
    public string ProjectId { get; set; }
    public string UnitPrice { get; set; }
}

public class ProjectConfigurationDto
{
    public string Id { get; set; }
    public string ProjectId { get; set; }
    public string UnitPrice { get; set; }
    public bool IsRequestingData { get; set; }
}