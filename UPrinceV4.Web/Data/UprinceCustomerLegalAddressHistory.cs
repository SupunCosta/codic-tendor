namespace UPrinceV4.Web.Data;

public class UprinceCustomerLegalAddressHistory
{
    public int ID { get; set; }
    public int UprinceCustomerProfileId { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string Country { get; set; }

    public string PostalCode { get; set; }
    //public string UserId { get; set; }
    //public string Action { get; set; }
    //[Key]
    //public System.DateTime SysStartTime { get; set; }
    //public System.DateTime SysEndTime { get; set; }
}