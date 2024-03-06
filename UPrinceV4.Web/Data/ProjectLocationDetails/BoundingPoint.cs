using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace UPrinceV4.Web.Data.ProjectLocationDetails;

public class BoundingPoint
{
    [JsonIgnore] public string Id { get; set; }

    [JsonIgnore] [ForeignKey("Position")] public string TopLeftPointId { get; set; }

    public virtual Position TopLeftPoint { get; set; }

    [JsonIgnore] [ForeignKey("Position")] public string BtmRightPointId { get; set; }

    public virtual Position BtmRightPoint { get; set; }
}