using Newtonsoft.Json;

namespace UPrinceV4.Web.Data.ProjectLocationDetails;

public class Address
{
    [JsonIgnore] public string Id { get; set; }
    public string Municipality { get; set; }
    public string CountrySecondarySubdivision { get; set; }
    public string CountrySubdivision { get; set; }
    public string CountryCode { get; set; }
    public string Country { get; set; }
    public string CountryCodeISO3 { get; set; }
    public string FreeformAddress { get; set; }
}