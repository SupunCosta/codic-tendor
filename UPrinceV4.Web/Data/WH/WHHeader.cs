using System;
using UPrinceV4.Web.Data.CPC;
using UPrinceV4.Web.Data.ProjectLocationDetails;

namespace UPrinceV4.Web.Data.WH;

public class WHHeader : WHMetaData
{
    public string Id { get; set; }
    public string SequenceId { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public string StatusId { get; set; }
    public string TypeId { get; set; }
    public string Address { get; set; }
    public string OpeningHoursFrom { get; set; }
    public string OpeningHoursTo { get; set; }
    public string ManagerId { get; set; }
    public string Area { get; set; }
    public WHHistoryLog WHHistory { get; set; }
    public WHTaxonomy WHTaxonomy { get; set; }
    public string CPCIdVehicle { get; set; }
}

public class WHListDto
{
    public string Id { get; set; }
    public string SequenceId { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public string StatusId { get; set; }
    public string Status { get; set; }
    public string TypeId { get; set; }
    public string Type { get; set; }
    public string Address { get; set; }
    public string OpeningHoursFrom { get; set; }
    public string OpeningHoursTo { get; set; }
    public string ManagerId { get; set; }
    public string Area { get; set; }
}

public class WHCreateDto
{
    public string Id { get; set; }
    public string SequenceId { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public string StatusId { get; set; }
    public string TypeId { get; set; }
    public string LocationId { get; set; }
    public string OpeningHoursFrom { get; set; }
    public string OpeningHoursTo { get; set; }
    public string ManagerId { get; set; }
    public string Area { get; set; }

    public string CPCIdVehicle { get; set; }
}

public class WHHeaderDto
{
    public string Id { get; set; }
    public string SequenceId { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public string OpeningHoursFrom { get; set; }
    public string OpeningHoursTo { get; set; }
    public string ManagerId { get; set; }
    public string ManagerName { get; set; }
    public string Area { get; set; }
    public WHTypeDto Type { get; set; }
    public WHStatusDto Status { get; set; }
    public WHHistoryDto History { get; set; }
    public MapLocation MapLocation { get; set; }
    public string LocationId { get; set; }

    public CpcWareHousetDto VehicleNumber { get; set; }
}

public class WHHistoryDto
{
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public string CreatedBy { get; set; }
    public string ModifiedBy { get; set; }
}