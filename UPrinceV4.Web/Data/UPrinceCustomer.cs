using System.Collections.Generic;

namespace UPrinceV4.Web.Data;

public class UPrinceCustomer
{
    public UPrinceCustomer()
    {
        UprinceCustomerProfile = new UPrinceCustomerProfile();
        UprinceCustomerContactPreference = new UPrinceCustomerContactPreference();
        UprinceCustomerLocations = new List<UPrinceCustomerLocation>();
    }

    public int Id { get; set; }
    public UPrinceCustomerProfile UprinceCustomerProfile { get; set; }
    public IList<UPrinceCustomerLocation> UprinceCustomerLocations { get; set; }

    public UPrinceCustomerContactPreference UprinceCustomerContactPreference { get; set; }
    //public string UserId { get; set; }
    //public string Action { get; set; }
}

public class UPrinceCustomerDto
{
}