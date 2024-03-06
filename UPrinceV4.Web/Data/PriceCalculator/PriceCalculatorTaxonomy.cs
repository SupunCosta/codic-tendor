namespace UPrinceV4.Web.Data.PriceCalculator;

public class PriceCalculatorTaxonomy
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string ParentId { get; set; }
    public string PriceCalculatorTaxonomyLevelId { get; set; }
    public double? Value { get; set; }
    public int Order { get; set; }
}

public class CreatePriceCalculatorTaxonomy
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string ParentId { get; set; }
    public string PriceCalculatorTaxonomyLevelId { get; set; }
    public double? Value { get; set; }
    public int Order { get; set; }
}