namespace UPrinceV4.Web.Data.Contract;

public class ContractDocument
{
    public string id { get; set; }
    public string DocumentId { get; set; }
    public string DocumentName { get; set; }

    public ContractHeader ContractHeader { get; set; }

    public string ContractId { get; set; }
}