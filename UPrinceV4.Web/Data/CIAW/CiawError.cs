using System;

namespace UPrinceV4.Web.Data.CIAW;

public class CiawError
{
    public string Id { get; set; }
    public string CiawId { get; set; }
    public string errorCode { get; set; }
    public string errorDescription { get; set; }
    public DateTime RequestDateTime { get; set; }
}