using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.WF;

public class WFHistoryLog
{
    public string Id { get; set; }
    public WFHeader WFHeader { get; set; }
    [ForeignKey("WFHeader")] public string WorkFlowId { get; set; }
    public string CreatedBy { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}