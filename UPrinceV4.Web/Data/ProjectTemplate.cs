using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UPrinceV4.Web.Data;

public class ProjectTemplate
{
    [Required] public string Id { get; set; }
    public string Name { get; set; }
    public string LanguageCode { get; set; }
    public string TemplateId { get; set; }
}

public class ProjectTemplateDto
{
    [Required] public string Id { get; set; }
    public string Name { get; set; }
    public string LocaleCode { get; set; }
    public bool IsDeleted { get; set; }
    public IList<ProjectTemplateLocalizedData> ProjectTemplateLocalizedData { get; set; }
}

public class ProjectTemplateCreateDto
{
    public string Name { get; set; }
    public string Id { get; set; }
}