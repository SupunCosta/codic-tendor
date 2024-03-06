using System.Collections.Generic;

namespace UPrinceV4.Web.Data.PBS_;

public class PbsTaxonomyLevelLocalizedData
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string LanguageCode { get; set; }
    public string PbsTaxonomyLevelId { get; set; }
    public int Level { get; set; }
    public string TaxonomyId { get; set; }
    public bool IsSearchable { get; set; }
    public bool IsProduct { get; set; }
}

public class TaxonomyLevelDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int Level { get; set; }
    public bool IsSearchable { get; set; }
}

public class TaxonomyLevelReadDto
{
    public IEnumerable<TaxonomyLevelDto> LocationTaxonomyLevels { get; set; }
    public IEnumerable<TaxonomyLevelDto> UtilityTaxonomyLevels { get; set; }
    public IEnumerable<TaxonomyLevelDto> MachineTaxonomyLevels { get; set; }
}