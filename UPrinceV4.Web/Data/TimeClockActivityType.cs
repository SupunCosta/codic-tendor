namespace UPrinceV4.Web.Data;

public class TimeClockActivityType
{
    public int Id { get; set; }
    public string Type { get; set; }
    public int TypeId { get; set; }
    public string LocaleCode { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsDeleted { get; set; }
}

public class TimeClockActivityTypeDto
{
    public string Id { get; set; }
    public string Type { get; set; }
    public string TypeId { get; set; }
}