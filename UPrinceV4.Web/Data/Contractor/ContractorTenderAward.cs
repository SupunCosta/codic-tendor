namespace UPrinceV4.Web.Data.Contractor;

public class ContractorTenderAward
{
    public string Id { get; set; }
    public string LotId { get; set; }
    public string ContractorId { get; set; }
    public string Price { get; set; }
    public bool IsWinner { get; set; }
}