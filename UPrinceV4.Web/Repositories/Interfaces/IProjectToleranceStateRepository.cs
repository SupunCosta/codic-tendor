using System.Collections.Generic;
using System.Threading.Tasks;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IProjectToleranceStateRepository
{
    Task<IEnumerable<ProjectToleranceStateDto>> GetProjectToleranceStates(ApplicationDbContext context, string lang,
        ITenantProvider iTenantProvider);

    Task<Currency> GetProjectToleranceStateById(ApplicationDbContext context, string id);

    Task<string> CreateProjectToleranceState(ApplicationDbContext context,
        ProjectToleranceStateCreateDto projectToleranceStateCreateDto);

    Task<string> UpdateProjectToleranceState(ApplicationDbContext context,
        ProjectToleranceStateCreateDto projectToleranceStateCreateDto);

    bool DeleteProjectToleranceState(ApplicationDbContext context, string id);
}