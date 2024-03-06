namespace UPrinceV4.Web.Data.PriceCalculator;

public class PriceCalculatorTaxonomyLevel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LevelId { get; set; }
    public string LanguageCode { get; set; }
    public int DisplayOrder { get; set; }
}

public class GetPriceCalculatorTaxonomyLevel
{
    public string Key { get; set; }
    public string Text { get; set; }
}