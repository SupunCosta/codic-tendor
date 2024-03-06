using System;

namespace UPrinceV4.Web.Data.CAB;

public class CabCertification
{
    public string Id { get; set; }
    public string PersonId { get; set; }
    public DateTime? StartDate { get; set; } = null;
    public DateTime? EndDate { get; set; } = null;

    public string CertificationTaxonomyId { get; set; }
    public string CertificationTitle { get; set; }
    public string CertificationUrl { get; set; }
}

public class GetCabCertification
{
    public string Id { get; set; }
    public string PersonId { get; set; }
    public DateTime? StartDate { get; set; } = null;
    public DateTime? EndDate { get; set; } = null;

    public string CertificationTaxonomyId { get; set; }
    public string Title { get; set; }
    public string CertificationTitle { get; set; }
    public string CertificationUrl { get; set; }
    public bool Validity { get; set; } = true;
}

public class GetCabCertificationDto
{
    public string PersonId { get; set; }
    public DateTime? Date { get; set; }
}