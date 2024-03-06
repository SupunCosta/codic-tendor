namespace UPrinceV4.Web.Data.VisualPlan;

public class OrganizationTeamRole
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string RoleId { get; set; }
    public string LanguageCode { get; set; }
    public int DisplayOrder { get; set; }
}

public class OrganizationTeamRoleDto
{
    public string Key { get; set; }
    public string Text { get; set; }
}