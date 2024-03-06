namespace UPrinceV4.Web.Data.Contractor;

public class ContractorFinalDeliveryDocs
{
    public string Id { get; set; }
    public string LotId { get; set; }
    public string TypeId { get; set; }
    public string Link { get; set; }
    public string Title { get; set; }
}

public class ContractorFinalDeliveryDocsDto
{
    public string Id { get; set; }
    public string LotId { get; set; }
    public string Link { get; set; }
    public string Title { get; set; }
    public string TypeId { get; set; }
    public string TypeName { get; set; }
    public string Count { get; set; }
}