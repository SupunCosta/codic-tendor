namespace UPrinceV4.Web.Data.PBS_;

public class PbsProductStatus
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LocaleCode { get; set; }
    public int DisplayOrder { get; set; }
}

public class PbsProductStatusDto
{
    public string Id { get; set; }
    public string Name { get; set; }
}