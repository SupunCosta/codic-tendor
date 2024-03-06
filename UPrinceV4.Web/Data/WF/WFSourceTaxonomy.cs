using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.WF;

public class WFSourceTaxonomy
{
    public string Id { get; set; }
    public WFHeader WFHeader { get; set; }
    [ForeignKey("WFHeader")] public string WorkFlowId { get; set; }
    public string TaxonomyId { get; set; }
    public string TaxonomyNodeId { get; set; }
}