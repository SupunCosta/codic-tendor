namespace UPrinceV4.Web.Models;

public class RiskFilterModel
{
    public string Title { get; set; }
    public string TypeId { get; set; }
    public string StateId { get; set; }
    public string PersonId { get; set; }
    public string PbsProductId { get; set; }
    public RiskSortingModel SortingModel { get; set; }
}

public class RiskSortingModel
{
    public string Attribute { get; set; }
    public string Order { get; set; }
}