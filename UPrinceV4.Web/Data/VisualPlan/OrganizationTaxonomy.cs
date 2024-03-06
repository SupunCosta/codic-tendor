using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.VisualPlan;

public class OrganizationTaxonomy
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string OrganizationId { get; set; }
    public string ParentId { get; set; }
    public string OrganizationTaxonomyLevelId { get; set; }
    public string PersonId { get; set; }
    public DateTime? StartDate { get; set; } = null;
    public DateTime? EndDate { get; set; } = null;
    public string RoleId { get; set; }
    public DateTime ModifiedDate { get; set; }
    public DateTime ExpiryDate { get; set; }

    [NotMapped] public string ComId { get; set; }

    public bool IsDefaultBu { get; set; } = false;
    public string BuName { get; set; }
    public string BuSequenceId { get; set; }
    public string Project { get; set; }
    public string TemporaryTeamNameId { get; set; }
}

public class OrganizationTaxonomyList
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string PersonId { get; set; }
    public string PersonName { get; set; }
    public string OrganizationId { get; set; }
    public string ParentId { get; set; }
    public string OrganizationTaxonomyLevelId { get; set; }
    public int DisplayOrder { get; set; }
    public string RoleId { get; set; }
    public bool IsChildren { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public GetOrganization Organization { get; set; }
    public string BuId { get; set; }
    public bool IsDefaultBu { get; set; }
    public string BuName { get; set; }
    public string BuSequenceId { get; set; }
    public string CabPersonId { get; set; }
}

public class OrganizationTaxonomyListForProjectPlan
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string CabPersonCompanyId { get; set; }
    public string PersonName { get; set; }
    public string OrganizationId { get; set; }
    public string ParentId { get; set; }
    public string OrganizationTaxonomyLevelId { get; set; }
    public int DisplayOrder { get; set; }
    public string RoleId { get; set; }
    public bool IsChildren { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public GetOrganization Organization { get; set; }
    public string BuId { get; set; }
    public bool IsDefaultBu { get; set; }
    public string BuName { get; set; }
    public string BuSequenceId { get; set; }
    public string CabPersonId { get; set; }
}

public class OrganizationTaxonomyFilter
{
    public string OrganizationTaxonomyLevelId { get; set; }
    public string PersonId { get; set; }
}

public class OrganizationTaxonomyDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string OrganizationId { get; set; }
    public string ParentId { get; set; }
    public string OrganizationTaxonomyLevelId { get; set; }
    public string PersonId { get; set; }
    public DateTime? StartDate { get; set; } = null;
    public DateTime? EndDate { get; set; } = null;
    public string RoleId { get; set; }
    public string Cu { get; set; }
    public string BuName { get; set; }
    public string BuSequenceId { get; set; }
    public string PersonName { get; set; }
}

public class OrganizationTaxonomyPerson
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string ParentId { get; set; }
    public string PersonId { get; set; }
    public string OrganizationTaxonomyLevelId { get; set; }
}

public class OrganizationTaxonomyBu
{
    public string Id { get; set; }
}

public class CabPersonOrganizationTaxonomy
{
    public string Id { get; set; }
    public string FullName { get; set; }
}