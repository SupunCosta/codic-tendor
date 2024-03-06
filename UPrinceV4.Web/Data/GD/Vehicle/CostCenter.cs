using System.Text.Json.Serialization;

namespace UPrinceV4.Web.Data.GD.Vehicle;

public class CostCenter
{
    [JsonPropertyName("Id")] public string Id { get; set; }

    [JsonPropertyName("Name")] public string Name { get; set; }

    [JsonPropertyName("Code")] public string Code { get; set; }

    [JsonPropertyName("Poi")] public Poi Poi { get; set; }
}