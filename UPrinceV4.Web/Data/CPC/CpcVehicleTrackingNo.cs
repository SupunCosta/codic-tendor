using System;
using System.Collections.Generic;
using UPrinceV4.Web.Data.GD.Vehicle;
using UPrinceV4.Web.Data.ProjectLocationDetails;

namespace UPrinceV4.Web.Data.CPC;

public class CpcVehicleTrackingNo
{
    public string Id { get; set; }
    public string CpcId { get; set; }
    public string ResourceId { get; set; }
    public string TrackingNo { get; set; }
}

public class PmolCpcData
{
    public string Id { get; set; }
    public string ProjectMoleculeId { get; set; }
    public string Name { get; set; }
    public DateTime ExecutionDate { get; set; }
    public string ExecutionStartTime { get; set; }
    public string ExecutionEndTime { get; set; }
    public string ProjectSequenceCode { get; set; }
    public string Title { get; set; }
    public List<PmolVehicle> PmolVehical { get; set; }
    public string ProjectTitle { get; set; }
    public string TeamId { get; set; }
}

public class PmolVehicle
{
    public string CoperateProductCatalogId { get; set; }
    public string Title { get; set; }
    public string ResourceNumber { get; set; }
    public string PmolId { get; set; }
    public string ResourceId { get; set; }
    public string TrackingNo { get; set; }
    public List<VtsMapData> Positions { get; set; }
    public Bar[] VehicleStatus { get; set; }
}

public class GetLabourPmolVehicalesPositionsDto
{
    public string PersonId { get; set; }
    public DateTime ExecutionDate { get; set; }
}

public class VtsData
{
    public string CoperateProductCatalogId { get; set; }
    public string Title { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime StartTime { get; set; }
    public Position StartPoint { get; set; }
    public Position Destination { get; set; }
    public long Speed1 { get; set; }
    public long Speed2 { get; set; }
}

public class VtsMapData
{
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string City { get; set; }
}