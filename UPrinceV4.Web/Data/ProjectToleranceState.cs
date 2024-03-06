using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UPrinceV4.Web.Data;

public class ProjectToleranceState
{
    [Required] public string Id { get; set; }
    public string Name { get; set; }
    public bool IsDefault { get; set; } = false;
    public string LanguageCode { get; set; }
    public string ProjectToleranceStateId { get; set; }
    public int ListingOrder { get; set; }
}

public class ProjectToleranceStateDto
{
    [Required] public string Id { get; set; }
    public string Name { get; set; }
    public bool IsDefault { get; set; } = false;
    public string LocaleCode { get; set; }
    public int ListingOrder { get; set; }
    public bool IsDeleted { get; set; }
    public IList<ProjectToleranceStateLocalizedData> ProjectToleranceStateLocalizedData { get; set; }
}

public class ProjectToleranceStateCreateDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public bool IsDefault { get; set; } = false;
}