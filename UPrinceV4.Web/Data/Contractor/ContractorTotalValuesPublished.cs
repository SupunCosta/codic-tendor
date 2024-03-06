namespace UPrinceV4.Web.Data.Contractor;

public class ContractorTotalValuesPublished
{
    public string Id { get; set; }
    public string LotId { get; set; }
    public string CompanyId { get; set; }
    public float TotalBAFO { get; set; }
    public float TotalCost { get; set; }
    public bool IsWinner { get; set; }
}

public class ContractorTotalValuesPublishedDto
{
    public string CompanyId { get; set; }
    public float TotalBAFO { get; set; }
    public float TotalCost { get; set; }
    public bool IsWinner { get; set; }
    public string LotId { get; set; }
    public string Id { get; set; }
}