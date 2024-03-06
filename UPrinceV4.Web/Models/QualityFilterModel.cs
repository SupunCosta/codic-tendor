namespace UPrinceV4.Web.Models;

public class QualityFilterModel
{
    public string Title { get; set; }
    public QualitySortingModel SortingModel { get; set; }
    public string PbsProductId { get; set; }
}

public class QualitySortingModel
{
    public string Attribute { get; set; }
    public string Order { get; set; }
}