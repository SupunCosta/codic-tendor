namespace UPrinceV4.Web.Data.Contractor;

public class ContractorPsOrderNumber
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string TypeId { get; set; }
    public string LanguageCode { get; set; }
}

public class GetContractorPsOrderNumber
{
    public string Key { get; set; }
    public string Text { get; set; }
    public bool IsPublished { get; set; }
    public bool IsExist { get; set; }
}