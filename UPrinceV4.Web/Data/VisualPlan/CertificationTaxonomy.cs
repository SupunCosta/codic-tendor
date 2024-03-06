namespace UPrinceV4.Web.Data.VisualPlaane;

public class CertificationTaxonomy
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string CetificationId { get; set; }
    public string ParentId { get; set; }
    public string CertificationTaxonomyLevelId { get; set; }
    public string ParentCertificationId { get; set; }
}

public class CertificationTaxonomyList
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string CetificationId { get; set; }
    public string ParentId { get; set; }
    public string CertificationTaxonomyLevelId { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsChildren { get; set; }
    public string ParentCertificationId { get; set; }
    public GetOrganizationCertification Certification { get; set; }
}

public class CertificationTaxonomyFilter
{
    public string CertificationId { get; set; }
}