namespace UPrinceV4.Web.Data.Contractor;

public class ContractorTotalValues
{
    public string Id { get; set; }
    public string LotId { get; set; }
    public string CompanyId { get; set; }
    public float TotalBAFO { get; set; }
    public float TotalCost { get; set; }
    public bool IsWinner { get; set; }
}

public class ContractorTotalValuesDto
{
    public string CompanyId { get; set; }
    public float TotalBAFO { get; set; }
    public float TotalCost { get; set; }
    public bool IsWinner { get; set; }
    public string LotId { get; set; }
    public string Id { get; set; }
}