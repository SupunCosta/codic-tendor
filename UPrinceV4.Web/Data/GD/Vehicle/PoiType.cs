using System.Text.Json.Serialization;

namespace UPrinceV4.Web.Data.GD.Vehicle;

public class PoiType
{
    [JsonPropertyName("Id")] public string Id { get; set; }

    [JsonPropertyName("Name")] public string Name { get; set; }

    [JsonPropertyName("Color")] public string Color { get; set; }
}