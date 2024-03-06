using System.Collections.Generic;
using System.Threading.Tasks;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.ProjectLocationDetails;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces.PBS;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IProjectDefinitionRepository
{
    Task<IEnumerable<ProjectDefinition>> GetProjectDefinition(ApplicationDbContext context);

    Task<ProjectDefinitionDto> GetProjectDefinitionById(ApplicationDbContext context, string id, string lang,
        ITenantProvider iTenantProvider);
    

    Task<ProjectCreateReturnResponse> CreateProjectDefinition(ApplicationDbContext context,
        ProjectDefinitionCreateDto projectDto, IProjectTimeRepository iProjectTimeRepository,
        IProjectFinanceRepository iProjectFinanceRepository, IProjectTeamRepository projectTeamRepository,
        ITenantProvider iTenantProvider, ApplicationUser user, IPbsRepository _IPbsRepository);

    Task<string> UpdateProjectDefinition(ApplicationDbContext context, ProjectDefinitionUpdateDto projectDto,
        IProjectTimeRepository iProjectTimeRepository, IProjectFinanceRepository iProjectFinanceRepository,
        IProjectTeamRepository projectTeamRepository, ITenantProvider iTenantProvider, ApplicationUser user);
    
    Task<string> UpdateProjectDefinitionCopy(ApplicationDbContext context, CreateNewProjectMyEnvDto projectDto,
        IProjectTimeRepository iProjectTimeRepository, IProjectFinanceRepository iProjectFinanceRepository,
        IProjectTeamRepository projectTeamRepository, ITenantProvider iTenantProvider, ApplicationUser user);

    Task<bool> DeleteProjectDefinition(ApplicationDbContext context, string id, ApplicationUser user);

    // This method works for querying history using temporal tables
    Task<IEnumerable<ProjectDefinitionHistoryLog>> GetProjectDefinitionsHistoryLog(ApplicationDbContext context);

    Task<IEnumerable<AllProjectAttributes>> Filter(ApplicationDbContext context, ProjectFilter filter, string lang,
        ITenantProvider iTenantProvider);

    Task<ProjectDropdown> GetDropdownData(ApplicationDbContext context, string lang,
        IProjectManagementLevelRepository projectManagementLevelRepository,
        IProjectToleranceStateRepository projectToleranceStateRepository,
        IProjectTypeRepository projectTypeRepository, IProjectStateRepository projectStateRepository,
        IProjectTemplateRepository projectTemplateRepository, ICurrencyRepository currencyRepository,
        ITenantProvider iTenantProvider, string ProjectSqCode, string Cu);

    Task<string> UpdateProjectViewTime(ApplicationDbContext context, string projectId, ApplicationUser user,
        string lang, ITenantProvider iTenantProvider);

    Task<IEnumerable<ProjectDefinitionLastSeenDto>> ReadLastSeenProjects(ApplicationDbContext context,
        int numberOfRecords, string Oid, ITenantProvider itenantProvider, string Id);

    Task<IEnumerable<ProjectQrCodeDto>> ProjectsForQr(ApplicationDbContext context, ITenantProvider iTenantProvider,
        string title);

    Task<MapLocation> GetLocationByProjectId( string id, string lang,
        ITenantProvider itenantProvider);

    Task<projectData> GetProjectDataById(ApplicationDbContext uPrinceCustomerContext, string id, string lang,
        ITenantProvider itenantProvider);

    Task<string> ProjectsUpdateVAT(ApplicationDbContext context, ITenantProvider iTenantProvider,
        ProjectDefinitionVATDto title);

    Task<ProjectDefinitionDto> GetProjectDefinitionByIdNew(ProjectDefinitionParameter ProjectDefinitionParameter);

    Task<string> AddProjectConfiguration(ApplicationDbContext context, ITenantProvider iTenantProvider,
        ProjectConfigurationDto projectConfiguration);

    Task<ProjectCreateReturnResponse> CreateProjectForTh(ApplicationDbContext context,
        ProjectDefinitionCreateDto projectDto, IProjectTimeRepository iProjectTimeRepository,
        IProjectFinanceRepository iProjectFinanceRepository, IProjectTeamRepository projectTeamRepository,
        ITenantProvider iTenantProvider, ApplicationUser user, IPbsRepository _IPbsRepository);
    


    string AddProjectLocationCopy(ApplicationDbContext context, CreateNewProjectMyEnvDto projectCreateDto,
       CreateNewProjectMyEnvDto projectUpdateDto);

    Task<string> UpdateProjectDefinitionForTh(
        ProjectDefinitionUpdateDto projectDto, IProjectTimeRepository iProjectTimeRepository,
        ITenantProvider iTenantProvider, ApplicationUser user);

    Task<string> CreateTeam(ApplicationDbContext context, ITenantProvider iTenantProvider,
        CreateTeamDto createTeamDto,string Oid);
}

public class ProjectDefinitionParameter
{
    public ApplicationDbContext ApplicationDbContext { get; set; }
    public string Id { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public string Lang { get; set; }
    public string ContractingUnitSequenceId { get; set; }

}