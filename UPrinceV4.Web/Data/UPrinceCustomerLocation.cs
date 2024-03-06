namespace UPrinceV4.Web.Data;

public class UPrinceCustomerLocation
{
    public int ID { get; set; }
    public int UprinceCustomerId { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string Country { get; set; }

    public string PostalCode { get; set; }
    //public string UserId { get; set; }
    //public string Action { get; set; }
}

public class UPrinceCustomerLocationDto
{
    public int UprinceCustomerId { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string PostalCode { get; set; }
}