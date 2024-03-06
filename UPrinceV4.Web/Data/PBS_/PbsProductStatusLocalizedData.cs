namespace UPrinceV4.Web.Data.PBS_;

public class PbsProductStatusLocalizedData
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string LanguageCode { get; set; }

    //[ForeignKey("PbsProductStatus")]
    public string PbsProductStatusId { get; set; }

    //public PbsProductStatus PbsProductStatus { get; set; }
    public int DisplayOrder { get; set; }
}

public class GetPbsProductStatusLocalizedDataByCode
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string LanguageCode { get; set; }
    public string PbsProductStatusId { get; set; }
    public string Name { get; set; }
}

public class UpdatePbsProductStatusLocalizedDataDto
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string LanguageCode { get; set; }
}

public class AddPbsProductStatusLocalizedDataDto
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string LanguageCode { get; set; }
    public string PbsProductStatusId { get; set; }
    public int DisplayOrder { get; set; }
    public string Name { get; set; }
}