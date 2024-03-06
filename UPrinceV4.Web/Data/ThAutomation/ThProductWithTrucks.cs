using System;
using System.Collections.Generic;
using UPrinceV4.Web.Data.PBS_;

namespace UPrinceV4.Web.Data.ThAutomation;

public class ThProductWithTrucks
{
    public string Id { get; set; }
    public string CpcId { get; set; }
    public string ProductId { get; set; }
    public DateTime Date { get; set; }
    public bool IsTruck { get; set; }

}

public class GetThProductWithTrucks
{
    public string Id { get; set; }
    public string ProductId { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public string Title { get; set; }
    public string ProjectSequenceCode { get; set; }
    public List<GetThProductWithTrucksDto> Trucks { get; set; }
    public List<PbsDynamicAttributesDto> PbsDynamicAttributes { get; set; }
    public int Capacity { get; set; }
    public bool IsTruck { get; set; }

}

public class GetThProductWithTrucksDto
{
    public string Id { get; set; }
    public string CpcId { get; set; }
    public string ProductId { get; set; }
    public string ProjectSequenceCode { get; set; }
    public DateTime Date { get; set; }
    public string Title { get; set; }
    public int? TruckOrder { get; set; }
    public string STime { get; set; }
    public string ETime { get; set; }
    public string Type { get; set; }
    public bool IsTruck { get; set; }

}

public class ThProductWithTrucksDto
{
    public DateTime Date { get; set; }
}

public class ThTruckWithProductDto
{
    public DateTime Date { get; set; }

}

public class TruckAssignDto
{
    public string Id { get; set; }
    public bool IsPmol { get; set; }
    public string ProjectSequenceCode { get; set; }
    public bool IsTruck { get; set; }

}

public class ThTruckWithProductData
{
    public string Id { get; set; }
    public string TruckTitle { get; set; }
    public string TruckCapacity { get; set; }
    public int SortingOrder { get; set; }

    public List<Product> Product { get; set; }
    public List<TimeSlots> TimeSlots { get; set; }
    
}

public class Product
{
    public string Id { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string ProductTitle { get; set; }
}

public class RemoveTruckFromDayDto
{
    public string Id { get; set; }
    public DateTime Date { get; set; }
}

public class CaculateDistance
{
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public double PreviousLongitude { get; set; }
    public double PreviousLatitude { get; set; }
}


public class ThResourceFamiliesDto
{
    public bool IsTruck { get; set; }
}

public class GetThResourceFamilies
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string ParentId { get; set; }
    public string CPCId { get; set; }

}
public class TimeSlots
{
    public string STime { get; set; }
    public string ETime { get; set; }
    public int SortingOrder { get; set; }

}