using System;
using System.ComponentModel.DataAnnotations;

namespace UPrinceV4.Web.Data;

public class UprinceCustomerLocationHistory
{
    public int ID { get; set; }
    public int UprinceCustomerId { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string PostalCode { get; set; }
    public string UserId { get; set; }
    public string Action { get; set; }
    [Key] public DateTime SysStartTime { get; set; }
    public DateTime SysEndTime { get; set; }
}