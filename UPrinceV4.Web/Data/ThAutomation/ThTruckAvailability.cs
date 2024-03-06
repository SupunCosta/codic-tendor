using System;

namespace UPrinceV4.Web.Data.ThAutomation
{
    public class ThTruckAvailability
    {
        public string Id { get; set; }
        public string StockId { get; set; }
        public string ActivityType { get; set; }
        public DateTime? Date { get; set; }
        public bool Availability { get; set; }
        public string ResourceFamilyId { get; set; }
        public string STime { get; set; }
        public string ETime { get; set; }
        public int SortingOrder { get; set; }


    }
    
    public class ThTruckAvailabilityDto
    {
        public string Id { get; set; }
        public string StockId { get; set; }
        public string ActivityType { get; set; }
        public DateTime? Date { get; set; }
        public bool Availability { get; set; }
        public string ResourceFamilyId { get; set; }
        public string STime { get; set; }
        public string ETime { get; set; }
        public int SortingOrder { get; set; }
        
    }
    
    public class ThStockCreate
    {
        public string CPCId { get; set; }
        
        
    }
    
    public class ThStockDelete
    {
        public string Id { get; set; }
        

        
    }
}
