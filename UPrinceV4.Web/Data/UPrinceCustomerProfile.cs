namespace UPrinceV4.Web.Data;

public class UPrinceCustomerProfile
{
    public int ID { get; set; }
    public int UprinceCustomerId { get; set; }
    public string VerificationStatus { get; set; }
    public string CompanyName { get; set; }
    public UPrinceCustomerLegalAddress UprinceCustomerLegalAddress { get; set; }

    public UPrinceCustomerPrimaryContact UprinceCustomerPrimaryContact { get; set; }
    //public string UserId { get; set; }
    //public string Action { get; set; }
}

public class UPrinceCustomerProfileDto
{
    public int UprinceCustomerId { get; set; }
    public string VerificationStatus { get; set; }
    public string CompanyName { get; set; }
}