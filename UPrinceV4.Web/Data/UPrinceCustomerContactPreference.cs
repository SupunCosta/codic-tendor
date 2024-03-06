namespace UPrinceV4.Web.Data;

public class UPrinceCustomerContactPreference
{
    public int ID { get; set; }
    public int UPrinceCustomerJobRoleId { get; set; }
    public int UprinceCustomerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string Email { get; set; }
    //public string UserId { get; set; }
    //public string Action { get; set; }
}

public class UPrinceCustomerContactPreferenceDto
{
    public int UPrinceCustomerJobRoleId { get; set; }
    public int UprinceCustomerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
}