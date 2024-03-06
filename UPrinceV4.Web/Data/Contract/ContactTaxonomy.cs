namespace UPrinceV4.Web.Data.Contract;

public class ContractTaxonomy
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string ContractId { get; set; }
    public string ParentId { get; set; }
    public string ContractTaxonomyLevelId { get; set; }
}

public class ContractTaxonomyDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string ContractId { get; set; }
    public string ParentId { get; set; }
    public string ContractTaxonomyLevelId { get; set; }
}