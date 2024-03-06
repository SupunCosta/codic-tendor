using System;

namespace UPrinceV4.Web.Models;

public class ProjectCostFilter
{
    public string ProductTitle { get; set; }

    public string BorTitle { get; set; }

    // when sorting attribute should be Date not StrDate of EndDate
    public DateTime? StrDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string ResourceTypeId { get; set; }
    public string ResourceTitle { get; set; }
    public string productType { get; set; }

    //public string PmolTitle { get; set; }
    //public string ResourceNumber { get; set; }
    //public string ResourceTitle { get; set; }
    //public Nullable<double> Mou { get; set; }
    //public Nullable<double> ConsumedQuantity { get; set; }
    //public Nullable<double> CostMou { get; set; }
    //public Nullable<double> TotalCost { get; set; }
    public ProjectCostSortingModel ProjectCostSortingModel { get; set; }
}

public class ProjectCostSortingModel
{
    // ProductTitle, BorTitle, Date, ResourceType, PmolTitle, ResourceNumber, ResourceTitle
    // Mou, ConsumedQuantity, CostMou, TotalCost
    public string Attribute { get; set; }
    public string Order { get; set; }
}