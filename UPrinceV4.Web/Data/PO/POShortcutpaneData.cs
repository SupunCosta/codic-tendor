namespace UPrinceV4.Web.Data.PO;

public class POShortcutpaneData
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LanguageCode { get; set; }
    public int DisplayOrder { get; set; }
    public string Type { get; set; }
    public int Value { get; set; }
}

public class POShortcutpaneDataDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Value { get; set; }
    public int DisplayOrder { get; set; }
    public string Label { get; set; }
}