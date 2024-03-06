namespace UPrinceV4.Web.Data.Contractor;

public class ConstructorWorkFlowStatus
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string StatusId { get; set; }
    public string LanguageCode { get; set; }
    public int DisplayOrder { get; set; }
}

public class ConstructorWorkFlowStatusDto
{
    public string Key { get; set; }
    public string Text { get; set; }
}