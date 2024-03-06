using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using UPrinceV4.Web.Data.BOR;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Data.ProjectLocationDetails;
using UPrinceV4.Web.Data.PS;

namespace UPrinceV4.Web.Data.PMOL;

public class Pmol
{
    public string Id { get; set; }
    public string ProjectMoleculeId { get; set; }
    public string Name { get; set; }
    public string TypeId { get; set; }
    public string StatusId { get; set; }
    public string ForemanMobileNumber { get; set; }
    public DateTime? ExecutionDate { get; set; }
    public string ForemanId { get; set; }
    public string Comment { get; set; }
    public bool IsDeleted { get; set; }
    [ForeignKey("Bor")] public string BorId { get; set; }
    public virtual Bor Bor { get; set; }
    public string Title { get; set; }
    [ForeignKey("Location")] public string LocationId { get; set; }
    public DateTime? StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }
    public bool IsFinished { get; set; }
    public bool IsBreak { get; set; } = false;
    public string PmolType { get; set; }
    public string ProductId { get; set; }
    public string ProjectSequenceCode { get; set; }
    public string ParentId { get; set; }
    public string ExecutionStartTime { get; set; }
    public string ExecutionEndTime { get; set; }
    public string PmolLotId { get; set; }

}

public class PmolListDto
{
    public string Id { get; set; }
    public string ProjectMoleculeId { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public DateTime? ExecutionDate { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }
    public string TypeId { get; set; }
    public string StatusId { get; set; }
    public string Foreman { get; set; }
    public string ForemanId { get; set; }
    public bool IsJobNotDone { get; set; }
}

public class PmolListDtoForMobile
{
    public string Id { get; set; }
    public string ProjectMoleculeId { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public DateTime? ExecutionDate { get; set; }

    public List<PmolBreakDto> Break { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }
    public string TypeId { get; set; }
    public string StatusId { get; set; }
    public string Foreman { get; set; }
    public string ForemanId { get; set; }
    public string ProjectId { get; set; }
    public string TypeNo { get; set; }
    public string PbsId { get; set; }
    public DateTime? StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }
    public ProjectDefinitionMobDto ProjectDefinition { get; set; }

    public PmolResourceReadAllDto PlannedResource { get; set; }

    public PmolResourceReadAllDto ExtraResource { get; set; }

    public bool IsBreak { get; set; }

    public bool IsForeman { get; set; }

    public bool IsLabourStarted { get; set; }
    public bool IsStarted { get; set; }
    public bool IsEnded { get; set; }
    public string ExecutionTime { get; set; }
    public bool IsFinished { get; set; }
    public bool IsShiftStart { get; set; }

}

public class PmolReport
{
    public string ProjectTitle { get; set; }

    public string ProjectManagerName { get; set; }

    public string Id { get; set; }
    public string ProjectMoleculeId { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public string ExecutionDate { get; set; }
    public string PMolTypeType { get; set; }
    public string Status { get; set; }
    public string TypeNo { get; set; }
    public string StartDateTime { get; set; }
    public string EndDateTime { get; set; }
    public string ResourceTitle { get; set; }
    public string MOU { get; set; }
    public string ResourceType { get; set; }
    public string RequiredQuantity { get; set; }
    public string ConsumedQuantity { get; set; }
    public string Type { get; set; }
    public string PMolTitle { get; set; }
    public string BorTitle { get; set; }
    public string PbsProductTitle { get; set; }
}

public class PmolCreateDto
{
    public string Id { get; set; }
    public string Name { get; set; }

    public string TypeId { get; set; }

    //public string UtilityTaxonomyPath { get; set; }
    //public string LocationTaxonomyPath { get; set; }
    public string StatusId { get; set; }
    public string ForemanMobileNumber { get; set; }
    public DateTime? ExecutionDate { get; set; }
    public string ForemanId { get; set; }
    public string Comment { get; set; }
    public string ProductId { get; set; }
    public string Title { get; set; }
    public BorGetByIdDto Bor { get; set; }
    public string LocationId { get; set; }
    public bool IsFinished { get; set; }
    public string PmolType { get; set; }
    public string ParentId { get; set; }
    public string ExecutionStartTime { get; set; }
    public string ExecutionEndTime { get; set; }
    public string PmolLotId { get; set; }

}

public class PmolGetByIdDto
{
    public string Id { get; set; }
    public string ProjectMoleculeId { get; set; }
    public string Name { get; set; }
    public string TypeId { get; set; }
    public string StatusId { get; set; }
    public string ForemanMobileNumber { get; set; }
    public DateTime? ExecutionDate { get; set; }
    public string ForemanId { get; set; }
    public string Comment { get; set; }
    public bool IsDeleted { get; set; }
    public string BorId { get; set; }
    public BorGetByIdDto Bor { get; set; }
    public string Title { get; set; }
    public string Foreman { get; set; }
    public ProjectDefinitionHistoryLogDto historyLog { get; set; }
    public string LocationId { get; set; }
    public string HeaderTitle { get; set; }
    public string TypeNo { get; set; }
    public bool IsFinished { get; set; }
    public string ProductId { get; set; }
    public string ProductTitle { get; set; }
    public bool IsBreak { get; set; }
    public bool hasBreak { get; set; }
    public ProjectDefinitionMobDto ProjectDefinition { get; set; }
    public PmolBreak PmolBreak { get; set; }
    public DateTime? StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }
    public string PmolType { get; set; }
    public string ExecutionStartTime { get; set; }
    public string ExecutionEndTime { get; set; }
    public string MaterialCount { get; set; }
    public string ConsumableCount { get; set; }
    public string LabourCount { get; set; }
    public string ToolsCount { get; set; }
    public bool IsStarted { get; set; }
    public bool IsForeman { get; set; }
    public bool IsLabourStarted { get; set; }
    public string PmolLotId { get; set; }
    public bool IsShiftStart { get; set; }

}

public class PmolDto
{
    public PmolGetByIdDto Header { get; set; }

    public PbsInstructionLoadAllPmolDto Instruction { get; set; }
    public PmolResourceReadAllDto PlannedResource { get; set; }

    public PmolResourceReadAllDto ExtraResource { get; set; }

    public IEnumerable<RiskReadDapperDto> Risk { get; set; }

    public IEnumerable<QualityDapperDto> Quality { get; set; }

    public IEnumerable<PbsSkillExperienceDto> Competencies { get; set; }

    public MapLocation MapLocation { get; set; }

    public PmolStopHandshakeReadDto StopHandshake { get; set; }

    public PmolJournalCreateDto Journal { get; set; }

    public PmolExtraWorkReadDto ExtraWork { get; set; }
    public PsCustomerReadDto Customer { get; set; }
    public PmolServiceGetByIdDto PmolService { get; set; }
}

public class ProjectWithPm
{
    public string ProjectId { get; set; }

    public string ProjectTitle { get; set; }

    public string ContractingUnitId { get; set; }
    public string CabPersonId { get; set; }
    public string SequenceCode { get; set; }
    public string Name { get; set; }
    public string FullName { get; set; }

    public string ProjectConnectionString { get; set; }

    public List<PmolListDtoForMobile> Pmols { get; set; }

    public List<BorListDto> BorList { get; set; }

    public List<Pbs> PbsList { get; set; }
}

public class PmolCreateCommentDto
{
    public string Id { get; set; }

    public string Comment { get; set; }
}

public class DayPlanPmol
{
    public string Id { get; set; }
    public string ProjectMoleculeId { get; set; }
    public string Name { get; set; }
    public string TypeId { get; set; }
    public string StatusId { get; set; }
    public string ForemanMobileNumber { get; set; }
    public DateTime? ExecutionDate { get; set; }
    public string ForemanId { get; set; }
    public string Comment { get; set; }
    public bool IsDeleted { get; set; }
    public string BorId { get; set; }
    public virtual Bor Bor { get; set; }
    public string Title { get; set; }
    public string LocationId { get; set; }
    public DateTime? StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }
    public bool IsFinished { get; set; }
    public bool IsBreak { get; set; } = false;
    public string PmolType { get; set; }
    public string ProductId { get; set; }
    public string ProjectSequenceCode { get; set; }
    public string ParentId { get; set; }
    public string ExecutionStartTime { get; set; }
    public string ExecutionEndTime { get; set; }
    public string ProductName { get; set; }
    public string ContractingUinit { get; set; }
}

public class DayPlanPmolClone
{
    public string PmolId { get; set; }
    public string ContractingUinit { get; set; }
    public string ProjectSequenceCode { get; set; }
    public string TypeId { get; set; }
    public DateTime? ExecutionDate { get; set; }

}

public class MultiplePmolClone
{
    public string PmolId { get; set; }
    public string ContractingUinit { get; set; }
    public string ProjectSequenceCode { get; set; }
    public string TypeId { get; set; }
    public List<DateTime?> ExecutionDate { get; set; }

}

public class BuPersonWithCompetence
{
    public List<BuPersonWithCompetenceTeam> Team { get; set; }
}

public class BuPersonWithCompetenceTeam
{
    public string Name { get; set; }
    public string Id { get; set; }
    public List<PersonCompetence> CompetenceList { get; set; }
}

public class PersonCompetence
{
    public string Id { get; set; }
    public string Name { get; set; }
    public List<WorkerData> Worker { get; set; }
}

public class WorkerData
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string CabPersonCompanyId { get; set; }
    public string CabPersonId { get; set; }
    public string CompanyId { get; set; }
    public string CompetenciesTaxonomyTitle { get; set; }
    public string CompetenciesTaxonomyId { get; set; }
    public string Organization { get; set; }
    public string CompanyName { get; set; }
    public bool IsLabour { get; set; } = false;
    public string CheckboxStatus { get; set; } = "0";
}

public class PmolDataForPrintDto
{
    public string Cu { get; set; }
    public string BuId { get; set; }
    public DateTime Date { get; set; }
}

public class PmolDataForPrint
{
    public string Project { get; set; }
    public string ProjectMolecule { get; set; }
    public string SiteManager { get; set; }
    public string Hour { get; set; }
    public List<string> Names { get; set; }
    public List<string> Material { get; set; }
    public List<string> Vehicle { get; set; }
    public string Address { get; set; }
    public string Id { get; set; }
}