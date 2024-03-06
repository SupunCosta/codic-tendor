using System.ComponentModel.DataAnnotations;

namespace UPrinceV4.Web.Data;

public class Currency
{
    [Required] public int Id { get; set; }
    public string Name { get; set; }
    public bool IsDefault { get; set; } = false;
}

public class CurrencyCreateDto
{
    public string Name { get; set; }
    public bool IsDefault { get; set; } = false;
}