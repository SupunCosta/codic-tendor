namespace UPrinceV4.Web.Data.PBS_;

public class PbsToleranceStateLocalizedData
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string LanguageCode { get; set; }

    //[ForeignKey("PbsToleranceState")]
    public string PbsToleranceStateId { get; set; }
    //public PbsToleranceState PbsTaxonomyLevel { get; set; }
}

public class GetPbsToleranceStateLocalizedDataByCode
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string LanguageCode { get; set; }
    public string PbsToleranceStateId { get; set; }
    public string Name { get; set; }
}

public class UpdatePbsToleranceStateLocalizedDataDto
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string LanguageCode { get; set; }
}

public class AddPbsToleranceStateLocalizedDataDto
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string LanguageCode { get; set; }
    public string PbsToleranceStateId { get; set; }
    public string Name { get; set; }
}