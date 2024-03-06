using System;
using System.Collections.Generic;
using UPrinceV4.Web.Data.VisualPlan;

namespace UPrinceV4.Web.Data.VisualPlaane;

public class OrganizationCertification
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string SequenceId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime? QualificationDate { get; set; } = null;
    public string QualificationOrganization { get; set; }
    public DateTime? StartDate { get; set; } = null;
    public DateTime? EndDate { get; set; } = null;
    public string CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; } = null;
    public string ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; } = null;
    public string QualificationTypeId { get; set; }
}

public class OrganizationCertificationCreate
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime? QualificationDate { get; set; } = null;
    public string QualificationOrganization { get; set; }
    public DateTime? StartDate { get; set; } = null;
    public DateTime? EndDate { get; set; } = null;
    public string QualificationTypeId { get; set; }
}

public class OrganizationCertificationHistory
{
    public string CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; } = null;
    public string ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; } = null;
}

public class GetOrganizationCertification
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string SequenceId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime? QualificationDate { get; set; } = null;
    public string QualificationOrganization { get; set; }
    public QualificationOrganization Organization { get; set; }
    public DateTime? StartDate { get; set; } = null;
    public DateTime? EndDate { get; set; } = null;
    public OrganizationCertificationHistory History { get; set; }
    public string QualificationTypeId { get; set; }
}

public class OrganizationCertificationDownData
{
    public IEnumerable<CertificationTaxonomyLevel> CertificationTaxonomyLevel { get; set; }
}

public class QualificationOrganization
{
    public string Key { get; set; }
    public string Text { get; set; }
}

public class OrganizationDownData
{
    public IEnumerable<OrganizationTeamRoleDto> OrganizationTeamRole { get; set; }
}