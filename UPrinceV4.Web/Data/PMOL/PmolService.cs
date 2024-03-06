using System.Collections.Generic;

namespace UPrinceV4.Web.Data.PMOL;

public class PmolService
{
    public string Id { get; set; }
    public string PmolId { get; set; }
    public string Quantity { get; set; }
    public string MouId { get; set; }
    public string UnitPrice { get; set; }
    public string TotalPrice { get; set; }
    public string Comments { get; set; }
}

public class PmolServiceCreateDto
{
    public string PmolId { get; set; }
    public string Quantity { get; set; }
    public string Mou { get; set; }
    public string UnitPrice { get; set; }
    public string TotalPrice { get; set; }
    public string Comments { get; set; }
    public List<string> Documents { get; set; }
}

public class PmolServiceGetByIdDto
{
    public string Id { get; set; }
    public string PmolId { get; set; }
    public string Quantity { get; set; }
    public string Mou { get; set; }
    public string UnitPrice { get; set; }
    public string TotalPrice { get; set; }
    public string Comments { get; set; }
    public List<string> Documents { get; set; }
    public string PmolTitle { get; set; }
    public string ProjectTitle { get; set; }
}

public class Mou
{
    public string Key { get; set; }
    public string Text { get; set; }
}