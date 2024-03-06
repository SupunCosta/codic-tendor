using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data;

public class ProjectCostConversion
{
    public string Id { get; set; }

    [ForeignKey("ProjectDefinition")] public string ProjectId { get; set; }
    public virtual ProjectDefinition ProjectDefinition { get; set; }
    public string TravelConversionOption { get; set; }
    public string LoadingConversionOption { get; set; }
}

public class ProjectCostConversionCreateDto
{
    public string ProjectId { get; set; }
    public string TravelConversionOption { get; set; } // 100 = none, 200 = time- work, 300 = time -distance
    public string LoadingConversionOption { get; set; } //  100 = none, 200 = work
}