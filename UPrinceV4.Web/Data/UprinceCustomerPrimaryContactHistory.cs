using System;
using System.ComponentModel.DataAnnotations;

namespace UPrinceV4.Web.Data;

public class UprinceCustomerPrimaryContactHistory
{
    public int ID { get; set; }
    public int UprinceCustomerProfileId { get; set; }
    public string Name { get; set; }
    public string phone { get; set; }
    public string Email { get; set; }
    public string UserId { get; set; }
    public string Action { get; set; }
    [Key] public DateTime SysStartTime { get; set; }
    public DateTime SysEndTime { get; set; }
}