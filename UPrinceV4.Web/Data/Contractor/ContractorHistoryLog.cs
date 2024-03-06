using System;

namespace UPrinceV4.Web.Data.Contractor;

public class ContractorHistoryLog
{
    public string Id { get; set; }
    public string LotId { get; set; }
    public string CreatedBy { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? DateTime { get; set; }
}