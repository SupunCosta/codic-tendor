using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.PBS_;

public class PbsProduct
{
    public string Id { get; set; }
    public string ProductId { get; set; }
    public string Name { get; set; }

    //[ForeignKey("PbsProductItemType")]
    public string PbsProductItemTypeId { get; set; }
    // public virtual PbsProductItemType PbsProductItemType { get; set; }

    //[ForeignKey("PbsProductStatus")]
    public string PbsProductStatusId { get; set; }
    //public virtual PbsProductStatus PbsProductStatus { get; set; }

    //[ForeignKey("PbsToleranceState")]
    public string PbsToleranceStateId { get; set; }
    //public virtual PbsToleranceState PbsToleranceState { get; set; }

    [ForeignKey("PbsQualityResponsibility")]
    public string PbsQualityResponsibilityId { get; set; }

    public virtual PbsQualityResponsibility PbsQualityResponsibility { get; set; }
    public string Scope { get; set; }
    public string Contract { get; set; }
    public string ProductPurpose { get; set; }
    public string ProductComposition { get; set; }
    public string ProductDerivation { get; set; }
    public string ProductFormatPresentation { get; set; }

    //[ForeignKey("ProjectDefinition")]
    //public string ProjectId { get; set; }
    //public virtual ProjectDefinition Project { get; set; }
    public bool IsDeleted { get; set; }
    public string Title { get; set; }
    [NotMapped] public string HeaderTitle => ProductId + " - " + Name;

    [NotMapped] public ProjectDefinitionHistoryLogDto PbsHistoryLog { get; set; }

    // public virtual IList<PbsProductTaxonomy> PbsProductTaxonomyList { get; set; }
    public string NodeType { get; set; }
    public string PbsTaxonomyLevelId { get; set; }
    public string PbsType { get; set; }
    public string ProjectSequenceCode { get; set; }
    public string ParentId { get; set; }
    public string MachineTaxonomy { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Quantity { get; set; }
    public string MeasurementCode { get; set; }
    public string Mou { get; set; }
    public string QualityProducerId { get; set; }
    public string WbsTaxonomyId { get; set; }
    public string PbsLotId { get; set; }
    public string PbsLocation { get; set; }


}

public class PbsProductCreateDto
{
    public string Id { get; set; }
    [Required] public string Name { get; set; }
    public string ProductId { get; set; }
    public string PbsProductItemTypeId { get; set; }
    public string PbsProductStatusId { get; set; }
    public string PbsToleranceStateId { get; set; }
    public string Scope { get; set; }
    public string Contract { get; set; }
    public string ProductPurpose { get; set; }
    public string ProductComposition { get; set; }
    public string ProductDerivation { get; set; }
    public string ProductFormatPresentation { get; set; }
    public string UtilityTaxonomyParentId { get; set; }
    public string LocationTaxonomyParentId { get; set; }
    public string pbsQualityResponsibilityId { get; set; }
    public string QualityProducerId { get; set; }
    public string QualityReviewerId { get; set; }
    public string QualityApproverId { get; set; }
    public string PbsType { get; set; }
    public string ParentId { get; set; }
    public string MachineTaxonomyParentId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public string ArticleNumber { get; set; }
    public string Quantity { get; set; }
    public string MeasurementCode { get; set; }
    public string Mou { get; set; }
    public string WbsTaxonomyId { get; set; }
    public string PbsLotId { get; set; }
    public string PbsLocation { get; set; }

}

public class PbsProductDto
{
    public string Key { get; set; }
    public string Text { get; set; }
}

public class PbsProductNodeCreateDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string ParentId { get; set; }
    public string PbsTaxonomyLevelId { get; set; }
    public string Key { get; set; }
    public bool IsEdit { get; set; }
}

public class PbsCloneDto
{
    public string Id { get; set; }
}

public class PbsInstructionCloneDto
{
    public string Id { get; set; }
}

public class UploadExcelDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string ParentId { get; set; }
    public string ArticleNo { get; set; }
    public string Quantity { get; set; }
    public string MeasurementCode { get; set; }
    public string Mou { get; set; }
    public string Key1 { get; set; }
    public string Value1 { get; set; }
    public string Key2 { get; set; }
    public string Value2 { get; set; }
    public string Key3 { get; set; }
    public string Value3 { get; set; }
    public string Key4 { get; set; }
    public string Value4 { get; set; }
    public string Key5 { get; set; }
    public string Value5 { get; set; }
}

public class ChildrenDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string ParentNode { get; set; }
    public List<ChildrenDto> Children { get; set; }
    public string ArticleNo { get; set; }
}

public class PBSCloneForVpDto
{
    public string PbsId { get; set; }
    public bool IsClone { get; set; } = false;
    public string TreeType { get; set; }
}

public class CreatePmolDto
{
    public List<CreatePmolCpcDto> Cpc { get; set; }
    public bool IsRepeat { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class CreatePmolCpcDto
{
    public string ProductId { get; set; }
    public string CpcId { get; set; }
    public string ResourceTypeId { get; set; }
}

public class GetPmolLabourDto
{
    public string PmolSequenceId { get; set; }
    public string CpcId { get; set; }
}

public class PbsAssignDto
{
    public string TeamId { get; set; }
    public string Id { get; set; }
    public DateTime WeekStartDate { get; set; }
    public DateTime WeekEndDate { get; set; }
    public DateTime ExecutionDate { get; set; }
    public string ProjectSequenceCode { get; set; }
    public string ContractingUnit { get; set; }
    public string BuId { get; set; }
}

public class ProjectAssignDto
{
    public string TeamId { get; set; }
    public string Id { get; set; }
    public DateTime WeekStartDate { get; set; }
    public DateTime WeekEndDate { get; set; }
    public DateTime ExecutionDate { get; set; }
    public string ProjectSequenceCode { get; set; }
    public string ContractingUnit { get; set; }
    public string BuId { get; set; }
}

public class CpcRelationsDto
{
    public string PbsId { get; set; }
    public string CpcId { get; set; }
}

public class ProjectSearchForVpDto
{
    public string BuId { get; set; }
    public string Title { get; set; }
}

public class MyCalCreatePmolDto
{
    public string PbsId { get; set; }
    public string ProjectSequenceId { get; set; }
    public string ContractingUnitId { get; set; }
    public string Title { get; set; }

}

public class PmolDeliverableResults
{
    public string Title { get; set; }
    public List<string> DocLinks { get; set; }
}

public class PmolDeliverableDto
{
    public string Title { get; set; }
    public string Link { get; set; }
    public string SequenceId { get; set; }

}