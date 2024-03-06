namespace UPrinceV4.Web.Data.HR;

public class HRRoles
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string RoleId { get; set; }
    public string LanguageCode { get; set; }
    public string Label { get; set; }
}

public class GetHRRoles
{
    public string Key { get; set; }
    public string Text { get; set; }
}