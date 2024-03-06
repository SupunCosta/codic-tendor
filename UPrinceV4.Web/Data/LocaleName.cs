using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace UPrinceV4.Web.Data;

public class LocaleName
{
    public string Id { get; set; }
    public int TenantId { get; set; }

    [Required] public string Language { get; set; }
    public string Country { get; set; }

    public string Icon { get; set; }

    [Required]
    //[RegularExpression(".[a-z]{2}-[A-Z]{2}$", ErrorMessage = "Invalid Locale")]
    public string Locale { get; set; }
}

public class LocaleNameCreateDto
{
    public int TenantId { get; set; }

    [Required] public string Language { get; set; }
    public string Country { get; set; }
    public IFormFile Icon { get; set; }

    [Required]
    //[RegularExpression(".[a-z]{2}-[A-Z]{2}$", ErrorMessage = "Invalid Locale")]
    public string Locale { get; set; }
}

public class LocaleNameUpdateDto
{
    public string Id { get; set; }
    public int TenantId { get; set; }

    [Required] public string Language { get; set; }
    public string Country { get; set; }
    public IFormFile Icon { get; set; }

    [Required]
    //[RegularExpression(".[a-z]{2}-[A-Z]{2}$", ErrorMessage = "Invalid Locale")]
    public string Locale { get; set; }
}