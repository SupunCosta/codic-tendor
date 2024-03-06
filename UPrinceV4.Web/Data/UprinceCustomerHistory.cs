using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data;

public class UprinceCustomerHistory
{
    public int Id { get; set; }
    [NotMapped] public UprinceCustomerProfileHistory UprinceCustomerProfileHistory { get; set; }
    [NotMapped] public IList<UprinceCustomerLocationHistory> UprinceCustomerLocationHistory { get; set; }
    [NotMapped] public UprinceCustomerContactPreferenceHistory UprinceCustomerContactPreferenceHistory { get; set; }
    public string UserId { get; set; }
    public string Action { get; set; }
    [Key] public DateTime SysStartTime { get; set; }
    public DateTime SysEndTime { get; set; }
}