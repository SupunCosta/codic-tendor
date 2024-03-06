using System;
using System.Collections.Generic;

namespace UPrinceV4.Web.Data.PBS_;

public class PbsTreeStructure
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string ParentId { get; set; }
    public string PbsSequenceId { get; set; }
    public string CpcId { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? CreatedDate { get; set; }
    public int TreeIndex { get; set; }
    public string PbsProductStatusId { get; set; }
    public string NodeType { get; set; }
    public string Type { get; set; }
    public string ProjectSequenceId { get; set; }
    public string Cu { get; set; }
    public string Project { get; set; }
    public string ProjectStatus { get; set; }
    public string projectScopeStatusId { get; set; }
    public string SequenceId { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsFilter { get; set; }
    public string PbsId { get; set; }
    public string CabPersonId { get; set; }
    public string ProjectTitle { get; set; }
    public string Wbs { get; set; }
    public string WbsTaxonomyLevelId { get; set; }
    public string TemplateId { get; set; }
    public double? ConsolidatedDuration { get; set; }


}

public class PbsTreeStructureDto
{
    public IEnumerable<PbsTreeStructure> utilityTaxonomy { get; set; }
    public IEnumerable<PbsTreeStructure> locationTaxonomy { get; set; }
    public IEnumerable<PbsTreeStructure> productTaxonomy { get; set; }
    public IEnumerable<PbsTreeStructure> machineTaxonomy { get; set; }
}

public class PbsForVPDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string ParentId { get; set; }
    public string Type { get; set; }
    public string PBSSequenceId { get; set; }
    public string BORSequenceId { get; set; }
    public string PMOLSequenceId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string CPCId { get; set; }
    public string MilestoneSequenceId { get; set; }
    public string CabPersonId { get; set; }
    public bool IsUsed { get; set; }
    public string PbsStatus { get; set; }
    public string BorStatus { get; set; }
    public string PmolStatus { get; set; }
    public string OrderStatus { get; set; }
    public string IsWarfAvailable { get; set; }

    public int TreeIndex { get; set; }
    public string Cu { get; set; }
    public string Project { get; set; }
    public string ProjectStatus { get; set; }
    
    public string ProjectScopeStatusId { get; set; }

}

public class PbsForVPDtoFilter
{
    public List<string> Type { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Title { get; set; }
    public string ProductStatus { get; set; }
    public bool withParent { get; set; }
    public string ProjectTitle { get; set; }
    public List<string> BusinessUnit { get; set; }
}

public class PbsTreeStructurefroProjectPlanningDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string ParentId { get; set; }
    public string PbsSequenceId { get; set; }
    public string Type { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}