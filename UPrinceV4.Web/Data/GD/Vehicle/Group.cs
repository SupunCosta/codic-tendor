using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace UPrinceV4.Web.Data.GD.Vehicle;

public class Group
{
    [JsonPropertyName("Id")] public string Id { get; set; }

    [JsonPropertyName("Name")] public string Name { get; set; }

    [JsonPropertyName("Pois")] public List<Poi> Pois { get; set; }

    [JsonPropertyName("NumberOfPois")] public int? NumberOfPois { get; set; }

    [JsonPropertyName("Type")] public string Type { get; set; }

    [JsonPropertyName("Code")] public string Code { get; set; }
}