using System;

namespace UPrinceV4.Web.Data.Contractor;

public class LotStatusChangeTime
{
    public string Id { get; set; }
    public string LotId { get; set; }
    public string StatusId { get; set; }
    public DateTime? DateTime { get; set; }
}

public class GetLotStatusChangeTime
{
    public string Id { get; set; }
    public string LotId { get; set; }
    public string StatusId { get; set; }
    public string StatusName { get; set; }

    public DateTime? DateTime { get; set; }
}