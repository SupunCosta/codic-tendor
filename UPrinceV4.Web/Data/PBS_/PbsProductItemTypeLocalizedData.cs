namespace UPrinceV4.Web.Data.PBS_;

public class PbsProductItemTypeLocalizedData
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string LanguageCode { get; set; }

    //[ForeignKey("PbsProductItemType")]
    public string PbsProductItemTypeId { get; set; }
    //public PbsProductItemType PbsProductItemType { get; set; }
}

public class GetPbsProductItemTypeLocalizedDataByCode
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string LanguageCode { get; set; }
    public string PbsProductItemTypeId { get; set; }
    public string Name { get; set; }
}

public class UpdatePbsProductItemTypeLocalizedDataDto
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string LanguageCode { get; set; }
}

public class AddPbsProductItemTypeLocalizedDataDto
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string LanguageCode { get; set; }
    public string PbsProductItemTypeId { get; set; }
    public string Name { get; set; }
}