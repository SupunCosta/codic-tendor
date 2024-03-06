using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.PBS_;

public class PbsExperienceLocalizedData
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string LanguageCode { get; set; }

    [ForeignKey("PbsExperience")] public string PbsExperienceId { get; set; }
    public PbsExperience PbsExperience { get; set; }
}

public class GetPbsExperienceLocalizedDataByCode
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string LanguageCode { get; set; }


    public string PbsExperienceId { get; set; }
    public string Name { get; set; }
}

public class UpdatePbsExperienceLocalizedDataDto
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string LanguageCode { get; set; }
}

public class AddPbsExperienceLocalizedDataDto
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string LanguageCode { get; set; }


    public string PbsExperienceId { get; set; }
    public string Name { get; set; }
}