using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UPrinceV4.Web.Data;

public class ProjectState
{
    [Required] public string Id { get; set; }
    public string Name { get; set; }
    public bool IsDefault { get; set; } = false;
    public string LocaleCode { get; set; }
    public bool IsDeleted { get; set; }
    public IList<ProjectStateLocalizedData> ProjectStateLocalizedData { get; set; }
}

public class ProjectStateCreateDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public bool IsDefault { get; set; } = false;
}