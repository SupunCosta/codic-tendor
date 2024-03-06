using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.PBS_;

public class PbsProductTaxonomy
{
    public string Id { get; set; }
    [ForeignKey("PbsProduct")] public string PbsProductId { get; set; }
    public virtual PbsProduct PbsProduct { get; set; }

    [ForeignKey("PbsTaxonomy")] public string PbsTaxonomyId { get; set; }
    public virtual PbsTaxonomy PbsTaxonomy { get; set; }
    public string PbsTaxonomyNodeId { get; set; }
}

public class PbsProductTaxonomyCreateDto
{
    public string Id { get; set; }
    public string PbsProductId { get; set; }
    public string PbsTaxonomyId { get; set; }
    public string ParentId { get; set; }
    public string PbsTaxonomyLevelId { get; set; }
}