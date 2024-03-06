using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace UPrinceV4.Web.Data.GD.Vehicle;

public class Vehicle
{
    [JsonPropertyName("Id")] public string Id { get; set; }

    [JsonPropertyName("Name")] public string Name { get; set; }

    [JsonPropertyName("Code")] public string Code { get; set; }

    [JsonPropertyName("Category")] public string Category { get; set; }

    [JsonPropertyName("FuelType")] public string FuelType { get; set; }

    [JsonPropertyName("Co2Emission")] public int? Co2Emission { get; set; }

    [JsonPropertyName("RowOfSeatsCount")] public int? RowOfSeatsCount { get; set; }

    [JsonPropertyName("SeatsCount")] public int? SeatsCount { get; set; }

    [JsonPropertyName("LicensePlate")] public string? LicensePlate { get; set; }

    [JsonPropertyName("VinNumber")] public string? VinNumber { get; set; }

    [JsonPropertyName("HasTrackingData")] public bool? HasTrackingData { get; set; }

    [JsonPropertyName("LastReportDate")] public DateTime? LastReportDate { get; set; }

    [JsonPropertyName("LastSyncDate")] public DateTime? LastSyncDate { get; set; }

    [JsonPropertyName("Weight")] public int? Weight { get; set; }

    [JsonPropertyName("EmissionStandard")] public string EmissionStandard { get; set; }

    [JsonPropertyName("MaximumSpeed")] public int? MaximumSpeed { get; set; }

    [JsonPropertyName("NominalConsumption")]
    public double? NominalConsumption { get; set; }

    [JsonPropertyName("CostCenter")] public CostCenter? CostCenter { get; set; }

    [JsonPropertyName("Groups")] public List<Group>? Groups { get; set; }
}

public class VehiclePositionDto
{
    public string ResourceId { get; set; }

    public DateTime Date { get; set; }
}

public class VehiclePosition
{
    [JsonProperty("__type")] public string TemperaturesType { get; set; }

    [JsonProperty("Id")] public Guid Id { get; set; }

    [JsonProperty("MileageToday")] public long MileageToday { get; set; }

    [JsonProperty("MileageTotal")] public long MileageTotal { get; set; }

    [JsonProperty("Type")] public long Type { get; set; }

    [JsonProperty("TypeName")] public string TypeName { get; set; }

    [JsonProperty("RtcDateTime")] public DateTimeOffset RtcDateTime { get; set; }

    [JsonProperty("GpsDateTime")] public DateTimeOffset GpsDateTime { get; set; }

    [JsonProperty("Speed")] public long Speed { get; set; }

    [JsonProperty("AllowedSpeed")] public long AllowedSpeed { get; set; }

    [JsonProperty("Heading")] public long Heading { get; set; }

    [JsonProperty("Latitude")] public double Latitude { get; set; }

    [JsonProperty("Longitude")] public double Longitude { get; set; }

    [JsonProperty("Satellites")] public long Satellites { get; set; }

    [JsonProperty("Pois")] public object[] Pois { get; set; }

    [JsonProperty("IoStates")] public object[] IoStates { get; set; }

    [JsonProperty("StatusInfo")] public string StatusInfo { get; set; }

    [JsonProperty("Address")] public Address Address { get; set; }

    [JsonProperty("BadgeNr")] public object BadgeNr { get; set; }

    [JsonProperty("BadgeUser")] public object BadgeUser { get; set; }

    [JsonProperty("ResourceId")] public string ResourceId { get; set; }

    [JsonProperty("_typeDescriptor")] public string TypeDescriptor { get; set; }
}

public class Address
{
    [JsonProperty("__type")] public string Type { get; set; }

    [JsonProperty("CountryName")] public string CountryName { get; set; }

    [JsonProperty("AddressLine")] public string AddressLine { get; set; }

    [JsonProperty("PoiList")] public object[] PoiList { get; set; }

    [JsonProperty("City")] public string City { get; set; }

    [JsonProperty("CityLine")] public string CityLine { get; set; }

    [JsonProperty("CountryCode")] public long CountryCode { get; set; }

    [JsonProperty("HouseNumber")] public string HouseNumber { get; set; }

    [JsonProperty("Latitude")] public string Latitude { get; set; }

    [JsonProperty("Longitude")] public string Longitude { get; set; }

    [JsonProperty("PostalCode")] public string PostalCode { get; set; }

    [JsonProperty("ShortAddressLine")] public string ShortAddressLine { get; set; }

    [JsonProperty("Street")] public string Street { get; set; }

    [JsonProperty("StreetLine")] public string StreetLine { get; set; }

    [JsonProperty("Submunicipality")] public string Submunicipality { get; set; }

    [JsonProperty("_typeDescriptor")] public string TypeDescriptor { get; set; }
}