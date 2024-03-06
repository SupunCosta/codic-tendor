using System;
using UPrinceV4.Web.Data.VisualPlan;

namespace UPrinceV4.Web.Data.HR;

public class VpHR
{
    public string Id { get; set; }
    public string PoId { get; set; }
    public string ProjectSequenceCode { get; set; }
    public string PersonId { get; set; }
    public string CPCId { get; set; }
    public string ParentId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class VpOrganizationTaxonomyList
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string PersonId { get; set; }
    public string PersonName { get; set; }
    public string BuId { get; set; }
    public string OrganizationId { get; set; }
    public string ParentId { get; set; }
    public string OrganizationTaxonomyLevelId { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsChildren { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string PoId { get; set; }
    public string ProjectSequenceCode { get; set; }
    public string CPCId { get; set; }
    public GetOrganization Organization { get; set; }
    public double Percentage { get; set; }
    public string ProjectTitle { get; set; }
}

public class GetVpHR
{
    public string Id { get; set; }
    public string PoId { get; set; }
    public string ProjectSequenceCode { get; set; }
    public string PersonId { get; set; }
    public string CPCId { get; set; }
    public string ParentId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Title { get; set; }
    public string Type { get; set; }
}