using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Extensions.Configuration;
using UPrinceV4.Web.Data.GL;
using UPrinceV4.Web.Data.PMOL;
using UPrinceV4.Web.Data.ProjectClassification;
using UPrinceV4.Web.Data.ProjectLocationDetails;
using UPrinceV4.Web.Data.ThAutomation;

namespace UPrinceV4.Web.Data;

public class ProjectDefinition
{
    public string Id { get; set; }
    [Required] public string Description { get; set; }
    public string ProjTypeId { get; set; }
    public string ProjManagementLevelId { get; set; }
    public string ProjToleranceStateId { get; set; }
    public string ProjTemplateId { get; set; }
    public virtual ProjectKPI ProjectKpi { get; set; }
    public string SequenceCode { get; set; }
    public virtual ProjectFinance ProjectFinance { get; set; }
    public virtual ProjectTime ProjectTime { get; set; }
    [NotMapped] public virtual ApplicationUser ChangeByUser { get; set; }
    [NotMapped] public string Action { get; set; }
    [NotMapped] public int RevisionNumber { get; set; }

    public string Name { get; set; }
    [NotMapped] public ProjectDefinitionHistoryLogDto ProjectHistory { get; set; }

    public string Title { get; set; }
    public string ContractingUnitId { get; set; }
    public string ProjectConnectionString { get; set; }
    public string ProjectDefinitionDbNameId { get; set; }
    [ForeignKey("Location")] public string LocationId { get; set; }

    public string GeneralLedgerId { get; set; }

    public string VATId { get; set; }

    public bool IsDeleted { get; set; }
    public string ProjectScopeStatusId { get; set; }
    public string ProjectStatus { get; set; }

    public string CustomerId { get; set; }
    public string Language { get; set; }
    public string ProjectManagerId { get; set; }
    [NotMapped] public SiteManagerParams SiteManager { get; set; }
    public string SiteManagerId { get; set; }

    [NotMapped] public bool IsCiawEnabled { get; set; }
    [NotMapped] public List<PmolTeamMeber> Team { get; set; }
    [NotMapped] public List<LabourCpc> LabourCpc { get; set; }
    
     [NotMapped]  public string BuId { get; set; }

}

public class ProjectDefinitionDto
{
    public string Id { get; set; }
    [Required] public string Description { get; set; }
    public string ProjectTypeId { get; set; }
    public string ProjectManagementLevelId { get; set; }
    public string ProjectToleranceStateId { get; set; }
    public string ProjectTemplateId { get; set; }
    public virtual ProjectKPI ProjectKpi { get; set; }
    public string SequenceCode { get; set; }
    public string ProjectConnectionString { get; set; }
    public string ContractingUnitId { get; set; }
    public string MapLocationId { get; set; }

    public virtual ProjectFinance ProjectFinance { get; set; }
    public virtual ProjectTime ProjectTime { get; set; }
    public virtual ProjectTeamUpdateDto ProjectTeam { get; set; }
    [NotMapped] public virtual ApplicationUser ChangeByUser { get; set; }
    [NotMapped] public string Action { get; set; }
    [NotMapped] public int RevisionNumber { get; set; }

    public string Name { get; set; }
    [NotMapped] public ProjectDefinitionHistoryLogDto ProjectHistory { get; set; }

    [NotMapped] public string Title => SequenceCode + " - " + Name;

    [NotMapped] public MapLocation Location { get; set; }
    public ProjectCostConversionCreateDto ProjectCostConversionCreateDto { get; set; }

    public string GeneralLedgerId { get; set; }

    public GeneralLedgerNumber GenaralLedgerNumber { get; set; }
    public string ProjectScopeStatusId { get; set; }
    public ProjectClassificationHeader ProjectClassification { get; set; }
    public string ProjectStatus { get; set; }

    public string CustomerId { get; set; }
    public string CustomerName { get; set; }

    public string Language { get; set; }
    public string ProjectManagerId { get; set; }
    public string ProjectManagerName { get; set; }
    public string SiteManagerId { get; set; }

    public string SiteManagerName { get; set; }
    public string CiawCode { get; set; }
    public string WbsTemplateId { get; set; }
    public string CustomerCompanyName { get; set; }

}

public class ProjectDefinitionCreateDto
{
    [Required] public string Name { get; set; }
    public string Description { get; set; }
    public string ProjectTypeId { get; set; }
    [Required] public string ProjectManagementLevelId { get; set; }
    [Required] public string ProjectToleranceStateId { get; set; }
    public string ProjectTemplateId { get; set; }
    public ProjectKPICreateDto Kpi { get; set; }
    public ProjectFinanceCreateDto ProjectFinance { get; set; }
    public ProjectTimeCreateDto ProjectTime { get; set; }
    public ProjectTeamCreateDto ProjectTeam { get; set; }
    public MapLocation Location { get; set; }
    public ProjectCostConversionCreateDto ProjectCostConversionCreateDto { get; set; }

    public string GeneralLedgerId { get; set; }
    public string ProjectScopeStatusId { get; set; }
    public string ProjectFinanceStatusId { get; set; }
    public ProjectClassificationCreateDto ProjectClassification { get; set; }
    public string ProjectStatus { get; set; }

    public string CustomerId { get; set; }
    public string Language { get; set; }
    public string ProjectManagerId { get; set; }
    public string SiteManagerId { get; set; }
    public CreateThProjectDto CreateThProjectDto { get; set; }
    public string Id { get; set; }
    public string CiawSiteCode { get; set; }
    public bool IsCiawEnabled { get; set; }
    public string SequenceId { get; set; }
    public IConfiguration _iConfiguration { get; set; }


}

public class ProjectDefinitionUpdateDto
{
    public string Id { get; set; }

    [Required] public string Name { get; set; }
    public string Description { get; set; }
    public string ProjectTypeId { get; set; }
    [Required] public string ProjectManagementLevelId { get; set; }
    [Required] public string ProjectToleranceStateId { get; set; }
    public string ProjectTemplateId { get; set; }

    public ProjectKPIUpdateDto Kpi { get; set; }
    public ProjectFinanceUpdateDto ProjectFinance { get; set; }
    public ProjectTimeUpdateDto ProjectTime { get; set; }
    public ProjectTeamCreateDto ProjectTeam { get; set; }
    public string SequenceCode { get; set; }
    public MapLocation Location { get; set; }
    public ProjectCostConversionCreateDto ProjectCostConversionCreateDto { get; set; }

    public string GeneralLedgerId { get; set; }
    public string ProjectScopeStatusId { get; set; }
    public string ProjectFinanceStatusId { get; set; }
    public ProjectClassificationUpdateDto ProjectClassification { get; set; }
    public string ProjectStatus { get; set; }

    public string CustomerId { get; set; }
    public string Language { get; set; }
    public string ProjectManagerId { get; set; }
    public string SiteManagerId { get; set; }
    public string CiawSiteCode { get; set; }
    public DateTime Date { get; set; }
    public bool IsCiawEnabled { get; set; }
    public string PmolTitle { get; set; }
    public string PmolType { get; set; }
    public string LabourResourceItem { get; set; }
    public string LabourResourceQuantity { get; set; }
    public string Cu { get; set; }

}

public class ProjectDefinitionQrCodeDto
{
    public string Title { get; set; }
}

public class ProjectQrCodeDto
{
    public string Key { get; set; }
    public string Text { get; set; }
}

public class ProjectManager
{
    public string Key { get; set; }
    public string Value { get; set; }
}

public class ProjectCreateReturnResponse
{
    public string ProjectId { get; set; }
    public string TeamId { get; set; }
    public string SequenceId { get; set; }
    
    public string ProjectConnectionString { get; set; }
}

public class ProjectAssignedToUserDto
{
    public string ProjectDefinitionSequenceCode { get; set; }
    public string CabCompanySequenceCode { get; set; }
}

public class ProjectDefToLoadPmol
{
    public string ProjectConnectionString { get; set; }
    public string Id { get; set; }
    public string Name { get; set; }
    public string SequenceCode { get; set; }
    public string CuId { get; set; }
}

public class projectData
{
    public string Title { get; set; }
    public string SequenceCode { get; set; }
    public string PmName { get; set; }
    public string MobileNumber { get; set; }
    public string Company { get; set; }
    public string Image { get; set; }
    public string RoleName { get; set; }
    public string Id { get; set; }
    public string ProjectScopeStatusId { get; set; }
    public string ProjectFinanceStatusId { get; set; }
}

public class ProjectDefinitionMobDto
{
    public string SequenceCode { get; set; }
    public string CuId { get; set; }
    public string Title { get; set; }
}

public class ProjectDefinitionPsDto
{
    public string Project { get; set; }
    public DateTime? Date { get; set; }
}

public class ProjectDefinitionVATDto
{
    public string Id { get; set; }

    [Required] public string VATId { get; set; }
}

public class ProjectDefinitionLastSeenDto
{
    public string SequenceCode { get; set; }
    public string ContractingUnitId { get; set; }
    public string ProjectName { get; set; }
    public string ProjectManager { get; set; }
}

public class ProjectForVpDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public string SequenceCode { get; set; }
}

public class ProjectParam
{
    public string Id { get; set; }
    public string SequenceCode { get; set; }
    public string ProjectId { get; set; }
    public string Lang { get; set; }
}

public class SiteManagerParams
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string FontColor { get; set; }
    public string BgColor { get; set; }
}

public class ProjectSearchMyEnv
{
    public string BuId { get; set; }
    public string Name { get; set; }
}

public class CreateNewProjectMyEnvDto
{
    public string Id { get; set; }

    [Required] public string Name { get; set; }
    public string Description { get; set; }
    public string ProjectTypeId { get; set; }
    [Required] public string ProjectManagementLevelId { get; set; }
    [Required] public string ProjectToleranceStateId { get; set; }
    public string ProjectTemplateId { get; set; }

    public ProjectKPIUpdateDto Kpi { get; set; }
    public ProjectFinanceUpdateDto ProjectFinance { get; set; }
    public ProjectTimeCreateDto ProjectTime { get; set; }
    public ProjectTeamCreateDto ProjectTeam { get; set; }
    public string SequenceCode { get; set; }
    public MapLocation Location { get; set; }
    public ProjectCostConversionCreateDto ProjectCostConversionCreateDto { get; set; }

    public string GeneralLedgerId { get; set; }
    public string ProjectScopeStatusId { get; set; }
    public string ProjectFinanceStatusId { get; set; }
    public ProjectClassificationUpdateDto ProjectClassification { get; set; }
    public string ProjectStatus { get; set; }

    public string CustomerId { get; set; }
    public string Language { get; set; }
    public string ProjectManagerId { get; set; }
    public string SiteManagerId { get; set; }
    public string CiawSiteCode { get; set; }
    public DateTime Date { get; set; }
    public bool IsCiawEnabled { get; set; }
    public string PmolTitle { get; set; }
    public string PmolType { get; set; }
    public string LabourResourceItem { get; set; }
    public string LabourResourceQuantity { get; set; }
    public string Cu { get; set; }
    public bool IsMyCalender { get; set; }

}

public class CreateTeamDto
{
    public string ProjectTitle { get; set; }
    public string Project { get; set; }
}