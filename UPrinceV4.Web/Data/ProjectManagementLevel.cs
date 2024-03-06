using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UPrinceV4.Web.Data;

public class ProjectManagementLevel
{
    [Required] public string Id { get; set; }
    public string Name { get; set; }
    public bool IsDefault { get; set; } = false;
    public string LanguageCode { get; set; }
    public string ProjectManagementLevelId { get; set; }
    public int ListingOrder { get; set; }
}

public class ProjectManagementLevelDto
{
    [Required] public string Id { get; set; }
    public string Name { get; set; }
    public bool IsDefault { get; set; } = false;
    public string LocaleCode { get; set; }
    public int ListingOrder { get; set; }
    public bool IsDeleted { get; set; }
    public IList<ProjectManagementLevelLocalizedData> ProjectManagementLevelLocalizedData { get; set; }
}

public class ProjectManagementLevelCreateDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public bool IsDefault { get; set; } = false;
}