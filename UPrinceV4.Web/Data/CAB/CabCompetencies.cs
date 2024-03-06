using System;

namespace UPrinceV4.Web.Data.CAB;

public class CabCompetencies
{
    public string Id { get; set; }
    public string PersonId { get; set; }
    public DateTime? Date { get; set; } = null;
    public string CompetenciesTaxonomyId { get; set; }
    public string ExperienceLevelId { get; set; }
}

public class GetCabCompetencies
{
    public string Id { get; set; }
    public string PersonId { get; set; }
    public DateTime? Date { get; set; } = null;
    public string CompetenciesTaxonomyId { get; set; }
    public string ExperienceLevelId { get; set; }
    public string ExperienceLevelName { get; set; }
    public string Title { get; set; }
}