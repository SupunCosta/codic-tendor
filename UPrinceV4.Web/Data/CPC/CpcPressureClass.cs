namespace UPrinceV4.Web.Data.CPC;

public class CpcPressureClass
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LocaleCode { get; set; }
    public int DisplayOrder { get; set; }
}

public class CpcPressureClassDto
{
    public string Key { get; set; }
    public string Text { get; set; }
}