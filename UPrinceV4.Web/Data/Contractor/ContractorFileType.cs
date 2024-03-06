namespace UPrinceV4.Web.Data.Contractor;

public class ContractorFileType
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string TypeId { get; set; }
    public string LanguageCode { get; set; }
    public int DisplayOrder { get; set; }
}

public class ContractorFileTypeDto
{
    public string Key { get; set; }
    public string Text { get; set; }
}