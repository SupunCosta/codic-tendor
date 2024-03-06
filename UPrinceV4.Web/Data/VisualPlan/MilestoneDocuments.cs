using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.VisualPlan;

public class MilestoneDocuments
{
    public string Id { get; set; }
    public string Link { get; set; }
    public MilestoneHeader MilestoneHeader { get; set; }
    [ForeignKey("MilestoneHeader")] public string MilestoneHeaderId { get; set; }
}