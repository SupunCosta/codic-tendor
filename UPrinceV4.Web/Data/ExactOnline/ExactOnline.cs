using System;

namespace UPrinceV4.Web.Data.ExactOnline;

public class ExactOnlineResponse
{
    public ExactOnline Content { get; set; }
    public string HashCode { get; set; }
}

public class ExactOnline
{
    public string Id { get; set; }
    public string Topic { get; set; }
    public string Action { get; set; }
    public string Division { get; set; }
    public string Key { get; set; }
    public string ExactOnlineEndpoint { get; set; }
    public DateTime EventCreatedOn { get; set; }
}