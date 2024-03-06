using System.Collections.Generic;

namespace UPrinceV4.Web.Data.PBS_;

public class PbsService
{
    public string Id { get; set; }
    public string ProductId { get; set; }
    public string Quantity { get; set; }
    public string MouId { get; set; }
    public string UnitPrice { get; set; }
    public string TotalPrice { get; set; }
    public string Comments { get; set; }
}

public class PbsServiceCreateDto
{
    public string ProductId { get; set; }
    public string Quantity { get; set; }
    public string Mou { get; set; }
    public string UnitPrice { get; set; }
    public string TotalPrice { get; set; }
    public string Comments { get; set; }
    public List<string> Documents { get; set; }
}

public class PbsServiceGetByIdDto
{
    public string Id { get; set; }
    public string ProductId { get; set; }
    public string Quantity { get; set; }
    public string Mou { get; set; }
    public string UnitPrice { get; set; }
    public string TotalPrice { get; set; }
    public string Comments { get; set; }
    public List<string> Documents { get; set; }
    public string ProductTitle { get; set; }
    public string ProjectTitle { get; set; }
}

public class Mou
{
    public string Key { get; set; }
    public string Text { get; set; }
}