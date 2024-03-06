namespace UPrinceV4.Web.Data;

public class UPrinceCustomerJobRole
{
    public int ID { get; set; }

    public string Role { get; set; }
    //public string UserId { get; set; }
    //public string Action { get; set; }
}

public class UPrinceCustomerJobRoleDto
{
    public string Role { get; set; }
}