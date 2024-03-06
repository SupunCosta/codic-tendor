using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.PBS_;

public class PbsTaxonomyLocalizedData
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string LanguageCode { get; set; }

    [ForeignKey("PbsTaxonomy")] public string PbsTaxonomyId { get; set; }
    public PbsTaxonomy PbsProductStatus { get; set; }
}