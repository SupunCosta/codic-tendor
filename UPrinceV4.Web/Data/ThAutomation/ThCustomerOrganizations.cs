using System;

namespace UPrinceV4.Web.Data.ThAutomation;

public class ThCustomerOrganizations
{
    public string Id { get; set; }
    public string CustomerId { get; set; }
    public string OrganizationId { get; set; }
    public string ProjectId { get; set; }
    public string PoId { get; set; }
}

public class CreateThProjectDto
{
    public string CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string OrganizationId { get; set; }
    public string OrganizationName { get; set; }
    public string ProjectName { get; set; }
    public string Latitude { get; set; }
    public string Longitude { get; set; }
    public string Capacity { get; set; }
    public string Velocity { get; set; }
    public DateTime? StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }
    public string PoId { get; set; }
}

public class UpdateThProjectDto
{
    public string ProductId { get; set; }
    public string ProjectSequenceCode { get; set; }
    public string Latitude { get; set; }
    public string Longitude { get; set; }
    public string Capacity { get; set; }
    public string Velocity { get; set; }
    public DateTime? StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }
}

public class DeleteThProjectDto
{
    public string ProductId { get; set; }
    public string ProjectSequenceCode { get; set; }
    
}