using System;

namespace UPrinceV4.Web.Data.PBS_;

public class PbsDynamicAttributes
{
    public string Id { get; set; }
    public string ProductId { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
}

public class GetPbsDynamicAttributes
{
    public string Id { get; set; }
    public string ProductId { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
    public DateTime StartDate { get; set; }
}

public class PbsDynamicAttributesDto
{
    private string _value;
    public string Id { get; set; }
    public string ProductId { get; set; }
    public string Key { get; set; }

    public string Value
    {
        get => _value;
        set
        {
            _value = Key switch
            {
                "Velocity" => value + " m3/u",
                "Capacity" => value + " m3",
                _ => value
            };
        }
    }
}

public class ThPbsCreateDto
{
    public float? Capacity { get; set; }
    public string Velocity { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string CpcTitle { get; set; }
    public int? TurnNumber { get; set; }
}