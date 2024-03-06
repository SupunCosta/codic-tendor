using System;
using System.ComponentModel.DataAnnotations;

namespace UPrinceV4.Web.Data;

public class UprinceCustomerJobRoleHistory
{
    public int ID { get; set; }
    public string Role { get; set; }
    public string UserId { get; set; }
    public string Action { get; set; }
    [Key] public DateTime SysStartTime { get; set; }
    public DateTime SysEndTime { get; set; }
}