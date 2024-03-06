using System;

namespace UPrinceV4.Web.Data.VisualPlan;

public class Organization
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string SequenceId { get; set; }
    public string Title { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; } = null;
    public string ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; } = null;
    public string OrganizationTaxonomyId { get; set; }
}

public class OrganizationCreate
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string OrganizationTaxonomyId { get; set; }
}

public class GetOrganization
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string SequenceId { get; set; }
    public string Title { get; set; }
    public string OrganizationTaxonomyId { get; set; }
    public OrganizationHistory History { get; set; }
}

public class OrganizationHistory
{
    public string CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; } = null;
    public string ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; } = null;
}