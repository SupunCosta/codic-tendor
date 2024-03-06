using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.WF;

public class WFActivity
{
    public string Id { get; set; }
    public WFHeader WFHeader { get; set; }
    [ForeignKey("WFHeader")] public string WorkFlowId { get; set; }
    public string RequesterId { get; set; }
    public string ExecutorId { get; set; }
    public DateTime RequiredDateAndTime { get; set; }
    public DateTime ExecutedDateAndTime { get; set; }
    public string EffortEstimate { get; set; }
    public string EffortCompleted { get; set; }
    public string Status { get; set; }
}