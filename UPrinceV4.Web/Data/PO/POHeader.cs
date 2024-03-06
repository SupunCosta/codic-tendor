using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UPrinceV4.Web.Data.CAB;
using UPrinceV4.Web.Data.ProjectLocationDetails;
using UPrinceV4.Web.Data.PS;

namespace UPrinceV4.Web.Data.PO;

public class POHeader : POMetaData

{
    [Key] public string Id { get; set; }
    public string SequenceId { get; set; }
    public string Title { get; set; }
    public string Name { get; set; }
    public string POTypeId { get; set; }
    public string POStatusId { get; set; }
    public string Comments { get; set; }
    public List<PODocument> Document { get; set; }
    public string ProjectSequenceCode { get; set; }
    public bool IsDeleted { get; set; }
    public string CustomerId { get; set; }
    public string CustomerReference { get; set; }
    public string SupplierCabPersonCompanyId { get; set; }
    public string SupplierReference { get; set; }
    public string CustomerCompanyId { get; set; }
    public string SuplierCompanyId { get; set; }
    public string ApprovedBy { get; set; }
    public string ProjectTitle { get; set; }
    public string NoOfMaterials { get; set; }
    public string NoOfTools { get; set; }
    public string NoOfConsumables { get; set; }
    public string NoOfLabours { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string LocationId { get; set; }
    public double TotalAmount { get; set; }
    public bool IsClone { get; set; } = false;
    public bool IsCu { get; set; } = false;
    public string DeliveryRequest { get; set; }
    public string TaxonomyId { get; set; }
    public string PORequestType { get; set; }
    public DateTime? RequestedDate { get; set; }
    public DateTime? ExpectedDate { get; set; }
    public string RequestedProbability { get; set; }
    public string AvailableProbability { get; set; }
}

public class POListDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public DateTime? ExecutionDate { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }
    public string TypeId { get; set; }
    public string StatusId { get; set; }

    public double TotalAmount { get; set; }

    public string SequenceId { get; set; }

    public string CustomerCompanyId { get; set; }
    public string SuplierCompanyId { get; set; }

    public string Customer { get; set; }
    public string Supplier { get; set; }

    public string LastModifiedDate { get; set; }

    public string ProjectTitle { get; set; }
    public string NoOfMaterials { get; set; }
    public string NoOfTools { get; set; }
    public string NoOfConsumables { get; set; }
    public string NoOfLabours { get; set; }

    public string ProjectSequenceId { get; set; }

    public string ModifiedBy { get; set; }

    public string ProjectSequenceCode { get; set; }
    public string RequestType { get; set; }
    public string RequestTypeId { get; set; }
}

public class PODto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string BorStatusId { get; set; }
}

public class POCreateDto
{
    public string Id { get; set; }
    public string SequenceId { get; set; }
    public string Title { get; set; }
    public string Name { get; set; }
    public string POTypeId { get; set; }
    public string POStatusId { get; set; }
    public string Comments { get; set; }
    public string ProjectSequenceCode { get; set; }
    public string IsDeleted { get; set; }
    public string CustomerOrganisationId { get; set; }
    public string CustomerContactId { get; set; }
    public string CustomerReference { get; set; }
    public string SupplierOrganisationId { get; set; }
    public string SupplierContactId { get; set; }
    public string SupplierReference { get; set; }
    public POResourcesDto POResources { get; set; }
    public List<POProduct> ExternalProduct { get; set; }
    public List<string> files { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string LocationId { get; set; }

    public double? TotalAmount { get; set; }

    public bool IsClone { get; set; } = false;

    public bool IsCu { get; set; } = false;

    public string DeliveryLocation { get; set; }
    public string WarehouseTaxonomyId { get; set; }
    public bool IsDeliveryPlan { get; set; } = false;
    public string PoRequestTypeId { get; set; }
    public bool IsPo { get; set; } = false;
    public DateTime? RequestedDate { get; set; }
    public DateTime? ExpectedDate { get; set; }
    public string RequestedProbability { get; set; }
    public string AvailableProbability { get; set; }
}

public class POHeaderDto
{
    public string Id { get; set; }
    public string SequenceId { get; set; }
    public string Title { get; set; }
    public string Name { get; set; }
    public string Comments { get; set; }
    public string CustomerOrganisationId { get; set; }
    public string CustomerContactId { get; set; }
    public string SupplierOrganisationId { get; set; }
    public string SupplierContactId { get; set; }
    public string CustomerOrganisation { get; set; }
    public string SupplierOrganisation { get; set; }
    public string CustomerReference { get; set; }
    public string SupplierReference { get; set; }
    public POTypeDto Type { get; set; }
    public PORequestTypeDto RequestType { get; set; }
    public POStatusDto Status { get; set; }
    public POHistoryDto History { get; set; }


    public POInvolvePartiesDto CustomerContact { get; set; }

    public POInvolvePartiesDto SupplierContact { get; set; }

    public POResourcesDto POResources { get; set; }

    public string ProjectSequenceCode { get; set; }

    public List<string> Files { get; set; }

    public List<POProduct> ExternalProduct { get; set; }
    public MapLocation MapLocation { get; set; }

    public string LocationId { get; set; }

    public double TotalAmount { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public bool IsCu { get; set; } = false;
    public string DeliveryLocation { get; set; }
    public string WarehouseTaxonomyId { get; set; }
    public DateTime? RequestedDate { get; set; }
    public DateTime? ExpectedDate { get; set; }
    public string RequestedProbability { get; set; }
    public string AvailableProbability { get; set; }
    public IEnumerable<GetPOLabourTeam> Teams { get; set; }
    public IEnumerable<POToolPool> ToolsPool { get; set; }
    public string MaterialCount { get; set; } = "0";
    public string ConsumableCount { get; set; } = "0";
    public string LabourCount { get; set; } = "0";
    public string ToolsCount { get; set; } = "0";
    public bool IsLinkExpired { get; set; }
    public string ResourceLeadTime { get; set; }
    public string UnitPrice { get; set; }
    public string CustomerJobRole { get; set; }
    public string SupplierCity { get; set; }
}

public class POHistoryDto
{
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public string CreatedBy { get; set; }
    public string ModifiedBy { get; set; }
}

public class POInvolvePartiesDto
{
    public string CustomerEmail { get; set; }
    public string CustomerContact { get; set; }
    public string CustomerMobile { get; set; }
    public string VatId { get; set; }
    public string Vat { get; set; }

    public CabAddress CabAddress { get; set; }
}

public class POResourcesDto
{
    public List<POResourcesAddDto> materials { get; set; }
    public List<POResourcesAddDto> tools { get; set; }
    public List<POResourcesAddDto> consumable { get; set; }
    public List<POResourcesAddDto> labour { get; set; }
}

public class POBORResources
{
    public string BorTitle { get; set; }
    public string BorId { get; set; }

    public List<POResourcesAddDto> resources { get; set; }
}

public class POResourcesAddDto
{
    public string Id { get; set; }

    public string PurchesOrderId { get; set; }

    //public string Title { get; set; }
    public string ResourceNumber { get; set; }
    public DateTime? Date { get; set; }
    public string PQuantity { get; set; }
    public string PPurchased { get; set; }
    public string PDeliveryRequested { get; set; }
    public string warf { get; set; }
    public string PConsumed { get; set; }
    public string PInvoiced { get; set; }
    public string CorporateProductCatalogId { get; set; }
    public string BorTitle { get; set; }

    public string BorId { get; set; }

    //public string Mou { get; set; }
    public List<string> PDocuments { get; set; }
    public List<string> CDocuments { get; set; }
    public string PComments { get; set; }
    public string PUnitPrice { get; set; }
    public string PTotalPrice { get; set; }
    public string PCrossReference { get; set; }
    public string CComments { get; set; }
    public string CUnitPrice { get; set; }
    public string CTotalPrice { get; set; }
    public string CCrossReference { get; set; }
    public string CQuantity { get; set; }
    public string CPurchased { get; set; }
    public bool CDeliveryRequested { get; set; }
    public DateTime? CStartDate { get; set; }
    public DateTime? CStopDate { get; set; }
    public string CNumberOfDate { get; set; }
    public string CFullTimeEmployee { get; set; }
    public DateTime? PStartDate { get; set; }
    public DateTime? PStopDate { get; set; }
    public string PNumberOfDate { get; set; }
    public string PFullTimeEmployee { get; set; }
    public string Pdevices { get; set; }
    public string Cdevices { get; set; }
    public string ProjectTitle { get; set; }
    public string NoOfMaterials { get; set; }
    public string NoOfTools { get; set; }
    public string NoOfConsumables { get; set; }
    public string NoOfLabours { get; set; }
    public string PbsTitle { get; set; }

    public string CTitle { get; set; }

    public string PTitle { get; set; }

    public string CMou { get; set; }

    public string PMou { get; set; }

    public string CCPCId { get; set; }
    public string PCPCId { get; set; }

    public POHeaderDto POHeaderDto { get; set; }

    public bool? IsStock { get; set; }

    public string ResourceFamily { get; set; }

    public bool HasChanged { get; set; }
    public string ResourcesType { get; set; }


    public string ResourceTypeId { get; set; }

    public string BorResourceId { get; set; }
    public string StockAvailability { get; set; }

    public bool? IsUsed { get; set; } = false;
    public string UsedPoId { get; set; }
    public DateTime? RequestedDeliveryDate { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }
    public string ResourcesTypeName { get; set; }
    public string WFStatus { get; set; }
    public string WorkFlowId { get; set; }
    public string PbsProductId { get; set; }
}

public class POApproveDto
{
    public string POSequenceId { get; set; }
}

public class POResourcesExcelDto
{
    public string Id { get; set; }

    public string PurchesOrderId { get; set; }

    //public string Title { get; set; }
    public string ResourceNumber { get; set; }
    public DateTime? Date { get; set; }
    public string PQuantity { get; set; }
    public string PPurchased { get; set; }
    public string PDeliveryRequested { get; set; }
    public string warf { get; set; }
    public string PConsumed { get; set; }
    public string PInvoiced { get; set; }
    public string CorporateProductCatalogId { get; set; }
    public string BorTitle { get; set; }

    public string BorId { get; set; }

    //public string Mou { get; set; }
    public List<string> PDocuments { get; set; }
    public List<string> CDocuments { get; set; }
    public string PComments { get; set; }
    public string PUnitPrice { get; set; }
    public string PTotalPrice { get; set; }
    public string PCrossReference { get; set; }
    public string CComments { get; set; }
    public string CUnitPrice { get; set; }
    public string CTotalPrice { get; set; }
    public string CCrossReference { get; set; }
    public string CQuantity { get; set; }
    public string CPurchased { get; set; }
    public bool CDeliveryRequested { get; set; }
    public DateTime? CStartDate { get; set; }
    public DateTime? CStopDate { get; set; }
    public string CNumberOfDate { get; set; }
    public string CFullTimeEmployee { get; set; }
    public DateTime? PStartDate { get; set; }
    public DateTime? PStopDate { get; set; }
    public string PNumberOfDate { get; set; }
    public string PFullTimeEmployee { get; set; }
    public string Pdevices { get; set; }
    public string Cdevices { get; set; }
    public string ProjectTitle { get; set; }
    public string NoOfMaterials { get; set; }
    public string NoOfTools { get; set; }
    public string NoOfConsumables { get; set; }
    public string NoOfLabours { get; set; }
    public string PbsTitle { get; set; }

    public string CTitle { get; set; }
    public string PTitle { get; set; }

    public string ProjectCPCTitle { get; set; }

    public string CMou { get; set; }

    public string PMou { get; set; }

    public string CCPCId { get; set; }
    public string PCPCId { get; set; }

    public POHeaderDto POHeaderDto { get; set; }

    public bool? IsStock { get; set; }

    public string ResourceType { get; set; }

    public PsCustomerReadDto Customer { get; set; }

    public string ResourceFamily { get; set; }

    public bool? IsUsed { get; set; }
    public string ResourceTypeId { get; set; }

    public string ResourceName { get; set; }
}

public class POResourceForWorkFlow
{
    public string BorId { get; set; }
    public string BorTitle { get; set; }
    public string ResourcesType { get; set; }
    public string CComments { get; set; }
    public string CQuantity { get; set; }
    public string CTotalPrice { get; set; }
    public string CCPCId { get; set; }
    public string CMou { get; set; }
    public bool IsStock { get; set; }
    public string ProjectSequenceCode { get; set; }
    public string POName { get; set; }
    public string DeliveryRequest { get; set; }
    public string PoId { get; set; }
    public string CUnitPrice { get; set; }
    public string ExpectedDeliveryDate { get; set; }
    public string POResourcesId { get; set; }
    public string POTitle { get; set; }
}

public class POHeaderExcelDto
{
    public string Id { get; set; }
    public string SequenceId { get; set; }
    public string Title { get; set; }
    public string Name { get; set; }
    public string Comments { get; set; }
    public string CustomerOrganisationId { get; set; }
    public string CustomerContactId { get; set; }
    public string SupplierOrganisationId { get; set; }
    public string SupplierContactId { get; set; }
    public string CustomerOrganisation { get; set; }
    public string SupplierOrganisation { get; set; }
    public string CustomerReference { get; set; }
    public string SupplierReference { get; set; }
    public POTypeDto Type { get; set; }
    public POStatusDto Status { get; set; }
    public POHistoryDto History { get; set; }


    public POInvolvePartiesDto CustomerContact { get; set; }

    public POInvolvePartiesDto SupplierContact { get; set; }

    public POResourcesDto POResources { get; set; }

    public string ProjectSequenceCode { get; set; }

    public List<string> Files { get; set; }

    public List<POProduct> ExternalProduct { get; set; }
    public MapLocation MapLocation { get; set; }

    public string LocationId { get; set; }

    public double TotalAmount { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public bool IsCu { get; set; } = false;
}

public class POAllResourcesExcelDto
{
    public string Id { get; set; }

    public string PurchesOrderId { get; set; }

    //public string Title { get; set; }
    public string ResourceNumber { get; set; }
    public DateTime Date { get; set; }
    public string PQuantity { get; set; }
    public string PPurchased { get; set; }
    public string PDeliveryRequested { get; set; }
    public string warf { get; set; }
    public string PConsumed { get; set; }
    public string PInvoiced { get; set; }
    public string CorporateProductCatalogId { get; set; }
    public string BorTitle { get; set; }

    public string BorId { get; set; }

    //public string Mou { get; set; }
    public List<string> PDocuments { get; set; }
    public List<string> CDocuments { get; set; }
    public string PComments { get; set; }
    public string PUnitPrice { get; set; }
    public string PTotalPrice { get; set; }
    public string PCrossReference { get; set; }
    public string CComments { get; set; }
    public string CUnitPrice { get; set; }
    public string CTotalPrice { get; set; }
    public string CCrossReference { get; set; }
    public string CQuantity { get; set; }
    public string CPurchased { get; set; }
    public bool CDeliveryRequested { get; set; }
    public DateTime? CStartDate { get; set; }
    public DateTime? CStopDate { get; set; }
    public string CNumberOfDate { get; set; }
    public string CFullTimeEmployee { get; set; }
    public DateTime? PStartDate { get; set; }
    public DateTime? PStopDate { get; set; }
    public string PNumberOfDate { get; set; }
    public string PFullTimeEmployee { get; set; }
    public string Pdevices { get; set; }
    public string Cdevices { get; set; }
    public string ProjectTitle { get; set; }
    public string NoOfMaterials { get; set; }
    public string NoOfTools { get; set; }
    public string NoOfConsumables { get; set; }
    public string NoOfLabours { get; set; }
    public string PbsTitle { get; set; }

    public string CUCPCTitle { get; set; }

    public string ProjectCPCTitle { get; set; }

    public string CMou { get; set; }

    public string PMou { get; set; }

    public string CCPCId { get; set; }
    public string PCPCId { get; set; }

    public POHeaderExcelDto POHeaderDto { get; set; }

    public bool? IsStock { get; set; }

    public string ResourceType { get; set; }

    public PsCustomerReadDto Customer { get; set; }

    public string ResourceFamily { get; set; }
}

public class VPPOHeaderDto
{
    public string Id { get; set; }
    public string SequenceId { get; set; }
    public string Title { get; set; }
    public string Name { get; set; }
    public string Comments { get; set; }
    public string CustomerOrganisationId { get; set; }
    public string CustomerContactId { get; set; }
    public string SupplierOrganisationId { get; set; }
    public string SupplierContactId { get; set; }
    public string CustomerOrganisation { get; set; }
    public string SupplierOrganisation { get; set; }
    public string CustomerReference { get; set; }
    public string SupplierReference { get; set; }
    public POTypeDto Type { get; set; }
    public string RequestTypeId { get; set; }
    public string RequestTypeName { get; set; }
    public POStatusDto Status { get; set; }
    public POHistoryDto History { get; set; }


    public POInvolvePartiesDto CustomerContact { get; set; }

    public POInvolvePartiesDto SupplierContact { get; set; }

    public POResourcesDto POResources { get; set; }

    public string ProjectSequenceCode { get; set; }

    public List<string> Files { get; set; }

    public List<POProduct> ExternalProduct { get; set; }
    public MapLocation MapLocation { get; set; }

    public string LocationId { get; set; }

    public double TotalAmount { get; set; }

    public DateTime DeliveryDate { get; set; }

    public bool IsCu { get; set; } = false;
    public string DeliveryLocation { get; set; }
    public string WarehouseTaxonomyId { get; set; }
    public DateTime? RequestedDate { get; set; }
    public DateTime? ExpectedDate { get; set; }
    public string RequestedProbability { get; set; }
    public string AvailableProbability { get; set; }
    public IEnumerable<GetPOLabourTeam> Teams { get; set; }
    public IEnumerable<GetPOToolPool> ToolsPool { get; set; }
    public DateTime? CreatedDate { get; set; }
}

public class POResourcesForVpBor
{
    public string Id { get; set; }

    public string PurchesOrderId { get; set; }

    //public string Title { get; set; }
    public string ResourceNumber { get; set; }
    public DateTime? Date { get; set; }
    public string PQuantity { get; set; }
    public string PPurchased { get; set; }
    public string PDeliveryRequested { get; set; }
    public string warf { get; set; }
    public string PConsumed { get; set; }
    public string PInvoiced { get; set; }
    public string CorporateProductCatalogId { get; set; }
    public string BorTitle { get; set; }

    public string BorId { get; set; }

    //public string Mou { get; set; }
    public List<string> PDocuments { get; set; }
    public List<string> CDocuments { get; set; }
    public string PComments { get; set; }
    public string PUnitPrice { get; set; }
    public string PTotalPrice { get; set; }
    public string PCrossReference { get; set; }
    public string CComments { get; set; }
    public string CUnitPrice { get; set; }
    public string CTotalPrice { get; set; }
    public string CCrossReference { get; set; }
    public string CQuantity { get; set; }
    public string CPurchased { get; set; }
    public bool CDeliveryRequested { get; set; }
    public DateTime? CStartDate { get; set; }
    public DateTime? CStopDate { get; set; }
    public string CNumberOfDate { get; set; }
    public string CFullTimeEmployee { get; set; }
    public DateTime? PStartDate { get; set; }
    public DateTime? PStopDate { get; set; }
    public string PNumberOfDate { get; set; }
    public string PFullTimeEmployee { get; set; }
    public string Pdevices { get; set; }
    public string Cdevices { get; set; }
    public string ProjectTitle { get; set; }
    public string NoOfMaterials { get; set; }
    public string NoOfTools { get; set; }
    public string NoOfConsumables { get; set; }
    public string NoOfLabours { get; set; }
    public string PbsTitle { get; set; }

    public string CTitle { get; set; }

    public string PTitle { get; set; }

    public string CMou { get; set; }

    public string PMou { get; set; }

    public string CCPCId { get; set; }
    public string PCPCId { get; set; }

    public POHeaderDto POHeaderDto { get; set; }

    public bool? IsStock { get; set; }

    public string ResourceFamily { get; set; }

    public bool HasChanged { get; set; }
    public string ResourcesType { get; set; }


    public string ResourceTypeId { get; set; }

    public string BorResourceId { get; set; }
    public string StockAvailability { get; set; }

    public bool? IsUsed { get; set; } = false;
    public string UsedPoId { get; set; }
    public DateTime? RequestedDeliveryDate { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }
    public string ResourcesTypeName { get; set; }
    public string WFStatus { get; set; }
    public string WorkFlowId { get; set; }
    public string PbsProductId { get; set; }
    public string PoTitle { get; set; }
    public string ProjectSequenceCode { get; set; }
    public string PoSequenceId { get; set; }
    public DateTime? RequestedDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public DateTime? ExpectedDate { get; set; }
    public string RequestTypeId { get; set; }
    public string RequestTypeName { get; set; }
    public string ProductSequenceId { get; set; }
    public string ProductTitle { get; set; }
    public string POStatusId { get; set; }
}