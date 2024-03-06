using System;

namespace UPrinceV4.Web.Data.Contractor;

public class ConstructorWfStatusChangeTime
{
    public string Id { get; set; }
    public string ConstructorWf { get; set; }
    public string StatusId { get; set; }
    public string SubscriptionComment { get; set; }
    public DateTime? DateTime { get; set; }
}

public class GetConstructorWfStatusChangeTime
{
    public string Id { get; set; }
    public string ConstructorWf { get; set; }
    public string StatusId { get; set; }
    public string StatusName { get; set; }
    public string SubscriptionComment { get; set; }
    public DateTime? DateTime { get; set; }
}