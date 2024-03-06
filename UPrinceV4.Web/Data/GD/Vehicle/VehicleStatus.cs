using System;
using Newtonsoft.Json;

namespace UPrinceV4.Web.Data.GD.Vehicle;

public class VehicleStatus
{
    [JsonProperty("__type")] public string Type { get; set; }

    [JsonProperty("ResourceId")] public Guid ResourceId { get; set; }

    [JsonProperty("StatusResults")] public StatusResult[] StatusResults { get; set; }

    [JsonProperty("_typeDescriptor")] public string TypeDescriptor { get; set; }
}

public class StatusResult
{
    [JsonProperty("__type")] public string Type { get; set; }

    [JsonProperty("ResourceId")] public Guid ResourceId { get; set; }

    [JsonProperty("ResourceName")] public string ResourceName { get; set; }

    [JsonProperty("FromDate")] public DateTimeOffset FromDate { get; set; }

    [JsonProperty("ToDate")] public DateTimeOffset ToDate { get; set; }

    [JsonProperty("Bars")] public Bar[] Bars { get; set; }

    [JsonProperty("_typeDescriptor")] public string TypeDescriptor { get; set; }
}

public class Bar
{
    [JsonProperty("__type")] public string Type { get; set; }

    [JsonProperty("StateName")] public string StateName { get; set; }

    [JsonProperty("Start")] public DateTimeOffset Start { get; set; }

    [JsonProperty("Stop")] public DateTimeOffset Stop { get; set; }

    [JsonProperty("Duration")] public DateTimeOffset Duration { get; set; }

    [JsonProperty("MileageBirdFlight")] public long MileageBirdFlight { get; set; }

    [JsonProperty("MileageDriven")] public long MileageDriven { get; set; }

    [JsonProperty("DriverId")] public object DriverId { get; set; }

    [JsonProperty("DriverName")] public object DriverName { get; set; }

    [JsonProperty("StartLocation")] public StartLocation StartLocation { get; set; }

    [JsonProperty("StopLocation")] public StartLocation StopLocation { get; set; }

    [JsonProperty("_typeDescriptor")] public string TypeDescriptor { get; set; }
}

public class StartLocation
{
    [JsonProperty("__type")] public string StartLocationType { get; set; }

    [JsonProperty("Id")] public Guid Id { get; set; }

    [JsonProperty("MileageToday")] public long MileageToday { get; set; }

    [JsonProperty("MileageTotal")] public double MileageTotal { get; set; }

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

    [JsonProperty("Address")] public Address1 Address { get; set; }

    [JsonProperty("BadgeNr")] public object BadgeNr { get; set; }

    [JsonProperty("BadgeUser")] public object BadgeUser { get; set; }

    [JsonProperty("ResourceId")] public Guid ResourceId { get; set; }

    [JsonProperty("_typeDescriptor")] public string TypeDescriptor { get; set; }
}

public class Address1
{
    [JsonProperty("__type")] public string Type { get; set; }

    [JsonProperty("CountryName")] public string CountryName { get; set; }

    [JsonProperty("AddressLine")] public string AddressLine { get; set; }

    [JsonProperty("PoiList")] public object[] PoiList { get; set; }

    [JsonProperty("City")] public string City { get; set; }

    [JsonProperty("CityLine")] public string CityLine { get; set; }

    [JsonProperty("CountryCode")] public long CountryCode { get; set; }

    [JsonProperty("HouseNumber")] public long HouseNumber { get; set; }

    [JsonProperty("Latitude")] public object Latitude { get; set; }

    [JsonProperty("Longitude")] public object Longitude { get; set; }

    [JsonProperty("PostalCode")] public long PostalCode { get; set; }

    [JsonProperty("ShortAddressLine")] public string ShortAddressLine { get; set; }

    [JsonProperty("Street")] public string Street { get; set; }

    [JsonProperty("StreetLine")] public string StreetLine { get; set; }

    [JsonProperty("Submunicipality")] public string Submunicipality { get; set; }

    [JsonProperty("_typeDescriptor")] public string TypeDescriptor { get; set; }
}