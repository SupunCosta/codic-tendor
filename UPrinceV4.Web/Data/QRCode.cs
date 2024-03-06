using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UPrinceV4.Web.Data.CPC;

namespace UPrinceV4.Web.Data;

public class QRCode
{
    [Key] public string Id { get; set; }

    // [ForeignKey("ProjectDefinition")]
    public string ProjectId { get; set; }
    [NotMapped] public virtual ProjectDefinition ProjectDefinition { get; set; }
    public DateTime Date { get; set; }

    [ForeignKey("CorporateProductCatalog")]
    public string VehicleNo { get; set; }

    public CorporateProductCatalog CorporateProductCatalog { get; set; }

    public string Location { get; set; }
    public string PersonalId { get; set; }
    public int Type { get; set; }
    public string CreatedByUserId { get; set; }
    public string TravellerType { get; set; }

    [ForeignKey("TimeClockActivityType")] public int ActivityTypeId { get; set; }
    public virtual TimeClockActivityType ActivityType { get; set; }

    public bool IsDeleted { get; set; }
    [NotMapped] public QRCodeHistoryDto History { get; set; }
}

public class CreateQRCodeDto
{
    public string Id { get; set; }
    public string ProjectId { get; set; }
    public DateTime Date { get; set; }
    public string VehicleNo { get; set; }
    public string Location { get; set; }
    public string PersonalId { get; set; }
    [Required] public int Type { get; set; }
    public string TravellerType { get; set; }
}

public class UpdateQRCodeDto
{
    [Required] public string Id { get; set; }
    public string ProjectId { get; set; }
    public DateTime Date { get; set; }
    public string VehicleNo { get; set; }
    public string Location { get; set; }
    public string PersonalId { get; set; }
    [Required] public int Type { get; set; }
    public string TravellerType { get; set; }
}

public class QRCodeHistoryDto
{
    public string CreatedByUser { get; set; }
    public DateTime? CreatedDateTime { get; set; }
    public string UpdatedByUser { get; set; }
    public DateTime? UpdatedDateTime { get; set; }
    public int RevisionNumber { get; set; }
}

public class QRCodeDto
{
    public string Id { get; set; }
    public string ProjectTitle { get; set; }
    public string VehicleNo { get; set; }
    public string Location { get; set; }
    public string Type { get; set; }
    public DateTime Date { get; set; }
    public string ProjectId { get; set; }
    public string TravellerType { get; set; }
}

public class QRCodeDapperDto
{
    public string Id { get; set; }
    public string ProjectId { get; set; }
    public DateTime Date { get; set; }
    public string VehicleCpcId { get; set; }
    public string VehicleNo { get; set; }
    public string TravellerType { get; set; }
    public string Location { get; set; }
    public string PersonalId { get; set; }
    public int Type { get; set; }
    public string CreatedByUserId { get; set; }
    public int ActivityTypeId { get; set; }
    public virtual TimeClockActivityTypeDto ActivityType { get; set; }
    public ProjectDefinitionHistoryLogDto History { get; set; }
    public ProjectDefinition ProjectDefinition { get; set; }
}