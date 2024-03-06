using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace UPrinceV4.Web.Data.GD.Vehicle;

public class Poi
{
    [JsonPropertyName("Id")] public string Id { get; set; }

    [JsonPropertyName("Name")] public string Name { get; set; }

    [JsonPropertyName("Code")] public string Code { get; set; }

    [JsonPropertyName("PoiType")] public PoiType PoiType { get; set; }

    [JsonPropertyName("IsAssetLocation")] public bool IsAssetLocation { get; set; }

    [JsonPropertyName("Street")] public string Street { get; set; }

    [JsonPropertyName("HouseNumber")] public string HouseNumber { get; set; }

    [JsonPropertyName("City")] public string City { get; set; }

    [JsonPropertyName("Submunicipality")] public string Submunicipality { get; set; }

    [JsonPropertyName("PostalCode")] public string PostalCode { get; set; }

    [JsonPropertyName("Country")] public string Country { get; set; }

    [JsonPropertyName("Priority")] public string Priority { get; set; }

    [JsonPropertyName("Description")] public string Description { get; set; }

    [JsonPropertyName("GeographyWkt")] public string GeographyWkt { get; set; }

    [JsonPropertyName("LonMin")] public double? LonMin { get; set; }

    [JsonPropertyName("LonMax")] public double? LonMax { get; set; }

    [JsonPropertyName("LatMin")] public double? LatMin { get; set; }

    [JsonPropertyName("LatMax")] public double? LatMax { get; set; }

    [JsonPropertyName("MarkerLon")] public double? MarkerLon { get; set; }

    [JsonPropertyName("MarkerLat")] public double? MarkerLat { get; set; }

    [JsonPropertyName("IsLambert72")] public bool? IsLambert72 { get; set; }

    [JsonPropertyName("Radius")] public double? Radius { get; set; }

    [JsonPropertyName("ReverseGeocoding")] public bool? ReverseGeocoding { get; set; }

    [JsonPropertyName("Groups")] public List<Group> Groups { get; set; }
}