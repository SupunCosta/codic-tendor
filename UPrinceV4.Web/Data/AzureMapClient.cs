using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using UPrinceV4.Web.Data.ProjectLocationDetails;

namespace UPrinceV4.Web.Data;

public class AzureMapClient
{
    private readonly IConfiguration  _configuration;

    public  AzureMapClient (IConfiguration  iConfiguration)
    {
        _configuration = iConfiguration;
    }
    

    public async Task <string> GetAddressByGeoLocations(double lat, double lon)
     {

         var mapKey = _configuration.GetValue<string>("AzureMapKey");

         var client = new HttpClient();

         var geoLocation = lat + "," + lon;
         
         var resultUrl =
             $"https://atlas.microsoft.com/search/address/reverse/json?api-version=1.0&subscription-key={mapKey}&query={geoLocation}";
         var response = await client.GetAsync(resultUrl);
         response.EnsureSuccessStatusCode();
         var content = await response.Content.ReadAsStringAsync();

         var addressResults = JsonConvert.DeserializeObject<GetAddressDto>(content);

         var address = addressResults?.addresses?.FirstOrDefault()?.address.freeformAddress;

         
         return address;
     }
    
    public async Task <SearchResults> GetPositionsByAddress(string address)
    {

        var mapKey = _configuration.GetValue<string>("AzureMapKey");

        var client = new HttpClient();

        var resultUrl =
            $"https://atlas.microsoft.com/search/address/json?api-version=1.0&subscription-key={mapKey}&query={address}";
        var response = await client.GetAsync(resultUrl);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        var addressResults = JsonConvert.DeserializeObject<SearchResultsDto>(content);

        var results = addressResults?.Results.FirstOrDefault();

         
        return results;
    }
}

public class GetAddressDto
{
    public Summary summary { get; set; }
    public List<Addresses> addresses { get; set; }
}
public class Addresses
{
    public GeoAddress address { get; set; }
    public string position { get; set; }
    public string id { get; set; }
}

public class GeoAddress
{
    public string buildingNumber { get; set; }
    public string streetNumber { get; set; }
    public List<object> routeNumbers { get; set; }
    public string street { get; set; }
    public string streetName { get; set; }
    public string streetNameAndNumber { get; set; }
    public string countryCode { get; set; }
    public string countrySubdivision { get; set; }
    public string countrySecondarySubdivision { get; set; }
    public string municipality { get; set; }
    public string postalCode { get; set; }
    public string country { get; set; }
    public string countryCodeISO3 { get; set; }
    public string freeformAddress { get; set; }
    public BoundingBox boundingBox { get; set; }
    public string localName { get; set; }
}

public class BoundingBox
{
    public string northEast { get; set; }
    public string southWest { get; set; }
    public string entity { get; set; }
}

public class GeoPositions
{
    public double Lat { get; set; }
    public double Lon { get; set; }
    public string Address { get; set; }

}

public class SearchResultsDto
{
   
    public List<SearchResults> Results { get; set; }

}

public class SearchResults
{
   
    public GeoPositions Position { get; set; }
    public Address Address { get; set; }

}

