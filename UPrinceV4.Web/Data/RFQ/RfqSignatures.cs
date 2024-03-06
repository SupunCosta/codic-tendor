using System;

namespace UPrinceV4.Web.Data.RFQ;

public class RfqSignatures
{
    public string Id { get; set; }
    public string RfdId { get; set; }
    public string FullName { get; set; }
    public string Signature { get; set; }
    public DateTime? Date { get; set; }
}