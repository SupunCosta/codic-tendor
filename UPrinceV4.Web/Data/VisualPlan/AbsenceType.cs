namespace UPrinceV4.Web.Data.VisualPlan;

public class AbsenceType
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LanguageCode { get; set; }
    public string TypeId { get; set; }
    public int DisplayOrder { get; set; }
}

public class AbsenceTypeDto
{
    public string key { get; set; }
    public string Text { get; set; }
}