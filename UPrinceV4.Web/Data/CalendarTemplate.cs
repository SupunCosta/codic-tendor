using System.ComponentModel.DataAnnotations;

namespace UPrinceV4.Web.Data;

public class CalendarTemplate
{
    [Required] public string Id { get; set; }
    public string Name { get; set; }
    public bool IsDefault { get; set; } = false;
}

public class CalendarTemplateCreateDto
{
    [Required] public string Name { get; set; }
    public bool IsDefault { get; set; } = false;
}