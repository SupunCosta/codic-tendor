using System;

namespace UPrinceV4.Web.Data.PO;

public class POToolPool
{
    public string Id { get; set; }

    public string POId { get; set; }

    //public string Title { get; set; }
    public string WareHouseTaxonomyId { get; set; }
    public string RequestedCPCId { get; set; }
    public string ResourceTypeId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string AssignedCPCId { get; set; }
    public string Project { get; set; }
}

public class GetPOToolPool
{
    public string Id { get; set; }

    public string POId { get; set; }

    //public string Title { get; set; }
    public string WareHouseTaxonomyId { get; set; }
    public string RequestedCPCId { get; set; }
    public string ResourceTypeId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string AssignedCPCId { get; set; }
    public string AssignedCPCTitle { get; set; }
    public string RequestedCPCTitle { get; set; }
}