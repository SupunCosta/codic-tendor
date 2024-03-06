using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.CAB;

public class CabAddress
{
    public string Id { get; set; }
    public string Street { get; set; }
    public string StreetNumber { get; set; }
    public string MailBox { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
    public string Region { get; set; }
    [ForeignKey("Country")] public string CountryId { get; set; }
    public virtual Country Country { get; set; }
}