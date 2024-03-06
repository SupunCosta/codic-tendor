using System;

namespace UPrinceV4.Web.Data.PO;

public class POLabourTeam
{
    public string Id { get; set; }

    public string POId { get; set; }

    //public string Title { get; set; }
    public string PersonId { get; set; }
    public string CPCId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class GetPOLabourTeam
{
    public string Id { get; set; }
    public string POId { get; set; }
    public string PersonId { get; set; }
    public string PersonName { get; set; }
    public string CPCId { get; set; }
    public string ProjectSequenceCode { get; set; }
    public string ParentId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}