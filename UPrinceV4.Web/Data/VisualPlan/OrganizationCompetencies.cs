using System;
using System.Collections.Generic;
using UPrinceV4.Web.Data.PBS_;

namespace UPrinceV4.Web.Data.VisualPlaane;

public class OrganizationCompetencies
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string SequenceId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ExperienceLevelId { get; set; }
    public string Occupation { get; set; }
    public string Qualification { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; } = null;
    public string ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; } = null;
    public string SkillTypeId { get; set; }
}

public class OrganizationCompetenciesCreate
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ExperienceLevelId { get; set; }
    public string Occupation { get; set; }
    public string Qualification { get; set; }
    public string SkillTypeId { get; set; }
}

public class OrganizationCompetenciesHistory
{
    public string CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; } = null;
    public string ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; } = null;
}

public class GetOrganizationCompetencies
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string SequenceId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ExperienceLevelId { get; set; }
    public string ExperienceLevelName { get; set; }
    public string Occupation { get; set; }
    public string Qualification { get; set; }
    public OrganizationCompetenciesHistory History { get; set; }
    public string SkillTypeId { get; set; }
}

public class OrganizationCompetenciesDownData
{
    public IEnumerable<CompetenciesTaxonomyLevel> CompetenciesTaxonomyLevel { get; set; }
    public IEnumerable<PbsExperienceDto> Experience { get; set; }
}