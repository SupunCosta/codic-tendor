using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace UPrinceV4.Web.Data.PBS_;

public class PbsSkillExperience
{
    public string Id { get; set; }

    [ForeignKey("PbsProduct")]
    [JsonIgnore]
    public string PbsProductId { get; set; }

    public virtual PbsProduct PbsProduct { get; set; }

    [ForeignKey("PbsSkill")] [JsonIgnore] public string PbsSkillId { get; set; }
    public virtual PbsSkill PbsSkill { get; set; }

    [ForeignKey("PbsExperience")]
    [JsonIgnore]
    public string PbsExperienceId { get; set; }

    public virtual PbsExperience PbsExperience { get; set; }
}

public class CompetenciesDto
{
    public string Id { get; set; }
    public string PbsProductId { get; set; }
    public string SkillId { get; set; }
    public string ExperienceId { get; set; }
}

public class PbsSkillExperienceDto
{
    public string PbsProductId { get; set; }
    public string Id { get; set; }
    public string SkillId { get; set; }
    public string Skill { get; set; }
    public string ExperienceId { get; set; }
    public string Experience { get; set; }
}