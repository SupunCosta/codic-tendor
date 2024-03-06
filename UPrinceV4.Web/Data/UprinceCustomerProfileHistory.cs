using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data;

public class UprinceCustomerProfileHistory
{
    public int ID { get; set; }
    public int UprinceCustomerId { get; set; }
    public string VerificationStatus { get; set; }
    public string CompanyName { get; set; }
    [NotMapped] public UprinceCustomerLegalAddressHistory UprinceCustomerLegalAddressHistory { get; set; }
    [NotMapped] public UprinceCustomerPrimaryContactHistory UprinceCustomerPrimaryContactHistory { get; set; }
    public string UserId { get; set; }
    public string Action { get; set; }
    [Key] public DateTime SysStartTime { get; set; }
    public DateTime SysEndTime { get; set; }
}