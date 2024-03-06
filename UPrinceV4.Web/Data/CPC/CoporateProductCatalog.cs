using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.CPC;

public class CorporateProductCatalog
{
    public string Id { get; set; }
    public string ResourceTitle { get; set; }

    [ForeignKey("CpcResourceType")] public string ResourceTypeId { get; set; }
    public virtual CpcResourceType ResourceType { get; set; }

    [ForeignKey("CpcResourceFamily")] public string ResourceFamilyId { get; set; }
    public virtual CpcResourceFamily ResourceFamily { get; set; }

    [ForeignKey("CpcBasicUnitOfMeasure")] public string CpcBasicUnitOfMeasureId { get; set; }
    public virtual CpcBasicUnitOfMeasure CpcBasicUnitOfMeasure { get; set; }

    [ForeignKey("CpcMaterial")] public string CpcMaterialId { get; set; }
    public virtual CpcMaterial CpcMaterial { get; set; }

    [ForeignKey("CpcPressureClass")] public string CpcPressureClassId { get; set; }
    public virtual CpcPressureClass CpcPressureClass { get; set; }
    public double? InventoryPrice { get; set; }

    [ForeignKey("CpcUnitOfSizeMeasure")] public string CpcUnitOfSizeMeasureId { get; set; }
    public virtual CpcUnitOfSizeMeasure CpcUnitOfSizeMeasure { get; set; }

    [ForeignKey("CpcBrand")] public string CpcBrandId { get; set; }
    public virtual CpcBrand CpcBrand { get; set; }
    public double? Size { get; set; }
    public double? WallThickness { get; set; }
    public double? MinOrderQuantity { get; set; }
    public double? MaxOrderQuantity { get; set; }
    public double? Weight { get; set; }
    public int Status { get; set; }
    public IList<CpcImage> CpcImage { get; set; }
    public IList<CpcResourceNickname> CpcResourceNickname { get; set; }
    public IList<CpcVendor> CpcVendor { get; set; }
    [NotMapped] public ProjectDefinitionHistoryLogDto CpcHistoryLog { get; set; }
    public string ResourceNumber { get; set; }
    public bool IsDeleted { get; set; }

    public string Title { get; set; }

    //[NotMapped] public string Title => ResourceNumber + " - " + ResourceTitle;
    [NotMapped] public string HeaderTitle { get; set; }
    [NotMapped] public string Key => Id;
    [NotMapped] public string Text => Title;
}

public class CorporateProductCatalogDto
{
    public string Id { get; set; }
    public string ResourceTitle { get; set; }
    public virtual CpcResourceTypeDto ResourceType { get; set; }
    public virtual CpcResourceFamilyDtoDapper ResourceFamily { get; set; }
    public virtual CpcBasicUnitOfMeasureDto CpcBasicUnitOfMeasure { get; set; }
    public virtual CpcMaterialDto CpcMaterial { get; set; }
    public virtual CpcPressureClassDto CpcPressureClass { get; set; }
    public double? InventoryPrice { get; set; }
    public virtual CpcUnitOfSizeMeasureDto CpcUnitOfSizeMeasure { get; set; }
    public virtual CpcBrandDto CpcBrand { get; set; }
    public double? Size { get; set; }
    public double? WallThickness { get; set; }
    public double? MinOrderQuantity { get; set; }
    public double? MaxOrderQuantity { get; set; }
    public double? Weight { get; set; }
    public int Status { get; set; }
    public IList<CpcImageCreateDto> CpcImage { get; set; }
    public IList<CpcResourceNicknameCreateDto> CpcResourceNickname { get; set; }
    public IList<CpcVendor> CpcVendor { get; set; }
    public ProjectDefinitionHistoryLogDto CpcHistoryLog { get; set; }
    public string ResourceNumber { get; set; }
    public bool IsDeleted { get; set; }
    public string Title { get; set; }
    public string HeaderTitle { get; set; }
    public string Key => Id;
    public string Text => Title;
    public string CpcMaterialId { get; set; }
    public string CpcMaterialName { get; set; }
    public CpcVehicleTrackingNo GdVehicle { get; set; }
    public double? Velocity { get; set; }
    public string SerialNumber { get; set; }
    public string StockId { get; set; }

}

public class CoperateProductCatalogCreateDto
{
    public string Id { get; set; }
    [Required] public string ResourceTitle { get; set; }
    public string ResourceTypeId { get; set; }
    public string ResourceFamilyId { get; set; }
    public string CpcBasicUnitOfMeasureId { get; set; }
    public string CpcMaterialId { get; set; }
    public string CpcPressureClassId { get; set; }
    public string CpcBrandId { get; set; }
    public double? Size { get; set; }
    public string CpcUnitOfSizeMeasureId { get; set; }
    public double? WallThickness { get; set; }
    public double? InventoryPrice { get; set; }
    public double? MinOrderQuantity { get; set; }
    public double? MaxOrderQuantity { get; set; }
    public double? Weight { get; set; }
    public int Status { get; set; }
    public List<CpcImageCreateDto> CpcImage { get; set; }
    public List<CpcResourceNicknameCreateDto> CpcResourceNickname { get; set; }
    public List<CpcVendorCreateDto> CpcVendor { get; set; }
    public string ResourceNumber { get; set; }
    public string CpcId { get; set; }
    public string ResourceId { get; set; }
    public string TrackingNo { get; set; }
    public double? Velocity { get; set; }
    public string SerialNumber { get; set; }

}

public class CoperateProductCatalogFilterDto
{
    //ResourceTitle    Status    ResourceNumber    ResourceType    Title    HeaderTitle
    // T-stuk    1    Ar01-01543    Materials Ar01-01543 - T-stuk Ar01-01543 - T-stuk
    public string Id { get; set; }
    public string ResourceTitle { get; set; }
    public string Status { get; set; }
    public string ResourceNumber { get; set; }
    public string ResourceType { get; set; }
    public string Title { get; set; }
    public string HeaderTitle { get; set; }
    public string BasicUnitOfMeasure { get; set; }
    public string BasicUnitOfMeasureId { get; set; }
    public string Project { get; set; }
}

public class CPCFilterDto
{
    public string Id { get; set; }
    public string Title { get; set; }
}

public class PMOLCPCFilterDto
{
    public string Id { get; set; }
    public string Title { get; set; }
}

public class CoperateProductCatalogFilterNewDto
{
    //ResourceTitle    Status    ResourceNumber    ResourceType    Title    HeaderTitle
    // T-stuk    1    Ar01-01543    Materials Ar01-01543 - T-stuk Ar01-01543 - T-stuk
    public string Id { get; set; }
    public string ResourceTitle { get; set; }
    public string Status { get; set; }
    public string ResourceType { get; set; }
    public string Title { get; set; }
    public string ResourceNumber { get; set; }
    public string MouId { get; set; }

}

public class BorCoperateProductCatalogFilterDto
{
    public List<CoperateProductCatalogFilterDto> Local { get; set; }
    public List<CoperateProductCatalogFilterDto> ContractingUnit { get; set; }
    public List<CoperateProductCatalogFilterDto> Central { get; set; }
}

public class BorCPCFilterDto
{
    public List<CPCFilterDto> Local { get; set; }
    public List<CPCFilterDto> ContractingUnit { get; set; }
    public List<CPCFilterDto> Central { get; set; }
}

public class PMolCPCFilterDto
{
    public List<PMOLCPCFilterDto> Local { get; set; }
    public List<PMOLCPCFilterDto> ContractingUnit { get; set; }
    public List<PMOLCPCFilterDto> Central { get; set; }
}

public class PBSCPCFilterDto
{
    public List<CPCFilterDto> Local { get; set; }
    public List<CPCFilterDto> ContractingUnit { get; set; }
    public List<CPCFilterDto> Central { get; set; }
}

public class CpcForBorDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string ResourceNumber { get; set; }
    public string ResourceTypeId { get; set; }
    public string ResourceType { get; set; }
    public string BasicUnitOfMeasureId { get; set; }
    public string BasicUnitOfMeasure { get; set; }
}

public class CpcForProductDto
{
    public string Key { get; set; }
    public string Text { get; set; }
}

public class CpcWareHousetDto
{
    public string Value { get; set; }
    public string Label { get; set; }
}

public class CpcDetails
{
    public string Id { get; set; }
    public string ResourceTitle { get; set; }
    public string ResourceType { get; set; }
    public string BasicUnitOfMeasure { get; set; }
    public string ResourceNumber { get; set; }
    public string BasicUnitOfMeasureId { get; set; }
}

public class CpcManagerDto
{
    public string Project { get; set; }
    public string CabPerson { get; set; }
    public List<CpcListDto> CPC { get; set; }
}

public class CpcListDto
{
    public string ResourceTitle { get; set; }
    public string ResourceNumber { get; set; }
    public string ResourceName { get; set; }
}

public class ProjectWithPM
{
    public string ProjectId { get; set; }

    public string ProjectTitle { get; set; }

    public string ContractingUnitId { get; set; }
    public string CabPersonId { get; set; }
    public string SequenceCode { get; set; }
    public string Name { get; set; }
    public string FullName { get; set; }

    public string ProjectConnectionString { get; set; }

    public List<CpcListDto> CPC { get; set; }
}

public class Databases
{
    public string DatabaseName { get; set; }
}

public class DatabasesEx
{
    public string DatabaseName { get; set; }

    public Exception Exception { get; set; }
}

public class DropDownDto
{
}

public class PbsResourcesEnvDto
{
    public string Id { get; set; }
    public string PbsProductId { get; set; }
    public string CoperateProductCatalogId { get; set; }
    public string Quantity { get; set; }
    public string ProjectSequenceCode { get; set; }
    public string PbsTitle { get; set; }
    public string CPCTitle { get; set; }
    public string Environment { get; set; }
}

public class AddCpcTruckDto
{
    public string Name { get; set; }
    public double? Size { get; set; }
    public string SerialNumber { get; set; }
    public string ResourceFamilyId { get; set; }

}

