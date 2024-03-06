using System.Collections.Generic;
using System.Threading.Tasks;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IProjectTypeRepository
{
    Task<IEnumerable<ProjectTypeDto>> GetProjectTypes(ApplicationDbContext context, string lang,
        ITenantProvider iTenantProvider);

    Task<Currency> GetProjectTypeById(ApplicationDbContext context, string id);
    Task<string> CreateProjectType(ApplicationDbContext context, ProjectTypeCreateDto projectTypeCreateDto);
    Task<string> UpdateProjectType(ApplicationDbContext context, ProjectTypeCreateDto projectTypeCreateDto);
    bool DeleteProjectType(ApplicationDbContext context, string id);
}