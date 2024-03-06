using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AzureMapsToolkit.Weather;
using UPrinceV4.Web.Data.ProjectLocationDetails;

namespace UPrinceV4.Web.Data.PMOL;

public class PmolPlannedWorkLabour
{
    public string Id { get; set; }

    // [ForeignKey("CorporateProductCatalog")]
    public string CoperateProductCatalogId { get; set; }

    // public virtual CorporateProductCatalog CorporateProductCatalog { get; set; }
    public double? RequiredQuantity { get; set; }
    public double? ConsumedQuantity { get; set; }
    public string CpcBasicUnitofMeasureId { get; set; }
    [ForeignKey("Pmol")] public string PmolId { get; set; }
    public virtual Pmol Pmol { get; set; }
    public string Type { get; set; }
    public bool IsDeleted { get; set; }
}

public class TeamsWithPmolDto
{
    public List<PmolTeamMeber> Team { get; set; }
    public List<PmolData> Pmol { get; set; }
    public string TeamId { get; set; }
    public string TeamTitle { get; set; }
    public string ProjectSequenceCode { get; set; }
    public string ContractingUinit { get; set; }
}

public class ProjectsWithPmolDto
{
    public List<ProjectDefinition> Projects { get; set; }
    public List<PmolData> Pmol { get; set; }
    public bool IsRfqGenerated { get; set; }
}

public class ProjectsPmol
{
    public DateTime? ExecutionDate { get; set; }
    public List<string> ContractingUnit { get; set; }
    public string BuId { get; set; }
    public string CabPersonId { get; set; }
    public string SiteManagerId { get; set; }
    public string PmolTypeId { get; set; }
    public string ProjectSequenceCode { get; set; }
    
    // public DateTime? StartDate { get; set; }
    // public DateTime? Enddate { get; set; }

}

public class DeleteLabour
{
    public string ProjectSequenceCode { get; set; }
    public string Cu { get; set; }
    public string TeamRoleId { get; set; }
    public string CabPersonId { get; set; }
    public string PmolId { get; set; }
}

public class RemoveLabour
{
    public string TeamId { get; set; }
    public string CabPersonId { get; set; }
    public string NewTeamId { get; set; }
    public DateTime? ExecutionDate { get; set; }
}

public class PmolData
{
    public string Id { get; set; }
    public string ProjectMoleculeId { get; set; }
    public string Name { get; set; }
    public string ExecutionDate { get; set; }
    public string ExecutionStartTime { get; set; }
    public string ExecutionEndTime { get; set; }
    public string ProjectSequenceCode { get; set; }
    public string Title { get; set; }
    public List<PomlVehicle> PomlVehical { get; set; }
    public List<PomlVehicle> PomlTool { get; set; }
    public string ProjectManager { get; set; }
    public string ProjectTitle { get; set; }
    public string TeamId { get; set; }
    public string ProjectSequenceId { get; set; }
    public string ContractingUinit { get; set; }
    public string StatusId { get; set; }
    public string TypeId { get; set; }
    public string ProductId { get; set; }
    public string ForemanId { get; set; }
    public string LocationId { get; set; }

    public List<PmolTeamMeber> Team { get; set; }
    public QuarterDayForecast Forecast { get; set; }
    public bool IsRFQGenerated { get; set; }
    public string ProductTaxonomy { get; set; }
    public string Comment { get; set; }
}

public class PomlList
{
    public List<TeamsWithPmolDto> Teams { get; set; }
}

public class PmolTeamMeber
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string ProjectSequenceId { get; set; }
    public string RoleId { get; set; }
    public string CompanyId { get; set; }
    public string CabPersonId { get; set; }
    public string TeamRoleId { get; set; }
    public string PmolId { get; set; }
    public string CompanyName { get; set; }
    public string RoleName { get; set; }
    public string CabPersonCompanyId { get; set; }
    public string ParentId { get; set; }
    public List<PersonAbsenceData> AbsenceData { get; set; }

}

public class PersonAbsenceData
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string PersonId { get; set; }
    public string Reason { get; set; }
}

public class LabourCpc
{
    public string Id { get; set; }
    public string ResourceTitle { get; set; }
    public string ResourceTypeId { get; set; }
    public string ResourceFamilyId { get; set; }
    public string CpcBasicUnitOfMeasureId { get; set; }
    public string Title { get; set; }
}

public class RfqTeamMeber
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string ProjectSequenceId { get; set; }
    public string RoleId { get; set; }
    public string CompanyId { get; set; }
    public string CabPersonId { get; set; }
    public string TeamRoleId { get; set; }
    public string PmolId { get; set; }
    public string CompanyName { get; set; }
    public double UnitPrice { get; set; }
    public string Quantity { get; set; }
    public string PmolTitle { get; set; }
    public DateTime ExecutionDate { get; set; }
}

public class HRList
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string HRId { get; set; }
    public string CabPersonCompanyId { get; set; }
    public string SequenceId { get; set; }
    public bool Absence { get; set; } = false;
    public string ProjectSequenceCode { get; set; }
    public string Precentage { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string PreferedCpc { get; set; }
    public List<string> ProjectList { get; set; }

}

public class PmolTeam
{
    public string ProjectMoleculeId { get; set; }
    public string Id { get; set; }
    public string CabPersonId { get; set; }
    public string ExecutionEndTime { get; set; }
    public string ExecutionStartTime { get; set; }
    public string Name { get; set; }
    public string ExecutionDate { get; set; }
    public string ProjectSequenceCode { get; set; }
    public string Title { get; set; }
    public string TypeId { get; set; }
}

public class AvailableLabour
{
    public string CabPersonId { get; set; }
    public string ExecutionDate { get; set; }
}

public class AvailableLWorkers
{
    public DateTime day { get; set; }
    public List<HRList> Workers { get; set; }
    public List<PomlVehicle> Vehicle { get; set; }
    public List<PomlVehicle> Tool { get; set; }
    public string ProjectSequenceId { get; set; }
    public string CuId { get; set; }
}

public class GetTeamDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime LocalDate { get; set; }
    public string Type { get; set; }
    public string PmolStatus { get; set; }
    public string ContractingUnit { get; set; }
    public DateTime WeekEndDate { get; set; }
    public string BuId { get; set; }
    public string CabPersonId { get; set; }
    public string PersonName { get; set; }
    public string IsPlanned { get; set; }
    public string PreferedCpc { get; set; }

}

public class CalenderGetTeamDto
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime LocalDate { get; set; }
    public string Type { get; set; }
    public string PmolStatus { get; set; }
    public string ContractingUnit { get; set; }
    public DateTime WeekEndDate { get; set; }
    public string BuId { get; set; }
    public string CabPersonId { get; set; }
    public string PersonName { get; set; }
    public string IsPlanned { get; set; }
    public string PreferedCpc { get; set; }

}
public class PomlVehicle
{
    public string CoperateProductCatalogId { get; set; }
    public string Title { get; set; }
    public string ResourceNumber { get; set; }
    public string PmolId { get; set; }
    public string CuId { get; set; }
    public string AvailableQuantity { get; set; }
    public string ConsumedQuantity { get; set; }
    public string RequiredQuantity { get; set; }
    public string AllocatedQuantity { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string ResourceFamilyId { get; set; }
    public string ProjectSequenceCode { get; set; }
}

public class PomlUpdateDto
{
    public string PmolId { get; set; }
    public string CoperateProductCatalogId { get; set; }
    public string CabPersonId { get; set; }
}

public class DayPlanningFilterDto
{
    public string Project { get; set; }
    public bool IsAllProjects { get; set; } = false;
    public string PmolStatus { get; set; }
    public List<string> ContractingUnit { get; set; }

    public string BuId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class PmolDrag
{
    public string ProjectSequenceCode { get; set; }
    public string Id { get; set; }
    public string PmolId { get; set; }
    public DateTime? ExecutionDate { get; set; }
    public string ContractingUnit { get; set; }
    public string ExecutionStartTime { get; set; }
    public string ExecutionEndTime { get; set; }
}

public class AddTeamMember
{
    public string ProjectSequenceCode { get; set; }
    public string PersonId { get; set; }
    public string PmolId { get; set; }
    public string ContractingUnit { get; set; }
    public DateTime ExecutionDate { get; set; }

    public string CabPersonCompanyId { get; set; }
    public bool IsExist { get; set; }
    public string RoleId { get; set; }

}

public class AddMutipleTeamMembers
{
    public string ProjectSequenceCode { get; set; }
    public string PmolId { get; set; }
    public string BuId { get; set; }
    public string ContractingUnit { get; set; }
    public DateTime ExecutionDate { get; set; }
    public List<TeamMemberDto> Team { get; set; }
}

public class TeamMemberDto
{
    public string PersonId { get; set; }
    public string CabPersonCompanyId { get; set; }
    public bool IsExist { get; set; }
    public string RoleId { get; set; }

}

public class AddTools
{
    public string ProjectSequenceCode { get; set; }
    public string CpcId { get; set; }
    public string PmolId { get; set; }
    public string ContractingUnit { get; set; }
    public bool IsVehicle { get; set; }
    public string CuId { get; set; }
}

public class DayPlanningFilter
{
    public string Project { get; set; }
    public string ProjectSequenceCode { get; set; }
    public List<DayPlanningFilterPbs> Pbs { get; set; }
}

public class DayPlanningFilterPbs
{
    public string Id { get; set; }
    public string ProductId { get; set; }
    public string Name { get; set; }
    public string PbsProductItemTypeId { get; set; }
    public string PbsProductStatusId { get; set; }
    public string PbsToleranceStateId { get; set; }
    public string Scope { get; set; }
    public string Contract { get; set; }
    public string ProductPurpose { get; set; }
    public string ProductComposition { get; set; }
    public string ProductDerivation { get; set; }
    public string ProductFormatPresentation { get; set; }
    public bool IsDeleted { get; set; }
    public string Title { get; set; }
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
    public bool IsPlanOverload { get; set; } = false;
    public List<DayPlanPmol> Pmol { get; set; }
    public int TreeIndex { get; set; }

}
public class PmolAssignDayPanningDto
{
    public string TeamId { get; set; }
    public string Id { get; set; }
    public DateTime ExecutionDate { get; set; }
    public string ExecutionStartTime { get; set; }
    public string ExecutionEndTime { get; set; }
    public string ProjectSequenceCode { get; set; }
    public string ContractingUnit { get; set; }
    public string PersonId { get; set; }
    public string PersonName { get; set; }
    public string CoperateProductCatalogId { get; set; }
    public List<PmolTeamMeber> Team { get; set; }
    public string BuId { get; set; }
    public string CabPersonCompanyId { get; set; }
}

public class ToolAssignDayPanningDto
{
    public string Id { get; set; }
    public string ProjectSequenceCode { get; set; }
    public string ContractingUinit { get; set; }
    public string AllocatedQuantity { get; set; }
    public string RequiredQuantity { get; set; }
    public string CoperateProductCatalogId { get; set; }
}

public class DayPlanningToolsFilter
{
    public List<string> Cu { get; set; }
}

public class DayPlanningToolsResults
{
    public string Id { get; set; }
    public string Cu { get; set; }
    public string CpcId { get; set; }
    public string Title { get; set; }
    public string ResourceNumber { get; set; }
}

public class PlannedWorkLabourOfPMol
{
    public string Id { get; set; }
    public string PmolId { get; set; }
    public bool IsDeleted { get; set; }
}

public class AssignPmolTeam
{
    public string PmolId { get; set; }
    public List<PmolTeamMeber> PmolTeamMember { get; set; }
}

public class PmolLabours
{
    public string PmolId { get; set; }
    public bool IsBreak { get; set; }
    public bool IsForeman { get; set; }
    public string Type { get; set; }
}

public class FilterBu
{
    public string Title { get; set; }
    public string ContractingUnit { get; set; }
    public List<string> BusinessUnit { get; set; }
}

public class BuDto
{
    public string Key { get; set; }
    public string Text { get; set; }
    public string BuSequenceId { get; set; }
    public string BuName { get; set; }
    public string ContractingUnit { get; set; }
    public string ContractingUnitId { get; set; }
}
public class MyCalanderProjectDto
{
    public string ProjectId { get; set; }
    public string ProjectTitle { get; set; }
    public string ContractingUnitId { get; set; }
    public string CabPersonId { get; set; }
    public string SequenceCode { get; set; }
    public string Name { get; set; }
    public string FullName { get; set; }
    public string Oid { get; set; }
}

public class AbsenceHeaderDto
{
    public string Id { get; set; }
    public string Person { get; set; }
    public string LeaveType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public bool AllDay { get; set; }
}

public class PmolPlannedWorkLabourDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string RoleId { get; set; }
}

public class GetPmolByPersonDto
{
    public DateTime? ExecutionDate { get; set; }
    public string CabPersonId { get; set; }
}

public class TrAppDto
{
    public string PmolId { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime StartTime { get; set; }
    public string TypeId { get; set; }
    public string ExecutionDate { get; set; }
    public Position StartPoint { get; set; }
    public Position Destination { get; set; }
    public string CpcId { get; set; }
    public string CpcTitle { get; set; }
    public bool IsDriver { get; set; }
    public string UserId { get; set; }
    public string Latitude { get; set; }
    public string Longitude { get; set; }
}

public class ProjectData
{
    public string Key { get; set; }
    public string Text { get; set; }
    public string SequenceCode { get; set; }

}

public class CreateProject
{
    public string Id { get; set; }
    public string Name { get; set; }
}

public class UpdatePmolTitle
{
    public string Id { get; set; }
    public string ProjectMoleculeId { get; set; }
    public string Name { get; set; }
}

public class MyCalenderListDataDto
{
    public string PersonId { get; set; }
    public string Title { get; set; }
    public string Project { get; set; }
    public string ContractingUnit { get; set; }
    public string OrganizationTeamId { get; set; }
    public string PmolId { get; set; }
}
public class ProjectSearchMyCalender
{
    public string Title { get; set; }
}

public class ProjectCreateMycal
{
    public string Id { get; set; }
    public string Name { get; set; }
    public DateTime? ExecutionDate { get; set; }
    public string Comment { get; set; }
    public string ProductId { get; set; }
    public bool IsFinished { get; set; }
    public string ExecutionStartTime { get; set; }
    public string ExecutionEndTime { get; set; }
    public string LabourCpcId { get; set; }
    public string MouId { get; set; }
    public string ConsumedQuantity { get; set; }
    public string PlannedQuantity { get; set; }
    public string ProjectSequenceId { get; set; }
    public string ForemanId { get; set; }
    public string PmolTypeId { get; set; }
    public string TeamId { get; set; }
    
    public List<PmolCbcResources> Cbc { get; set; }

}