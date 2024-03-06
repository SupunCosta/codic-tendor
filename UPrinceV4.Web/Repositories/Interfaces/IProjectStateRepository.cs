using System.Collections.Generic;
using System.Threading.Tasks;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IProjectStateRepository
{
    Task<IEnumerable<ProjectState>> GetProjectStates(ApplicationDbContext context, string lang,
        ITenantProvider iTenantProvider);

    Task<Currency> GetProjectStateById(ApplicationDbContext context, string id);
    Task<string> CreateProjectState(ApplicationDbContext context, ProjectStateCreateDto projectStateCreateDto);
    Task<string> UpdateProjectState(ApplicationDbContext context, ProjectStateCreateDto projectStateCreateDto);
    Task<bool> DeleteProjectState(ApplicationDbContext context, string id);
}