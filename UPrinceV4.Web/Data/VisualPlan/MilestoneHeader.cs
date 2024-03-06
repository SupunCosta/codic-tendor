using System;
using System.Collections.Generic;

namespace UPrinceV4.Web.Data.VisualPlan;

public class MilestoneHeader
{
    public string Id { get; set; }
    public string SequenceId { get; set; }
    public string Title { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }
    public string Abstract { get; set; }
    public string FullDescription { get; set; }
    public string UtilityTaxonomy { get; set; }
    public string LocationTaxonomy { get; set; }
    public string MachineTaxonomy { get; set; }
    public string AccountableId { get; set; }
    public string ResponsibleId { get; set; }
    public DateTime? RequestedStartDate { get; set; }
    public DateTime? RequestedEndDate { get; set; }
    public DateTime? ExpectedStartDate { get; set; }
    public DateTime? ExpectedEndDate { get; set; }
    public DateTime? ActualStartDate { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public string ExpectedAmmount { get; set; }
    public string ActualAmmount { get; set; }
    public string Comments { get; set; }
    public string CreatedBy { get; set; }
    public string ModifiedBy { get; set; }
    public string Project { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}

public class MilestoneHeaderCreateDto
{
    public string Id { get; set; }
    public string SequenceId { get; set; }
    public string Title { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }
    public string Abstract { get; set; }
    public string FullDescription { get; set; }
    public string UtilityTaxonomy { get; set; }
    public string LocationTaxonomy { get; set; }
    public string MachineTaxonomy { get; set; }
    public string AccountableId { get; set; }
    public string ResponsibleId { get; set; }
    public DateTime? RequestedStartDate { get; set; }
    public DateTime? RequestedEndDate { get; set; }
    public DateTime? ExpectedStartDate { get; set; }
    public DateTime? ExpectedEndDate { get; set; }
    public DateTime? ActualStartDate { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public string ExpectedAmount { get; set; }
    public string ActualAmount { get; set; }
    public string Comments { get; set; }
    public string CreatedBy { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public List<string> Files { get; set; }
    public string Project { get; set; }
}

public class MilestoneHeaderGetDto
{
    public string Id { get; set; }
    public string SequenceId { get; set; }
    public string Title { get; set; }
    public string Name { get; set; }
    public string Abstract { get; set; }
    public string FullDescription { get; set; }
    public string UtilityTaxonomy { get; set; }
    public string LocationTaxonomy { get; set; }
    public string MachineTaxonomy { get; set; }
    public string AccountableId { get; set; }
    public string Accountable { get; set; }
    public string ResponsibleId { get; set; }
    public string Responsible { get; set; }
    public DateTime? RequestedStartDate { get; set; }
    public DateTime? RequestedEndDate { get; set; }
    public DateTime? ExpectedStartDate { get; set; }
    public DateTime? ExpectedEndDate { get; set; }
    public DateTime? ActualStartDate { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public string ExpectedAmount { get; set; }
    public string ActualAmount { get; set; }
    public string Comments { get; set; }
    public List<string> Files { get; set; }
    public string Project { get; set; }
    public MilestoneTypeDto Type { get; set; }
    public MilestoneHistoryDto History { get; set; }
    public MilestoneStatusDto Status { get; set; }
}

public class MilestoneHeaderDropDownData
{
    public IEnumerable<MilestoneTypeDto> Types { get; set; }
    public IEnumerable<MilestoneStatusDto> Status { get; set; }
}

public class MilestoneHistoryDto
{
    public DateTime? CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string CreatedBy { get; set; }
    public string ModifiedBy { get; set; }
}

public class MilestoneFilter
{
    public string Title { get; set; }
    public string StatusId { get; set; }
    public List<string> TypeId { get; set; }
    public Sorter Sorter { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class MilestoneList
{
    public string Id { get; set; }
    public string SequenceId { get; set; }
    public string Title { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }
    public string Abstract { get; set; }
    public string FullDescription { get; set; }
    public string UtilityTaxonomy { get; set; }
    public string LocationTaxonomy { get; set; }
    public string MachineTaxonomy { get; set; }
    public string AccountableId { get; set; }
    public string ResponsibleId { get; set; }
    public DateTime? RequestedStartDate { get; set; }
    public DateTime? RequestedEndDate { get; set; }
    public DateTime? ExpectedStartDate { get; set; }
    public DateTime? ExpectedEndDate { get; set; }
    public DateTime? ActualStartDate { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public string ExpectedAmmount { get; set; }
    public string ActualAmmount { get; set; }
    public string Comments { get; set; }
    public string CreatedBy { get; set; }
    public string ModifiedBy { get; set; }
    public string Project { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string ParentId { get; set; } = null;
    public string MsTypeId { get; set; }
}

public class MilestoneListDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string ParentId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string SequenceId { get; set; }
}