namespace UPrinceV4.Web.Data.VisualPlan;

public class VpFilterDropdown
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LanguageCode { get; set; }
    public string TypeId { get; set; }
    public int DisplayOrder { get; set; }
    public string Label { get; set; }
}

public class VpFilterDropdownDto
{
    public string key { get; set; }
    public string Text { get; set; }
}