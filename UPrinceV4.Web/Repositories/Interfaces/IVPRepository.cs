using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.BOR;
using UPrinceV4.Web.Data.HR;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Data.PMOL;
using UPrinceV4.Web.Data.VisualPlaane;
using UPrinceV4.Web.Data.VisualPlan;
using UPrinceV4.Web.Data.WH;
using UPrinceV4.Web.Repositories.Interfaces.BOR;
using UPrinceV4.Web.Repositories.Interfaces.CPC;
using UPrinceV4.Web.Repositories.Interfaces.PBS;
using UPrinceV4.Web.Repositories.Interfaces.PMOL;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IVPRepository
{
    Task<VPShortCutPaneCommon> GetShortcutpaneData(VPParameter VPParameter);
    Task<IEnumerable<GetVpPo>> AllVpPo(VPParameter VPParameter);
    Task<IEnumerable<VpPo>> GetVpPo(VPParameter VPParameter);
    Task<IEnumerable<VpPo>> DeleteAllVpPo(VPParameter VPParameter);
    Task<VpPo> UpdateVpPo(VPParameter VPParameter);
    Task<IEnumerable<VpPo>> GetAll(VPParameter VPParameter);
    Task<IEnumerable<BorListDto>> BorList(VPParameter VPParameter);
    Task<IEnumerable<TeamsWithPmolDto>> Teams(VPParameter VPParameter);
    Task<IEnumerable<AvailableLWorkers>> AvailableTeams(VPParameter VPParameter);
    Task<PomlUpdateDto> UpdatePMOL(VPParameter VPParameter);
    Task<IEnumerable<VpWHTaxonomyListDto>> GetVpWH(VPParameter VPParameter);
    Task<IEnumerable<VpWHTaxonomyListDto>> GetVpWHTool(VPParameter VPParameter);
    Task<IEnumerable<TeamsWithPmolDto>> TeamsForCu(VPParameter VPParameter);
    Task<List<VpOrganizationTaxonomyList>> UpdatePersonDate(VPParameter VPParameter);
    Task<string> UpdateWHTaxonomyDate(VPParameter VPParameter);
    Task<GetHRByIdDto> VpHRByPersonId(VPParameter VPParameter);
    Task<List<PbsForVPDto>> GetPbsForVP(VPParameter VPParameter, bool isMyEnv);
    Task<GetByDate> GeTLabourTeamsAndToolsByDate(VPParameter VPParameter);
    Task<string> CreateMilestone(VPParameter VPParameter);
    Task<MilestoneHeaderGetDto> GetMilestoneById(VPParameter VPParameter);
    Task<MilestoneHeaderDropDownData> GetMilestoneDropdown(VPParameter VPParameter);
    Task<IEnumerable<MilestoneList>> GetMilestoneList(VPParameter VPParameter);
    Task<List<PmolShortcutpaneDataDto>> MilestoneShortcutPaneData(VPParameter VPParameter);
    Task<List<PmolShortcutpaneDataDto>> MyEnvMilestoneShortcutPaneData(VPParameter VPParameter);
    Task<List<PmolShortcutpaneDataDto>> CuMilestoneShortcutPaneData(VPParameter VPParameter);
    Task<IEnumerable<VpOrganizationTaxonomyList>> GetVpOrganizationTaxonomyList(VPParameter VPParameter);
    Task<IEnumerable<VpOrganizationTaxonomyList>> GetMTPOrganizationTaxonomyList(VPParameter VPParameter);
    Task<string> AddPmolPlannedLabour(VPParameter VPParameter);
    Task<string> AddPmolPlannedTools(VPParameter VPParameter);
    Task<string> MachineTaxonomyCreate(VPParameter VPParameter);
    Task<IEnumerable<MilestoneListDto>> GetMilestoneListAsTaxonomy(VPParameter VPParameter);
    Task<IEnumerable<GetVpPo>> AllVpPoMyEnv(VPParameter VPParameter);
    Task<List<PbsForVPDto>> GetPbsForVPMyEnv(VPParameter VPParameter, bool isMyEnv);
    Task<IEnumerable<TeamsWithPmolDto>> TeamsMyEnv(VPParameter VPParameter);
    Task<IEnumerable<AvailableLWorkers>> AvailableTeamsMyEnv(VPParameter VPParameter);
    Task<GetByDate> GeTLabourTeamsAndToolsByDateMyEnv(VPParameter VPParameter);
    Task<IEnumerable<TeamsWithPmolDto>> TeamsForPowerBi(VPParameter VPParameter);
    Task<string> UpdateBorPbsForVp(VPParameter VPParameter);
    Task<IEnumerable<VpWHTaxonomyListDto>> GetVpWHMyEnv(VPParameter VPParameter);
    Task<IEnumerable<VpWHTaxonomyListDto>> GetVpWHToolMyEnv(VPParameter VPParameter);
    Task<IEnumerable<VpFilterDropdownDto>> GetVPFilterDropdownData(VPParameter VPParameter);
    Task<PbsTreeStructureDto> GetPbsTreeStructureDataForVp(VPParameter VPParameter);
    Task<List<PbsForVPDto>> GetPbswithpmol(VPParameter VPParameter);
    public IEnumerable<VpProjectWithPm> VPProjectPm(string connection);
    Task<string> UpdatePbsProductTaxonomyDataForVp(VPParameter VPParameter);
    Task<PbsTreeStructure> PBSCloneForVpDto(VPParameter VPParameter);
    Task<string> PbsTreeIndex(VPParameter VPParameter);
    Task<List<DayPlanningFilter>> DayPlanningFilter(VPParameter VPParameter);
    Task<IEnumerable<TeamsWithPmolDto>> DayPlanningListData(VPParameter VPParameter);
    Task<IEnumerable<TeamsWithPmolDto>> DayPlanningListDataProject(VPParameter VPParameter);
    Task<string> PmolAssignDayPanning(VPParameter VPParameter);
    Task<string> CopyTeamswithPmol(VPParameter VPParameter);
    Task<IEnumerable<DayPlanningToolsResults>> GetDayPlanningToolsByCu(VPParameter VPParameter);
    Task<string> PersonAssignDayPlanning(VPParameter VPParameter);
    Task<string> CreateNewTeamDayPlanning(VPParameter VPParameter);
    Task<string> VehicleAssignDayPlanning(VPParameter VPParameter);
    Task<string> ToolAssignDayPlanning(VPParameter VPParameter);
    Task<IEnumerable<GetVpPo>> GetBorForVp(VPParameter VPParameter);
    Task<IEnumerable<GetVpPo>> GetResourceItemsForVp(VPParameter VPParameter);

    Task<IEnumerable<GetVpPo>> GetBorForProjectPlanning(VPParameter VPParameter);
    Task<IEnumerable<GetVpPo>> GetPoForProjectPlanning(VPParameter VPParameter);
    Task<string> TeamAssignForDayPlanning(VPParameter VPParameter);
    Task<IEnumerable<AvailableLWorkers>> AvailableTeamsDayPlanning(VPParameter VPParameter);
    Task<IEnumerable<AvailableLWorkers>> AvailableTeamsDayPlanningProject(VPParameter VPParameter);
    Task<IEnumerable<AvailableLWorkers>> AvailableTeamsDayPlanningCuHr(VPParameter VPParameter);
    Task<IEnumerable<AvailableLWorkers>> AvailableTeamsDayPlanningProjectCuhr(VPParameter VPParameter);
    Task<List<PmolShortcutpaneDataDto>> VpShortcutPaneData(VPParameter VPParameter);
    Task<ProjectsWithPmolDto> ProjectsWithPmol(VPParameter VPParameter);
    Task<ProjectsWithPmolDto> ProjectsWithPmolProjectLevel(VPParameter VPParameter);

    Task<List<ProjectDefinition>> GetProjectsForProjectsDayPlanning(VPParameter VPParameter);
    Task<List<DayPlanningFilter>> DayPlanningFilterForProjectDayPlanning(VPParameter VPParameter);
    Task<string> PmolExecutionDateSet(VPParameter VPParameter);
    Task<string> UpdatePmolStartEndDate(VPParameter VPParameter);
    Task<string> AddTeamMemberToProjectPmols(VPParameter VPParameter);
    Task<string> AddTeamMemberToPmol(VPParameter VPParameter);
    Task<string> AddMultipleMembersToPmol(VPParameter VPParameter,bool isPmol);
    Task<string> AddToolsToProjectPmols(VPParameter VPParameter);
    Task<string> AddToolsToPmol(VPParameter VPParameter);
    Task<List<BuDto>> BuFilterForDayPlanning(VPParameter VPParameter);
    Task<string> CretePrFromBor(VPParameter VPParameter);
    Task<RfqResults> GenerateRFQForProjectDayPlanning(VPParameter VPParameter);
    Task<RfqResults> GenerateRFQNew(VPParameter VPParameter);

    Task<string> DeleteLabourFromProjectDayPlanning(VPParameter VPParameter);
    Task<string> DeleteVpLabourAssign(VPParameter VPParameter);
    Task<string> RemoveLabour(VPParameter VPParameter);
    Task<string> RemoveLabourNew(VPParameter VPParameter);
    Task<PbsTreeStructureDto> GetPbsTreeStructureDataForMyEnv(VPParameter VPParameter);
    Task<List<ProjectData>> ProjectSearchMyEnv(VPParameter VPParameter);
    Task<string> CreateNewProjectMyEnv(VPParameter VPParameter);
    Task<UpdatePmolTitle> UpdatePmolTitleMyEnv(VPParameter VPParameter);
    Task<string> PbsDisplayOrder(VPParameter VPParameter);
    Task<string> TreeIndexSiblingAdd(VPParameter VPParameter);
    Task<string> PbsAssign(VPParameter VPParameter);
    Task<string> ProjectAssign(VPParameter VPParameter);
    Task<List<ProjectDefinition>> ProjectSearchForVp(VPParameter VPParameter);

}

public class VPParameter
{
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public string Id { get; set; }
    public string UserId { get; set; }
    public VpPo VpPo { get; set; }
    public BorFilter BorFilter { get; set; }
    public CorporateSheduleTimeDto CSDto { get; set; }
    public VpPoFilter Filter { get; set; }
    public GetTeamDto GetTeamDto { get; set; }
    public PomlUpdateDto PomlUpdateDto { get; set; }
    public bool IsLabourHistory { get; set; }
    public WHTaxonomyFilterDto WHTaxonomyFilter { get; set; }
    public UpdatePersonsDate UpdatePersonsDate { get; set; }
    public List<string> IdList { get; set; }
    public DateFilter DateFilter { get; set; }
    public MilestoneHeaderCreateDto MilestoneHeaderCreateDto { get; set; }
    public AddPmolPlannedWork AddPmolPlannedWork { get; set; }
    public MilestoneFilter MSFilter { get; set; }
    public PbsForVPDtoFilter PbsForVPDtoFilter { get; set; }
    public OrganizationTaxonomyFilter OrganizationTaxonomyFilter { get; set; }
    public MachineTaxonmyDto MachineTaxonmyDto { get; set; }
    public IPmolRepository IPmolRepository { get; set; }
    public UpdateBorPbsForVp UpdateBorPbsForVp { get; set; }
    public PbsTreeStructureFilter PbsTreeStructureFilter { get; set; }
    public PbsProductTaxonomyTree PbsProductTaxonomyTree { get; set; }
    public TestVp TestVp { get; set; }
    public PBSCloneForVpDto PBSCloneForVpDto { get; set; }
    public PbsTreeIndexDto PbsTreeIndexDto { get; set; }
    public DayPlanningFilterDto DayPlanningFilterDto { get; set; }
    public PmolAssignDayPanningDto PmolAssignDayPanningDto { get; set; }
    public DayPlanningToolsFilter DayPlanningToolsFilter { get; set; }

    public AssignPmolTeam AssignPmolTeam { get; set; }

    public ProjectsPmol ProjectsPmol { get; set; }

    public PmolDrag PmolDrag { get; set; }

    public AddTeamMember AddTeamMember { get; set; }
    public AddMutipleTeamMembers AddMutipleTeamMembers { get; set; }
    public IConfiguration Configuration { get; set; }

    public IPbsResourceRepository PbsResourceRepository { get; set; }

    public AddTools AddTools { get; set; }

    public BorVpFilter BorVpFilter { get; set; }

    public FilterBu FilterBu { get; set; }

    public CreatePr CreatePr { get; set; }
    public IBorRepository BorRepository { get; set; }
    public IProjectDefinitionRepository ProjectDefinitionRepository { get; set; }
    public ApplicationDbContext uPrinceCustomerContext { get; set; }
    public IShiftRepository _iShiftRepository { get; set; }
    public ToolAssignDayPanningDto ToolAssignDayPanningDto { get; set; }

    public DeleteLabour DeleteLabour { get; set; }
    public RemoveLabour RemoveLabour { get; set; }
    public bool IsMyEnv { get; set; }
    public bool IsCu { get; set; }
    public bool MTP { get; set; }

    public CreateNewProjectMyEnvDto CreateProject { get; set; }
    public PbsAssignDto PbsAssignDto { get; set; }
    public ProjectAssignDto ProjectAssignDto { get; set; }
    public ProjectDefinitionCreateDto ProjectDto { get; set; }
    public IPbsRepository _iPbsRepository { get; set; }
    public IBorResourceRepository _iBorResourceRepositoryRepository { get; set; }
    public ICoporateProductCatalogRepository _iCoporateProductCatalogRepository { get; set; }
    public IProjectTeamRepository _iProjectTeamRepository { get; set; }
    public IProjectDefinitionRepository _iProjectDefinitionRepository { get; set; }
    public ICiawRepository _iCiawRepository { get; set; }

    public UpdatePmolTitle UpdatePmolTitle { get; set; }
    public IProjectTimeRepository _iProjectTimeRepository { get; set; }
    public IProjectFinanceRepository _iProjectFinanceRepository { get; set; }
    public ProjectSearchMyEnv ProjectSearchMyEnv { get; set; }
    public List<PbsDisplayOrder> PbsDisplayOrder { get; set; }
    
    public PbsTreeIndexSibling PbsTreeIndexSibling { get; set; }
    public bool IsPmol { get; set; } = false;
    public ProjectSearchForVpDto ProjectSearchForVpDto { get; set; }
}