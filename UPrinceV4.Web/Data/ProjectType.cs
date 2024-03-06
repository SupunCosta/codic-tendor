using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UPrinceV4.Web.Data;

public class ProjectType
{
    [Required] public string Id { get; set; }
    public string Name { get; set; }
    public bool IsDefault { get; set; } = false;
    public string LanguageCode { get; set; }
    public string ProjectTypeId { get; set; }
}

public class ProjectTypeDto
{
    [Required] public string Id { get; set; }
    public string Name { get; set; }
    public bool IsDefault { get; set; } = false;
    public string LocaleCode { get; set; }
    public bool IsDeleted { get; set; }
    public IList<ProjectTypeLocalizedData> ProjectTypeLocalizedData { get; set; }
}

public class ProjectTypeCreateDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public bool IsDefault { get; set; } = false;
}