namespace UPrinceV4.Web.Data.VisualPlaane;

public class CompetenciesTaxonomy
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string CompetenciesId { get; set; }
    public string ParentId { get; set; }
    public string CompetenciesTaxonomyLevelId { get; set; }
    public string ParentCompetenciesId { get; set; }
}

public class CompetenciesTaxonomyList
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string CompetenciesId { get; set; }
    public string ParentId { get; set; }
    public string CompetenciesTaxonomyLevelId { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsChildren { get; set; }
    public string ParentCompetenciesId { get; set; }
    public GetOrganizationCompetencies Competence { get; set; }
}

public class CompetenciesTaxonomyFilter
{
    public string CompetenciesId { get; set; }
    public string CertificationId { get; set; }
    public string OrganizationId { get; set; }
}