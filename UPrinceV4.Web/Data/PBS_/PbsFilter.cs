namespace UPrinceV4.Web.Data.PBS_;

public class PbsFilter
{
    public string Title { get; set; }
    public string PbsProductItemTypeId { get; set; }
    public string PbsProductStatusId { get; set; }
    public string PbsToleranceStateId { get; set; }
    public string Scope { get; set; }
    public Sorter Sorter { get; set; }
    public string Taxonomy { get; set; }
    public string Type { get; set; }
    
    public string QualityProducerId { get; set; }

}

public class PbsFilterByTaxonomyLevel
{
    public string Title { get; set; }
    public string PbsTaxonomyLevelId { get; set; }
}