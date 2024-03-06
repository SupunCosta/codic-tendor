using System;

namespace UPrinceV4.Web.Data.WBS;

public class WbsTemplate
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Name { get; set; }
    public string SequenceCode { get; set; }
    public bool IsDelete { get; set; } = false;
    public bool IsDefault { get; set; } = false;
    public DateTime? CreatedDateTime{ get; set; }
    public DateTime? UpdatedDateTime{ get; set; }
    public string CreatedBy{ get; set; }
    public string UpdatedBy{ get; set; }
}