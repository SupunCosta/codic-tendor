using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.CAB;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IProjectTeamRepository
{
    Task<IEnumerable<ProjectTeam>> GetProjectTeams(ApplicationDbContext context);
    Task<Currency> GetProjectTeamById(ApplicationDbContext context, string id);

    Task<string> CreateProjectTeam(ProjectTeamCreateDto projectTeamCreateDto,
        ITenantProvider iTenantProvider, ApplicationUser user);

    Task<string> UpdateProjectTeam(ApplicationDbContext context, ProjectTeamUpdateDto projectTeamUpdateDto);

    Task DeleteProjectTeamRole(ProjectTeamRoleParameter parameter);

    //void CreateTeamRole(ProjectTeamRoleParameter parameter);
    Task<IEnumerable<CabCompanyDto>> GetContractingUnit(ProjectTeamRoleParameter parameter);
    Task<IEnumerable<CabCompanyDto>> GetAllContractingUnit(ProjectTeamRoleParameter parameter);
    Task<IEnumerable<ProjectTeamRoleReadDto>> GetProjectTeam(ProjectTeamRoleParameter parameter);
    Task DeleteProjectAccess(ProjectTeamRoleParameter parameter);
}

public class ProjectTeamRoleParameter
{
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public List<ProjectTeamRoleCreateDto> ProjectDto { get; set; }
    public string Id { get; set; }
    public List<string> IdList { get; set; }
    public string Name { get; set; }
    public string ProjectSequenceCode { get; set; }
}