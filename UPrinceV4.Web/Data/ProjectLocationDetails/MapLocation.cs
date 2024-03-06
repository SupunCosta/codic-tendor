using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace UPrinceV4.Web.Data.ProjectLocationDetails;

public class MapLocation
{
    public string Id { get; set; }
    public string Type { get; set; }
    public string Score { get; set; }
    public string EntityType { get; set; }

    [JsonIgnore] [ForeignKey("Address")] public string AddressId { get; set; }

    public virtual Address Address { get; set; }

    [JsonIgnore] [ForeignKey("Position")] public string PositionId { get; set; }

    public Position Position { get; set; }

    [JsonIgnore]
    [ForeignKey("BoundingPoint")]
    public string ViewportId { get; set; }

    public virtual BoundingPoint Viewport { get; set; }

    [JsonIgnore]
    [ForeignKey("BoundingPoint")]
    public string BoundingBoxId { get; set; }

    public virtual BoundingPoint BoundingBox { get; set; }

    [JsonIgnore]
    [ForeignKey("DataSources")]
    public string DataSourcesId { get; set; }

    public virtual DataSources DataSources { get; set; }
}