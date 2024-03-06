namespace UPrinceV4.Web.Data.Contractor;

public class ContractorSupplierList
{
    public string Id { get; set; }
    public string CabPersonId { get; set; }
    public string CabPersonName { get; set; }
    public string FileName { get; set; }
    public string ContractorId { get; set; }
}

public class ContractorSupplierListDto
{
    public string Id { get; set; }
    public string CabPersonId { get; set; }
    public string CabPersonName { get; set; }
    public string FileName { get; set; }
    public string ContractorId { get; set; }
    public string Company { get; set; }
}