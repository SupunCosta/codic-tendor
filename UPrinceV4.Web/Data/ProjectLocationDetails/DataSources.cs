using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace UPrinceV4.Web.Data.ProjectLocationDetails;

public class DataSources
{
    [JsonIgnore] public string Id { get; set; }

    [JsonIgnore] [ForeignKey("Geometry")] public string GeometryId { get; set; }

    public virtual Geometry Geometry { get; set; }
}