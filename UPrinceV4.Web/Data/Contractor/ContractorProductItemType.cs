namespace UPrinceV4.Web.Data.Contractor;

public class ContractorProductItemType
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string TypeId { get; set; }
    public string LanguageCode { get; set; }
    public int DisplayOrder { get; set; }
}

public class ContractorProductItemTypeDto
{
    public string Key { get; set; }
    public string Text { get; set; }
}