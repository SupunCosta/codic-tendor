using System;
using System.Collections.Generic;
using UPrinceV4.Web.Data.Contractor;
using UPrinceV4.Web.Data.ProjectLocationDetails;

namespace UPrinceV4.Web.Data.PBS_;

public class Pbs
{
    public string PbsProductItemType { get; set; }
    public string ProductId { get; set; }
    public string Title { get; set; }
    public string PbsToleranceState { get; set; }
    public string PbsProductStatus { get; set; }
    public string Id { get; set; }

    public string PbsTaxonomyId { get; set; }
    public string ParentId { get; set; }
    public string Name { get; set; }
    public string Quantity { get; set; }
    public string Key1 { get; set; }
    public string Value1 { get; set; }
    public string Key2 { get; set; }
    public string Value2 { get; set; }
    public string Key3 { get; set; }
    public string Value3 { get; set; }
    public string QualityProducerId { get; set; }
    public string QualityProducerName { get; set; }
    public string QualityProducerCompany { get; set; }

}

public class PbsDto
{
    public string PbsProductItemType { get; set; }
    public string ProductId { get; set; }
    public string Title { get; set; }
    public string HeaderTitle { get; set; }
    public string PbsToleranceState { get; set; }
    public string PbsProductStatus { get; set; }
    public string Parent { get; set; }
    public string Taxonomy { get; set; }
    public string Id { get; set; }
    public string Name { get; set; }
    public string Scope { get; set; }
    public string Contract { get; set; }
    public string ProductPurpose { get; set; }
    public string ProductComposition { get; set; }
    public string ProductDerivation { get; set; }
    public string ProductFormatPresentation { get; set; }
    public string PbsQualityResponsibilityId { get; set; }
    public string QualityApproverId { get; set; }
    public string QualityApprover { get; set; }
    public string QualityProducerId { get; set; }
    public string QualityProducer { get; set; }
    public string QualityReviewerId { get; set; }
    public string QualityReviewer { get; set; }
    public string PbsProductItemTypeId { get; set; }
    public string PbsProductStatusId { get; set; }
    public string PbsToleranceStateId { get; set; }
    public string NodeType { get; set; }
    public string PbsType { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string ParentId { get; set; }
    public string MachineTaxonomyParentId { get; set; }
    public string MeasurementCode { get; set; }
    public string WbsTaxonomyId { get; set; }
    public string PbsLotId { get; set; }
    public string PbsLocation { get; set; }

}

public class PbsGetByIdDto
{
    public string PbsProductItemType { get; set; }
    public string ProductId { get; set; }
    public string Title { get; set; }
    public string HeaderTitle { get; set; }
    public string PbsToleranceState { get; set; }
    public string PbsProductStatus { get; set; }
    public string UtilityTaxonomyParentId { get; set; }
    public string LocationTaxonomyParentId { get; set; }
    public string Id { get; set; }
    public string Name { get; set; }
    public string Scope { get; set; }
    public string Contract { get; set; }
    public string ProductPurpose { get; set; }
    public string ProductComposition { get; set; }
    public string ProductDerivation { get; set; }
    public string ProductFormatPresentation { get; set; }
    public string PbsQualityResponsibilityId { get; set; }
    public string QualityApproverId { get; set; }
    public string QualityApprover { get; set; }
    public string QualityProducerId { get; set; }
    public string QualityProducer { get; set; }
    public string QualityReviewerId { get; set; }
    public string QualityReviewer { get; set; }
    public string PbsProductItemTypeId { get; set; }
    public string PbsProductStatusId { get; set; }
    public string PbsToleranceStateId { get; set; }
    public ProjectDefinitionHistoryLogDto historyLog { get; set; }
    public string PbsType { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string ParentId { get; set; }
    public string MachineTaxonomyParentId { get; set; }
    public string Quantity { get; set; }
    public string MeasurementCode { get; set; }
    public string Mou { get; set; }
    public GetCBCDynamicsAttributes DynamicAttributes { get; set; }
    public PbsScopeOfWorkGetByIdDto PbsScopeOfWork { get; set; }
    public string Project { get; set; }
    public string WbsTaxonomyId { get; set; }
    public string PbsLotId { get; set; }
    public string PbsLocation { get; set; }
    public double? ConsolidatedDuration { get; set; }
    public MapLocation MapLocation { get; set; }

}

public class PbsPo
{
    public string Id { get; set; }
    public string PbsProductItemType { get; set; }
    public string PbsProductItemTypeId { get; set; }
    public string ProductId { get; set; }
    public string Title { get; set; }
    public string PbsToleranceState { get; set; }
    public string PbsToleranceStateId { get; set; }
    public string PbsProductStatus { get; set; }
    public string PbsProductStatusId { get; set; }
    public string PbsTaxonomyId { get; set; }
    public string TotalPrice { get; set; }
}

public class ProductResourceListGetByIdsDto
{
    public IEnumerable<GetLabourForPbsDto> Labour { get; set; }
    public IEnumerable<GetToolsForPbsDto> Tools { get; set; }
}

public class PbsForWeekPlanDto
{
    public string PbsLabourId { get; set; }
    public string PbsProductId { get; set; }
    public string CpcTitle { get; set; }
    public string CpcId { get; set; }
    public string Quantity { get; set; }
}

public class PbsRelationsDto
{
    public bool IsBorDependent { get; set; } = false;
    public bool IsMidTermDependent { get; set; } = false;
}

public class AllPbsForWeekPlanDto
{
    public string PbsId { get; set; }
    public IEnumerable<PbsForWeekPlanDto> PbsLabour { get; set; }
}

public class GetAllPmolLabourDto
{
    public string PmolId { get; set; }
    public string Id { get; set; }
    public string CpcId { get; set; }
    public List<PmolLabour> PmolLabour { get; set; }
}

public class PmolLabour
{
    public string CabPersonId { get; set; }
    public string CabPersonName { get; set; }
    public string RoleId { get; set; }
    public string RoleName { get; set; }
    public string PmolLabourId { get; set; }
}