namespace UPrinceV4.Web.Data;

public class ProjectCiawSite
{
    public string Id { get; set; }
    public string CiawSiteCode { get; set; }
    public string CiawSeverEntry { get; set; }
    public string ProjectId { get; set; }
    public bool IsCiawEnabled { get; set; } = false;
}