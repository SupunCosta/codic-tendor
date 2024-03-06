using Newtonsoft.Json;

namespace UPrinceV4.Web.Data.ProjectLocationDetails;

public class Position
{
    [JsonIgnore] public string Id { get; set; }
    public string Lat { get; set; }
    public string Lon { get; set; }
}