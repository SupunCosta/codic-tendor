namespace UPrinceV4.Web.Data.PMOL;

public class PmolShortcutpaneData
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LanguageCode { get; set; }
    public int DisplayOrder { get; set; }
    public string Type { get; set; }
    public int Value { get; set; }
}

public class PmolShortcutpaneDataDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public int Value { get; set; }
}