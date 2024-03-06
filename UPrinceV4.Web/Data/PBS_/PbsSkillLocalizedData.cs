using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.PBS_;

public class PbsSkillLocalizedData
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string LanguageCode { get; set; }

    [ForeignKey("PbsSkill")] public string PbsSkillId { get; set; }
    public PbsSkill PbsSkill { get; set; }
}