using System;

namespace UPrinceV4.Web.Data.WH;

public class WHRockCpc
{
    public string Id { get; set; }
    public string CpcId { get; set; }
    public string Quantity { get; set; }
    public string Comment { get; set; }
    public string CommentUrl { get; set; }
    public bool Correct { get; set; }
    public string StockId { get; set; }
    public string UserId { get; set; }
    public DateTime Date { get; set; }
    public DateTime DateTime { get; set; }
}

public class WHRockCpcFilter
{
    public string type { get; set; }
    public string text { get; set; }
    public Sorter Sorter { get; set; }
}

public class Sorter
{
    public string Attribute { get; set; }
    public string Order { get; set; }
}

public class WHRockCpcList
{
    public string Id { get; set; }
    public string CpcId { get; set; }
    public string Quantity { get; set; }
    public string Comment { get; set; }
    public string CommentUrl { get; set; }
    public bool Correct { get; set; }
    public string StockId { get; set; }
    public string ResourceItem { get; set; }
    public string Location { get; set; }
    public string LocationId { get; set; }
    public DateTime? Date { get; set; }
    public string UserId { get; set; }
    public DateTime DateTime { get; set; }
}

public class WHRockCpcDto
{
    public string CpcId { get; set; }
    public string Quantity { get; set; }
    public string Comment { get; set; }
    public string CommentUrl { get; set; }
    public bool Correct { get; set; }
    public string StockId { get; set; }
}