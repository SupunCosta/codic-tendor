using System;
using System.Collections.Generic;

namespace UPrinceV4.Web.Data.BurndownChart;

public class BurndownChartDto
{
    
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    
}

public class GetBurndownChart
{
    
        public string Title { get; set; }
        public bool IsPrimary { get; set; }
        public List<BurndownData> Data { get; set; }

}

public class BurndownData
{
    
        public string Name { get; set; }
        public float? Consumed { get; set; }
        public float? Planned { get; set; }

}

public class LabourData
{
    
        public string CoperateProductCatalogId { get; set; }
        public string ResourceFamilyId { get; set; }
        public DateTime? ExecutionDate { get; set; }

        public float? ConsumedQuantity { get; set; }
        
        public float? RequiredQuantity { get; set; }
        

        
}